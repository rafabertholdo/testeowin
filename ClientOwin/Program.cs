﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Clients;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Tokens.Http;
using Util;

namespace ClientOwin
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            string token = OAuthUtil.GetToken();

            using (var owinClient = new HttpClient())
            {
                owinClient.BaseAddress = new Uri("http://localhost:3209/");
                owinClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent("UserName=ze@grupoaec.com.br");
                using (var response = owinClient.PostAsync("/api/UsuarioCentralAdmin",content).Result)
                {
                    response.EnsureSuccessStatusCode();
                }                    
                
                using (var owinResponse = owinClient.GetAsync("/api/testes").Result)
                {
                    owinResponse.EnsureSuccessStatusCode();
                    var owinContent = owinResponse.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(owinContent);
                }
            }            
        }
    }
}
