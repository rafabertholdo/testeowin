using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Util
{
    public static class OAuthUtil
    {
        public static string GetToken()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            var form = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                  { OAuth2Constants.GrantType, OAuth2Constants.Password },
                  { OAuth2Constants.UserName, "rafael.bertholdo@grupoaec.com.br" },
                  { OAuth2Constants.Password, "P@ss0wrd38*" },
                  { OAuth2Constants.Scope, "urn:aec:hospitale" }                  
              });

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
              new BasicAuthenticationHeaderValue(
                "codeflowclient",
                "jksrz9pKTZDE5wWhgwNGIo34o8hfOAkJaC4jTnlUyho=");
            var result = client.PostAsync(new Uri("https://localhost:44305/issue/oauth2/token"), form).Result;
            string response = result.Content.ReadAsStringAsync().Result;
            dynamic dResponse = Newtonsoft.Json.Linq.JObject.Parse(response);
            return dResponse.access_token.Value.ToString();
        }
    }
}
