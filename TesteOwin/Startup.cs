using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Jwt;


[assembly: OwinStartup(typeof(TesteOwin.Startup))]

namespace TesteOwin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseOAuthBearerAuthentication(
                new OAuthBearerAuthenticationOptions()
                {
                    Realm = "https://localhost:44301/",
                    Provider = new OAuthBearerAuthenticationProvider
                    {
                        OnApplyChallenge = (context) => {
                            return Task.FromResult<object>(null);
                        },
                        OnRequestToken = (context) => {
                            return Task.FromResult<object>(null);
                        },
                        OnValidateIdentity = (context) =>
                        {
                            return Task.FromResult<object>(null);
                        }
                    },
                    AccessTokenFormat = new JwtFormat
                                            (
                                                allowedAudience: "https://localhost:44301/",
                                                issuerCredentialProvider: new SymmetricKeyIssuerSecurityTokenProvider
                                                                            (
                                                                                issuer: "https://pasteur",
                                                                                base64Key: "c8wfH2hkyI0nJE6p4KjaqCOK4iVWSbNsPwKHnNVlVhw="
                                                                            )
                                            )
                }
            );

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // configure Web API 
            app.UseWebApi(config);
        }
    }
}
