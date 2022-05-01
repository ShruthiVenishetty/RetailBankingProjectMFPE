using Newtonsoft.Json;
using RetailBankingMVC.Models.AuthenticationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RetailBankingMVC.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private string baseurl = "http://localhost:9000/AuthenticationService";


        public async Task<HttpResponseMessage> Login(LoginData loginData)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseurl + "/Login");
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<LoginData>(httpClient.BaseAddress, loginData);
                return response;
            }
        }

        public async Task<HttpResponseMessage> Register(RegisterData registerData)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                StringContent content1 = new StringContent(JsonConvert.SerializeObject(registerData), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(baseurl + "/Register", content1);
               
                return response;
            }
        }
    }
}
