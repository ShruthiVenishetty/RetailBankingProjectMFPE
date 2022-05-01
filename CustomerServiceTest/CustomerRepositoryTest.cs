using CustomerServiceRepository.Models;
using CustomerServiceRepository.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomerServiceTest
{
    public class CustomerRepositoryTest
    {

        private ICustomerRepo customerRepo;
        [SetUp]
        public void Setup()
        {
            customerRepo = new CustomerRepo();
        }

        [Test]
        public async Task GetAllCustomers_Returns_AllCustomers()
        {
            List<Customer> customers = await customerRepo.GetAllCustomers();
            Assert.IsNotNull(customers, "Customer List is null");
            Assert.Pass();
        }


        [TestCase(102030,"Tada Nikhil Reddy")]
        [TestCase(102031, "Akhil Reddy")]
        public async Task GetCustomerDetailsById_Returns_Customer(int customerId,string expectedName)
        {
            Customer customer = await customerRepo.GetCustomerDetailsById(customerId);
            Assert.IsNotNull(customer, "Customer is null");
            Assert.AreEqual(customer.Name, expectedName);
            Assert.Pass();
        }
        [Test]
        public async Task CreateCustomer_Returns_Success()
        {
            DateTime dob = new DateTime(1990, 12, 31);
            List<Customer> customers = await customerRepo.GetAllCustomers();
            int BeforeInsertCount = customers.Count;
            Customer customer = new Customer { CustomerId = 190001, Name = "Shayam", Address = "Shamerpet", DOB = dob.AddYears(-1), PAN_Number = "QWERT12345R", Email = "shyam@gmail.com", CreatedAt = DateTime.Now };
            CustomerCreationStatus customerCreationStatus = await customerRepo.InsertCustomer(customer);
            customers = await customerRepo.GetAllCustomers();
            int AfterInsertCount = customers.Count;
            Assert.AreEqual(AfterInsertCount, BeforeInsertCount+1);
            Assert.AreEqual(1,customerCreationStatus.StatusId);
            Assert.AreEqual(190001, customerCreationStatus.CustomerId);
            Assert.AreEqual(customerCreationStatus.StatusMessage, "Successfully Created 190001");
            Assert.Pass();
        }
       
    }
}