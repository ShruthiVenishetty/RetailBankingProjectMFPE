using Newtonsoft.Json;
using RetailBankingMVC.Controllers;
using RetailBankingMVC.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RetailBankingMVC.Services
{
    public class CustomerService : ICustomerService
    {
        private string baseurl = "http://localhost:9000/CustomerService/Customers";

        public async Task<HttpResponseMessage> GetAllCustomers( string token)
        {
            using (HttpClient client = new HttpClient())
            {

                MediaTypeWithQualityHeaderValue ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);

                client.BaseAddress = new Uri(baseurl);
                client.DefaultRequestHeaders.Accept.Add(ContentType);
                HttpResponseMessage response = await client.GetAsync("");
                return response;
            }
        }

        public async Task<HttpResponseMessage> GetCustomerDetailsById(int customerId,string token)
        {
            using (HttpClient client = new HttpClient())
            {
                var ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);

                client.BaseAddress = new Uri(baseurl+$"/{customerId}");
                client.DefaultRequestHeaders.Accept.Add(ContentType);
                var response = await client.GetAsync("");
                return response;
            }
        }

        public async Task<HttpResponseMessage> InsertCustomer(Customer customer, string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseurl);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
                HttpResponseMessage response = await httpClient.PostAsJsonAsync<Customer>(httpClient.BaseAddress, customer);
                return response;
            }
        }
    }
}
