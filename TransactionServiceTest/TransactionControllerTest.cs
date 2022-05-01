using NUnit.Framework;
using TransactionServiceRepository.Repository;
using Moq;
using System.Collections.Generic;
using TransactionServiceRepository.Models;
using System;
using System.Threading.Tasks;
using TransactionServiceApi.Controllers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Moq.Protected;
using System.Threading;
using System.Transactions;

namespace TransactionServiceTest
{

    public class TransactionControllerTest
    {
        private Mock<ITransactionRepo> transactionRepo;
        HttpClient httpClient;
        TransactionsController transactionsController;
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
            transactionRepo = new Mock<ITransactionRepo>();
            transactionsController = new TransactionsController(httpClient, transactionRepo.Object);
        }
        private static List<Transactions> GetTransactions()
        {
            List<Transactions> fakeTransactions = new List<Transactions> {
                new Transactions { TransactionId = 100000, CustomerId = 123456, AccountId = 500000, TransactionType = "Self",  AmountOfTransaction = 5000,TransactionMode="Deposit" ,PreviousBalance = 1000, CurrentBalance = 60000 ,DateOfTransaction=DateTime.Now,CounterPartyId=0,PaymentMethod="Cash"},
                new Transactions { TransactionId = 100001, CustomerId = 123456, AccountId = 500000, TransactionType = "Payment",  AmountOfTransaction = 5000,TransactionMode="Withdraw", PreviousBalance = 6000, CurrentBalance = 1000,DateOfTransaction=DateTime.Now ,CounterPartyId=0,PaymentMethod="Cash"},
                new Transactions { TransactionId = 100002, CustomerId = 123456, AccountId = 500000, TransactionType = "Adjustment",  AmountOfTransaction = 5000,TransactionMode="Transfer", PreviousBalance = 6000, CurrentBalance = 1000,DateOfTransaction=DateTime.Now ,CounterPartyId=0, PaymentMethod = "Cash"}
            };
            return fakeTransactions;
        }



        [Test]
        public async Task GetAllTransactions_Returns_AllTransactions()
        {



            List<Transactions> fakeTransactions = GetTransactions();
            StatementRequest statementRequest = new StatementRequest()
            {
                accountId = 500000,
                from_Date = DateTime.Now.AddDays(-30),
                to_Date = DateTime.Now

            };

            transactionRepo.Setup(x => x.GetAllTransactions(statementRequest)).ReturnsAsync(fakeTransactions);
            ActionResult<List<Transactions>> transaction = await transactionsController.GetFinancialTransaction(500000, DateTime.Now.AddDays(-30), DateTime.Now);
            OkObjectResult controllerResult = transaction.Result as OkObjectResult;
            Assert.AreEqual(200, controllerResult.StatusCode);
            Assert.IsNotNull(transaction, "Transaction List is null");
            Assert.AreEqual(3, (controllerResult.Value as List<Transaction>).Count, "Got wrong number of Transactions");



            Assert.Pass();
        }
        [Test]
        public async Task GetAllTransactions_Returns_NoTransactions()
        {



            List<Transactions> emptyTransactions = new List<Transactions>();
            StatementRequest statementRequest = new StatementRequest()
            {
                accountId = 500000,
                from_Date = DateTime.Now.AddDays(-30),
                to_Date = DateTime.Now

            };
            transactionRepo.Setup(x => x.GetAllTransactions(statementRequest)).ReturnsAsync(emptyTransactions);
            ActionResult<List<Transactions>> transaction = await transactionsController.GetFinancialTransaction(500000, DateTime.Now.AddDays(-30), DateTime.Now);
            NoContentResult controllerResult = transaction.Result as NoContentResult;
            Assert.AreEqual(204, controllerResult.StatusCode);
            Assert.Pass();
        }
        [Test]
        public async Task PerformTransactions_Returns_Success()
        {

            TransactionServiceRepository.Models.TransactionStatus transactionStatus = new TransactionServiceRepository.Models.TransactionStatus()
            { 
                TransactionStatusId=1,
                TStatusMessage= "Withdraw of 5000 is Successfull",
                AccountId=500000,
                ClosingBalance=1000,
                CustomerId=123456,
                OpeningBalance=6000
            };


            List<Transactions> emptyTransactions = new List<Transactions>();
            TransactionRequest transactionRequest = new TransactionRequest()
            {
               CustomerId=123456,
               AccountId=50000,
               Amount=5000,
               CounterPartyId=0,
               PaymentMethod="Cash",
               TransactionType="Self",
               TransactionMode="Withdraw"

    };
            transactionRepo.Setup(x => x.PerformTransactions(transactionStatus, transactionRequest)).Returns((Task<Transaction>)null);
            ActionResult<TransactionServiceRepository.Models.TransactionStatus> transaction = await transactionsController.PerformTransaction(transactionRequest);
            OkObjectResult controllerResult = transaction.Result as OkObjectResult;
            Assert.AreEqual(200, controllerResult.StatusCode);
            Assert.Pass();
        }

    }
}
    
