using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using OAuthImplicitLibrary;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.WsFederation;
using System.IdentityModel.Claims;
using System.Net.Security;
using System.Xml;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security.Tokens;
[assembly: OwinStartup(typeof(WebClientOwin.Startup))]

namespace WebClientOwin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //// Configure the db context and user manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            //app.UseCookieAuthentication(new CookieAuthenticationOptions { });
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            //app.UseOAuthImplicitAuthentication(new OAuthImplicitAuthenticationOptions
            //{
            //    AuthorizeEndpoint = new Uri("https://pasteur/issue/oauth2/authorize"),
            //    ClientId = "testewebapi",
            //    ClientSecret = "7/Cp5O/kqBfTQT5aW0kMWomlfDm6rXOViH7lI5tYehU=",
            //    Scope = "https://localhost:44301/",
            //    RedirectUri = new Uri("https://localhost:44301/"),
            //    ResponseType = "code",
            //    Issuer = "https://pasteur",
            //    AllowedAudience = "https://localhost:44301/",
            //    SymmetricSigningKey = "c8wfH2hkyI0nJE6p4KjaqCOK4iVWSbNsPwKHnNVlVhw="
            //});

            app.SetDefaultSignInAsAuthenticationType(WsFederationAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(
                new CookieAuthenticationOptions
                {                    
                    AuthenticationType =
                       WsFederationAuthenticationDefaults.AuthenticationType
                });
            app.UseWsFederationAuthentication(
                new WsFederationAuthenticationOptions
                {
                    MetadataAddress = "https://pasteur/FederationMetadata/2007-06/FederationMetadata.xml",
                    Wtrealm = "https://localhost:44301/",
                    BackchannelCertificateValidator = new EmptyCertificateValidator(),                    
                    SecurityTokenHandlers = new System.IdentityModel.Tokens.SecurityTokenHandlerCollection{
                        new SoapJwtSecurityTokenHandler()
                    },
                    //TokenValidationParameters = new TokenValidationParameters
                    //{
                    //    IssuerSigningToken = new BinarySecretSecurityToken(Convert.FromBase64String("c8wfH2hkyI0nJE6p4KjaqCOK4iVWSbNsPwKHnNVlVhw=")),
                    //},
                    Wreply = "https://localhost:44301/signin",
                    Notifications = new WsFederationAuthenticationNotifications
                    {
                        AuthenticationFailed = context =>
                        {
                            return Task.FromResult<object>(null);
                        },
                        MessageReceived = context =>
                        {
                            return Task.FromResult<object>(null);
                        },
                        RedirectToIdentityProvider = context =>
                        {                            
                            return Task.FromResult<object>(null);
                        },
                        SecurityTokenReceived = context =>
                        {                            
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(context.SecurityToken);

                            context.SecurityToken = doc.InnerText;
                            context.OwinContext.Request.Headers.Add("Token", new string[] { DecodeUtil.Base64Decode(doc.InnerText) });
                            return Task.FromResult<object>(null); 
                        },
                        SecurityTokenValidated = context =>
                        {                            
                            context.AuthenticationTicket.Identity.AddClaim(new System.Security.Claims.Claim("urn:thinktecture:token", context.OwinContext.Request.Headers["Token"]));
                            return Task.FromResult<object>(null);
                        }
                    }
                });

            //app.UseOpenIdConnectAuthentication(
            //    new OpenIdConnectAuthenticationOptions
            //    {
            //        Client_Id = "TesteWebApi",
            //        Authority = "https://pasteur"
            //});
        }
    }

    public class SoapJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        public override bool CanReadToken(string tokenString)
        {
            return true;
        }
        
        public override System.Security.Claims.ClaimsPrincipal ValidateToken(string jwtEncodedString, TokenValidationParameters validationParameters)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(jwtEncodedString);

            string decoded = DecodeUtil.Base64Decode(doc.InnerText);
            validationParameters.IssuerSigningToken = new BinarySecretSecurityToken(Convert.FromBase64String("c8wfH2hkyI0nJE6p4KjaqCOK4iVWSbNsPwKHnNVlVhw=")); 
            return base.ValidateToken(decoded, validationParameters);
        }        
    }

    public static class DecodeUtil
    {
        public static string Base64Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }
    }
    
    public class EmptyCertificateValidator : Microsoft.Owin.Security.ICertificateValidator
    {
        public bool Validate(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
