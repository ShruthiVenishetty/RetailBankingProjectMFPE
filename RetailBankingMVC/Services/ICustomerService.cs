using RetailBankingMVC.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailBankingMVC.Services
{
    public interface ICustomerService
    {
        Task<HttpResponseMessage> GetAllCustomers(string token);
        Task<HttpResponseMessage> GetCustomerDetailsById(int customerId,string token);
        Task<HttpResponseMessage> InsertCustomer(Customer customer, string token);
    }
}
