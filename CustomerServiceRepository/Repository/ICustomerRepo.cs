using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerServiceRepository.Models;

namespace CustomerServiceRepository.Repository
{
    public interface ICustomerRepo
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> GetCustomerDetailsById(int cid);
        Task<CustomerCreationStatus> InsertCustomer(Customer customer);
       
    }
}
