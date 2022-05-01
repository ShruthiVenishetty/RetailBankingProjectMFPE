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
using RichardSzalay.MockHttp;

namespace AccountServiceTest
{
    public class AccountTransactionsControllerTest
    {
        private Mock<IAccountRepo> accountRepo;
        HttpClient httpClient;
        AccountTransactionsController accountTransactionsController;
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
            accountRepo = new Mock<IAccountRepo>();
            accountTransactionsController = new AccountTransactionsController(httpClient,accountRepo.Object);
        }
        private static List<Account> GetAccounts()
        {
           
            List<Account> fakeAccounts = new List<Account> {
                new Account {AccountId=500000, AccountTypeCode=200, CurrentBalance=1000, CustomerId=123456 },
                new Account {AccountId=200000,AccountTypeCode=200, CurrentBalance=1000, CustomerId=123456 },
                new Account {AccountId=300000, AccountTypeCode=200, CurrentBalance=1000, CustomerId=123456}
                };
            return fakeAccounts;
        }

        [Test]
        public async Task Transaction_Returns_Success_OnWithdraw()
        {
            TransactionRequest transactionRequest = new TransactionRequest()
            {
                AccountId = 500000,
                Amount = 5000,
                TransactionMode = "Withdraw"
            };
            TransactionStatus transactionStatus = new TransactionStatus()
            {
               TransactionStatusId = 1,
               CustomerId = 100000,
                AccountId = 500000,
                TStatusMessage = $"Withdraw of 5000 is Successfull",
               OpeningBalance = 1000,
                ClosingBalance = 6000
            };
            RuleStatus ruleStatus = new RuleStatus()
            {
                Status = "Allowed"
            };
            Account fakeAccount = GetAccounts().Find(x => x.AccountId == 500000);
            accountRepo.Setup(x => x.GetAccountDetailsById(500000)).ReturnsAsync(fakeAccount);
            accountRepo.Setup(x => x.Transaction(500000, 5000, "Withdraw")).ReturnsAsync(transactionStatus);
            //var mockClient = new Mock<HttpClient>();
            //mockClient.Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>())).ReturnsAsync(handlerMock.Object);
            ActionResult<TransactionStatus> transaction = await accountTransactionsController.Transaction(transactionRequest);
           
            OkObjectResult controllerResult = transaction.Result as OkObjectResult;
            Assert.AreEqual(200, controllerResult.StatusCode);
            Assert.IsNotNull(transaction, "No details for Transaction");
            Assert.AreEqual("Withdraw of 5000 is Successfull", (controllerResult.Value as TransactionStatus).TStatusMessage);
        }
        [Test]
        public async Task Transaction_Returns_Success_OnDeposit()
        {
            TransactionRequest transactionRequest = new TransactionRequest()
            {
                AccountId = 500000,
                Amount = 5000,
                TransactionMode = "Deposit"
            };
            TransactionStatus transactionStatus = new TransactionStatus()
            {
                TransactionStatusId = 1,
                CustomerId = 100000,
                AccountId = 500000,
                TStatusMessage = $"Deposit of 5000 is Successfull",
                OpeningBalance = 6000,
                ClosingBalance = 1000
            };
            Account fakeAccount = GetAccounts().Find(x => x.AccountId == 500000);
            accountRepo.Setup(x => x.GetAccountDetailsById(500000)).ReturnsAsync(fakeAccount);
            accountRepo.Setup(x => x.Transaction(500000, 5000, "Deposit")).ReturnsAsync(transactionStatus);
            ActionResult<TransactionStatus> transaction = await accountTransactionsController.Transaction(transactionRequest);
            OkObjectResult controllerResult = transaction.Result as OkObjectResult;
            Assert.AreEqual(200, controllerResult.StatusCode);
            Assert.IsNotNull(transaction, "No details for Transaction");
            Assert.AreEqual("Deposit of 5000 is Successfull", (controllerResult.Value as TransactionStatus).TStatusMessage);
        }
        [Test]
        public async Task Transaction_Returns_Failure_OnWithdraw()
        {
            TransactionRequest transactionRequest = new TransactionRequest()
            {
                AccountId = 500000,
                Amount = 5000,
                TransactionMode = "Withdraw"
            };
            TransactionStatus transactionStatus = new TransactionStatus()
            {
                TransactionStatusId = 0,
                CustomerId = 100000,
                AccountId = 500000,
                TStatusMessage = $"Withdraw of 5000 is Not Successfull",
                OpeningBalance = 1000,
                ClosingBalance = 1000
            };
            Account fakeAccount = GetAccounts().Find(x => x.AccountId == 500000);
            accountRepo.Setup(x => x.GetAccountDetailsById(500000)).ReturnsAsync(fakeAccount);
            accountRepo.Setup(x => x.Transaction(500000, 5000, "Withdraw")).ReturnsAsync(transactionStatus);
            ActionResult<TransactionStatus> transaction = await accountTransactionsController.Transaction(transactionRequest);
            OkObjectResult controllerResult = transaction.Result as OkObjectResult;
            Assert.AreEqual(200, controllerResult.StatusCode);
            Assert.IsNotNull(transaction, "No details for Transaction");
            Assert.AreEqual("Withdraw of 5000 is Not Successfull", (controllerResult.Value as TransactionStatus).TStatusMessage);
        }
        [Test]
        public async Task Transaction_Returns_Failure_OnDeposit()
        {
            TransactionRequest transactionRequest = new TransactionRequest()
            {
                AccountId = 500000,
                Amount = 5000,
                TransactionMode = "Deposit"
            };
            TransactionStatus transactionStatus = new TransactionStatus()
            {
                TransactionStatusId = 0,
                CustomerId = 100000,
                AccountId = 500000,
                TStatusMessage = $"Deposit of 5000 is Not Successfull",
                OpeningBalance = 1000,
                ClosingBalance = 1000
            };
            Account fakeAccount = GetAccounts().Find(x => x.AccountId == 500000);
            accountRepo.Setup(x => x.GetAccountDetailsById(500000)).ReturnsAsync(fakeAccount);
            accountRepo.Setup(x => x.Transaction(500000, 5000, "Deposit")).ReturnsAsync(transactionStatus);
            ActionResult<TransactionStatus> transaction = await accountTransactionsController.Transaction(transactionRequest);
            OkObjectResult controllerResult = transaction.Result as OkObjectResult;
            Assert.AreEqual(200, controllerResult.StatusCode);
            Assert.IsNotNull(transaction, "No details for Transaction");
            Assert.AreEqual("Deposit of 5000 is Not Successfull", (controllerResult.Value as TransactionStatus).TStatusMessage);
        }
    }
}