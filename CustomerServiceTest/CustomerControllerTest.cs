using NUnit.Framework;
using CustomerServiceRepository.Repository;
using Moq;
using System.Collections.Generic;
using CustomerServiceRepository.Models;
using System;
using System.Threading.Tasks;
using CustomerServiceApi.Controllers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using CustomerServiceApi.Models;
using System.Net;
using Moq.Protected;
using System.Threading;

namespace CustomerServiceTest
{
    public class CustomerControllerTest
    {
        private Mock<ICustomerRepo> customerRepo;
        HttpClient httpClient;
        CustomersController customerController;
        Mock<HttpMessageHandler> handlerMock = new Mock<HttpMessageHandler>();
        HttpResponseMessage response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };
      
        [SetUp]
        public void Setup()
        {
            handlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);
            httpClient = new HttpClient(handlerMock.Object);
            customerRepo = new Mock<ICustomerRepo>();
            customerController = new CustomersController(httpClient, customerRepo.Object);
        }
        private static  List<Customer> GetCustomers()
        {
            DateTime dob = new DateTime(1990, 12, 31);
            List<Customer> fakeCustomers = new List<Customer> {
                new Customer {CustomerId=100000, Name = "Test1", Address="Test1 Address", DOB=dob.AddYears(-1),PAN_Number="QWERT12345R",Email="test1@gmail.com",CreatedAt=DateTime.Now},
                new Customer {CustomerId=200000, Name = "Test2", Address="Test2 Address",DOB=dob,PAN_Number="QWERT12345S",Email="test1@gmail.com",CreatedAt=DateTime.Now},
                new Customer { CustomerId=300000, Name = "Test3", Address="Test3 Address",DOB=dob.AddYears(1),PAN_Number="QWERT12345T",Email="test1@gmail.com",CreatedAt=DateTime.Now}
                };
            return fakeCustomers;
        }

        [Test]
        public async Task GetAllCustomers_Returns_AllCustomers()
        {

            List<Customer> fakeCustomers =GetCustomers();
            customerRepo.Setup(x => x.GetAllCustomers()).ReturnsAsync(fakeCustomers);
            ActionResult<IEnumerable<Customer>> customers = await customerController.GetAllCustomers();
            OkObjectResult controllerResult = customers.Result as OkObjectResult;
            Assert.AreEqual(200, controllerResult.StatusCode);
            Assert.IsNotNull(customers, "Customer List is null");
            Assert.AreEqual(3, (controllerResult.Value as List<Customer>).Count, "Got wrong number of Customers");
            
           
            Assert.Pass();
        }
        [Test]
        public async Task GetAllCustomers_Returns_NoCustomers()
        {

            List<Customer> emptyCustomers = new List<Customer>();
            customerRepo.Setup(x => x.GetAllCustomers()).ReturnsAsync(emptyCustomers);
            ActionResult<IEnumerable<Customer>> customers = await customerController.GetAllCustomers();
            NoContentResult controllerResult = customers.Result as NoContentResult;
            Assert.AreEqual(204, controllerResult.StatusCode);
            Assert.Pass();
        }
        [Test]
        public async Task GetCustomerDetails_Returns_Customer()
        {
            Customer fakeCustomer = GetCustomers().Find(x=>x.CustomerId== 100000);
            customerRepo.Setup(x => x.GetCustomerDetailsById(100000)).ReturnsAsync(fakeCustomer);
            ActionResult<Customer> customer = await customerController.GetCustomerDetails(100000);
            OkObjectResult controllerResult = customer.Result as OkObjectResult;
            Assert.AreEqual(200, controllerResult.StatusCode);
            Assert.IsNotNull(customer, "No details for Customer");
            Assert.AreEqual("test1@gmail.com", (controllerResult.Value as Customer).Email, "Got Wrong Customer");
        }
        [Test]
        public async Task GetCustomerDetails_Returns_NoCustomer()
        {
            Customer fakeCustomer = new Customer();
            customerRepo.Setup(x => x.GetCustomerDetailsById(100000)).ThrowsAsync(new Exception("No Customer with such id Found")); 
            ActionResult<Customer> customer = await customerController.GetCustomerDetails(100000);
            NotFoundObjectResult controllerResult = customer.Result as NotFoundObjectResult;
            Assert.AreEqual(404, controllerResult.StatusCode);
            Assert.AreEqual("No record found for the user Id :100000", controllerResult.Value as String);
            Assert.Pass();
        }
        [Test]
        public async Task CreateCustomer_Returns_Success()
        {
            Customer fakeCustomer = GetCustomers().Find(x => x.CustomerId == 100000);
            CustomerCreationStatus customerCreationStatus = new CustomerCreationStatus() 
            {
                StatusId = 1,
                CustomerId = fakeCustomer.CustomerId,
                StatusMessage = $"Successfully Created {fakeCustomer.CustomerId}"
            };
            CustomerRequest customerRequest = new CustomerRequest();
            customerRequest.CustomerId = fakeCustomer.CustomerId;
           
            customerRepo.Setup(x => x.InsertCustomer(fakeCustomer)).ReturnsAsync(customerCreationStatus);
            ActionResult<CustomerCreationStatus> status = await customerController.CreateCustomer(fakeCustomer);
            OkObjectResult controllerResult = status.Result as OkObjectResult;
            Assert.AreEqual(200, controllerResult.StatusCode);
            Assert.AreEqual(1, (controllerResult.Value as CustomerCreationStatus).StatusId);
            Assert.AreEqual(fakeCustomer.CustomerId, (controllerResult.Value as CustomerCreationStatus).CustomerId);
            Assert.Pass();
        }
        [Test]
        public async Task CreateCustomer_WithNoDetails()
        {
            Customer fakeCustomer = new Customer();
            CustomerCreationStatus customerCreationStatus = new CustomerCreationStatus();
            CustomerRequest customerRequest = new CustomerRequest();
            customerRequest.CustomerId = fakeCustomer.CustomerId;

            customerRepo.Setup(x => x.InsertCustomer(fakeCustomer)).ReturnsAsync(customerCreationStatus);
            ActionResult<CustomerCreationStatus> status = await customerController.CreateCustomer(fakeCustomer);
            StatusCodeResult controllerResult = status.Result as StatusCodeResult;
            Assert.AreEqual(409, controllerResult.StatusCode);
            Assert.Pass();
        }
    }
}