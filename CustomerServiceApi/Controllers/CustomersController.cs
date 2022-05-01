using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerServiceRepository.Models;
using CustomerServiceRepository.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Json;
using Newtonsoft.Json;
using CustomerServiceApi.Models;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Identity.Web;

namespace CustomerServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepo customerRepo;
        HttpClient httpClient;
        public CustomersController(HttpClient client,ICustomerRepo icustomerRepo)
        {           
            httpClient = client;
            httpClient.BaseAddress = new Uri("http://localhost:9000/AccountService/Acccounts");
            customerRepo = icustomerRepo;
        }
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            try
            {
                List<Customer> customers = await customerRepo.GetAllCustomers();
                if (customers.Count!=0)
                {
                    return Ok(customers);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception e)
            {
                return new StatusCodeResult(500);
            }

        }
        [HttpGet("{customerId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<Customer>> GetCustomerDetails(int customerId)
        {
            
            try
            {
                Customer customer = await customerRepo.GetCustomerDetailsById(customerId);
                return Ok(customer);
                
               
            }
            catch (Exception e)
            {
                if(e.Message== "No Customer with such id Found")
                { 
                    return NotFound("No record found for the user Id :" + customerId);
                }
                else
                {
                    return new StatusCodeResult(500);
                }               
            }
          
        }
        private void PublishToMessageQueue(string integrationEvent, string eventData)
        {
            var factory = new ConnectionFactory();
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(eventData);
            channel.BasicPublish(exchange: "customer", routingKey: integrationEvent, basicProperties: null, body: body);
        }
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<CustomerCreationStatus>> CreateCustomer([FromBody]Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please enter Correct Details");
            }
            try
            {

                CustomerCreationStatus status= await customerRepo.InsertCustomer(customer);

                if (status.StatusId==1)
                {                  
                    CustomerRequest customerRequest = new CustomerRequest();
                    customerRequest.CustomerId = customer.CustomerId;
                    await httpClient.PostAsJsonAsync("",customerRequest);
                    String integrationEventData = JsonConvert.SerializeObject(new { CustomerId = customer.CustomerId, Email = customer.Email });
                    PublishToMessageQueue("customer.add", integrationEventData);
                    return Ok(status);
                }
                else
                {
                    //The request could not be completed due to a conflict with the current state
                    //of the target resource. This code is used in situations where the user might
                    //be able to resolve the conflict and resubmit the request.
                    return new StatusCodeResult(409);
                }
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }


          
        }
    }
}
