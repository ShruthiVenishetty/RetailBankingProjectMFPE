using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RetailBankingMVC.Models.CustomerModels;
using RetailBankingMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailBankingMVC.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        string token;
        public CustomersController(ICustomerService customerService)
        {
           
            _customerService = customerService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CreateCustomer()
        {
          
            if (HttpContext.Session.GetString("IsEmployee") == null)
            {
                return RedirectToAction("Login", "Authentication");
            }
            else if (HttpContext.Session.GetString("IsEmployee") == "False")
            {
                return RedirectToAction("UnAuthorized", "Authentication");
            }
            else
            {
                return View();
            }
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer(Customer customer)
        {
             token = HttpContext.Session.GetString("token");
            if (HttpContext.Session.GetString("IsEmployee") == null)
            {
                return RedirectToAction("Login", "Authentication");
            }
            else
            {
                if (!ModelState.IsValid)
                    return View(customer);

                CustomerCreationStatus createSuccess = new CustomerCreationStatus();
                HttpResponseMessage response = await _customerService.InsertCustomer(customer,token);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    String jsonContent = await response.Content.ReadAsStringAsync();

                    createSuccess = JsonConvert.DeserializeObject<CustomerCreationStatus>(jsonContent);
                    return View("CreateSuccess", createSuccess);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    ModelState.AddModelError("", "Having server issue while adding record");
                    return View(customer);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    ModelState.AddModelError("", "Username already present with ID :" + customer.CustomerId);
                    return View(customer);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError("", "Invalid model states");
                    return View(customer);
                }

                return View(customer);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            token = HttpContext.Session.GetString("token");
            if (HttpContext.Session.GetString("IsEmployee") == null)
            {
                return RedirectToAction("Login", "Authentication");
            }

            else
            {
                List<Customer> customers = new List<Customer>();

                HttpResponseMessage response = await _customerService.GetAllCustomers(token);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    String JsonContent = await response.Content.ReadAsStringAsync();
                    customers = JsonConvert.DeserializeObject<List<Customer>>(JsonContent);
                    return View(customers);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    ViewBag.Message = "Having server issue while adding record";
                    return View(customers);
                }




                return View(customers);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCustomerDetails(int customerId)
        {
            token = HttpContext.Session.GetString("token");
            if (HttpContext.Session.GetString("IsEmployee") != "True" && HttpContext.Session.GetString("IsCustomer") != "True")
            {
                return RedirectToAction("Login", "Authentication");
            }
            else
            {
                Customer customer = new Customer();

                HttpResponseMessage response = await _customerService.GetCustomerDetailsById(customerId, token);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JsonContent = await response.Content.ReadAsStringAsync();
                    customer = JsonConvert.DeserializeObject<Customer>(JsonContent);
                    return View(customer);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    ViewBag.Message = "No any record Found! Bad Request";
                    return View(customer);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    ViewBag.Message = "Having server issue while adding record";
                    return View(customer);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.Message = "No record found in DB for ID :" + customerId;
                    return View(customer);
                }

                return View(customer);
            }

        }
    }
}
