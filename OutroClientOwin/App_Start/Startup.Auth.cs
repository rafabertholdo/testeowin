using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using System;
using OutroClientOwin.Models;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.WsFederation;

namespace OutroClientOwin
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            //app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = OpenIdConnectAuthenticationDefaults.AuthenticationType,                
            //});
            
            ////app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);            

            ////app.UseCookieAuthentication(new CookieAuthenticationOptions { });

            //app.UseOpenIdConnectAuthentication(
            //    new OpenIdConnectAuthenticationOptions
            //    {
            //        Client_Id = "TesteWebApi",
            //        Client_Secret = "P@ssw0rd",
            //        Authority = "https://pasteur",                    
            //    });


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
                    Wtrealm = "urn:mvcjwtrp",
                });            
        }
    }
}