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
                  { OAuth2Constants.UserName, "dominick" },
                  { OAuth2Constants.Password, "123456" },
                  { OAuth2Constants.Scope, "https://localhost:44301/" }                  
              });

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
              new BasicAuthenticationHeaderValue(
                "testewebapi",
                "7/Cp5O/kqBfTQT5aW0kMWomlfDm6rXOViH7lI5tYehU=");
            var result = client.PostAsync(new Uri("https://pasteur/issue/oauth2/token"), form).Result;
            string response = result.Content.ReadAsStringAsync().Result;
            dynamic dResponse = Newtonsoft.Json.Linq.JObject.Parse(response);
            return dResponse.access_token.Value.ToString();
        }
    }
}
