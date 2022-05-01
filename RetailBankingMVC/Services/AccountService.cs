using Microsoft.AspNetCore.Http;
using RetailBankingMVC.Controllers;
using RetailBankingMVC.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RetailBankingMVC.Services
{
    public class AccountService : IAccountService
    {
		private string baseurl = "http://localhost:9000/AccountService/Acccounts";
		public async Task<HttpResponseMessage> GetAccountDetailsByCid(int customerId,string token)
        {
			using (HttpClient client = new HttpClient())
			{
				MediaTypeWithQualityHeaderValue ContentType = new MediaTypeWithQualityHeaderValue("application/json");
				client.BaseAddress = new Uri(baseurl+ "/GetCustomerAccounts/" + customerId);
				client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);

				client.DefaultRequestHeaders.Accept.Add(ContentType);
				HttpResponseMessage response = await client.GetAsync("");
				return response;
			}
		}

       
    }
}
