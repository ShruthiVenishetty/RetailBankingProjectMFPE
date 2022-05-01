using RetailBankingMVC.Models.AuthenticationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace RetailBankingMVC.Services
{
    public interface IAuthenticationService
    {
        Task<HttpResponseMessage> Login(LoginData loginData);
        Task<HttpResponseMessage> Register(RegisterData registerData);
    }
}
