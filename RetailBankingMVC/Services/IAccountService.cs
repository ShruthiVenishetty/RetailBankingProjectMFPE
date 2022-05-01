using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RetailBankingMVC.Models.AccountModels;

namespace RetailBankingMVC.Services
{
    public interface IAccountService
    {
        Task<HttpResponseMessage> GetAccountDetailsByCid(int customerId,string token);
      

    }
}
