using NUnit.Framework;
using AccountServiceRepository.Repository;
using Moq;
using System.Collections.Generic;
using AccountServiceRepository.Models;
using System;
using System.Threading.Tasks;
using AccountServiceApi.Controllers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using AccountServiceApi.Models;
using System.Net;
using Moq.Protected;
using System.Threading;
namespace AccountServiceTest
{
    public class AccountControllerTest
    {
        private Mock<IAccountRepo> accountRepo;
        AccountsController accountController;
        [SetUp]
        public void Setup()
        {
            accountRepo = new Mock<IAccountRepo>();
            accountController = new AccountsController(accountRepo.Object);
        }
        private static List<Account> GetAccounts()
        {
           
            List<Account> fakeAccounts = new List<Account> {
                new Account {AccountId=100000, AccountTypeCode=200, CurrentBalance=1000, CustomerId=123456 },
                new Account {AccountId=200000,AccountTypeCode=200, CurrentBalance=1000, CustomerId=123456 },
                new Account {AccountId=300000, AccountTypeCode=200, CurrentBalance=1000, CustomerId=123456}
                };
            return fakeAccounts;
        }

        [Test]
        public async Task GetAccountDetails_Returns_Account()
        {
            Account fakeAccount = GetAccounts().Find(x => x.AccountId == 100000);
            accountRepo.Setup(x => x.GetAccountDetailsById(100000)).ReturnsAsync(fakeAccount);
            ActionResult<Account> account = await accountController.GetAccount(100000);
            OkObjectResult controllerResult = account.Result as OkObjectResult;
            Assert.AreEqual(200, controllerResult.StatusCode);
            Assert.IsNotNull(account, "No details for Account");
            Assert.AreEqual(200, (controllerResult.Value as Account).AccountTypeCode, "Got Wrong Account");
        }
        [Test]
        public async Task GetAccountDetails_Returns_NoAccount()
        {
            Account fakeCustomer = new Account();
            accountRepo.Setup(x => x.GetAccountDetailsById(100001)).ThrowsAsync(new Exception());
            ActionResult<Account> customer = await accountController.GetAccount(100000);
            NotFoundObjectResult controllerResult = customer.Result as NotFoundObjectResult;
            Assert.AreEqual(404, controllerResult.StatusCode);
            Assert.AreEqual("No account with that Id Found", controllerResult.Value as String);
            Assert.Pass();
        }
        [Test]
        public async Task CreateAccount_Returns_Success()
        {
            int fakeCustomerId = 1000000;
            AccountCreationStatus accountCreationStatus = new AccountCreationStatus()
            {
                StatusId = 1,
                StatusMessage = $"Successfully Created 2 accounts for { fakeCustomerId}"
            };
            CustomerRequest accountRequest = new CustomerRequest();
            accountRequest.CustomerId = fakeCustomerId;
            accountRepo.Setup(x => x.InsertAccount(fakeCustomerId)).ReturnsAsync(accountCreationStatus);
            ActionResult<AccountCreationStatus> status = await accountController.createAccount(accountRequest);
            OkObjectResult controllerResult = status.Result as OkObjectResult;
            Assert.AreEqual(200, controllerResult.StatusCode);
            Assert.AreEqual(1, (controllerResult.Value as AccountCreationStatus).StatusId);
            Assert.AreEqual($"Successfully Created 2 accounts for 1000000", (controllerResult.Value as AccountCreationStatus).StatusMessage);
            Assert.Pass();
        }
        [Test]
        public async Task CreateAccount_WithNoDetails()
        {
            int fakeCustomerId = 0;
            AccountCreationStatus accountCreationStatus = new AccountCreationStatus();
            CustomerRequest accountRequest = new CustomerRequest();
            accountRequest.CustomerId = fakeCustomerId;
            accountRepo.Setup(x => x.InsertAccount(fakeCustomerId)).ThrowsAsync(new Exception());
            ActionResult<AccountCreationStatus> status = await accountController.createAccount(accountRequest);
            BadRequestObjectResult controllerResult = status.Result as BadRequestObjectResult;
            Assert.AreEqual(400, controllerResult.StatusCode);
            Assert.Pass();
        }
    }
}