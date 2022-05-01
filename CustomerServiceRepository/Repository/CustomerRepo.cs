using CustomerServiceRepository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceRepository.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        CustomerDBContext dc = new CustomerDBContext();
        public async Task<List<Customer>> GetAllCustomers()
        {
            return await dc.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomerDetailsById(int cid)
        {
            try
            {
                Customer customer =await (from c in dc.Customers where c.CustomerId == cid select c).FirstAsync();
                return customer;
            }
            catch (Exception)
            {
                throw new CustomerServiceException("No Customer with such id Found");
                
            }
        }

        public async Task<CustomerCreationStatus> InsertCustomer(Customer customer)
        {
            try
            {
                customer.CreatedAt = DateTime.Now;
                await dc.Customers.AddAsync(customer);
                await dc.SaveChangesAsync();
                Customer cust = await GetCustomerDetailsById(customer.CustomerId);
                CustomerCreationStatus customerCreationStatus = new CustomerCreationStatus();
                if (cust != null)
                {
                    customerCreationStatus.StatusId = 1;
                    customerCreationStatus.CustomerId = cust.CustomerId;
                    customerCreationStatus.StatusMessage = $"Successfully Created {cust.CustomerId}";
                }
                else
                {
                    customerCreationStatus.StatusId = 0;
                    customerCreationStatus.CustomerId = cust.CustomerId;
                    customerCreationStatus.StatusMessage = $"Not Created {cust.CustomerId}";
                }
                return customerCreationStatus;

            }
            catch (Exception)
            {
                throw new CustomerServiceException("Error While Creating and Updating a Customer");
            }

        }
         
    }
}
