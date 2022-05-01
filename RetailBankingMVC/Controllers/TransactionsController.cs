using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RetailBankingMVC.Models.AccountModels;
using RetailBankingMVC.Models.TransactionModels;
using RetailBankingMVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailBankingMVC.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService;
        string token;
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        public IActionResult Index()
        {
            return View();
        }
       
        public  IActionResult GetTransactions(int accountId)
        {
            StatementRequest statementRequest = new StatementRequest();
            statementRequest.accountId = accountId;
            return View(statementRequest);
        }
        [HttpPost]
        public async Task<IActionResult> GetTransactions(StatementRequest statementRequest)
        {
            token = HttpContext.Session.GetString("token");
            if (HttpContext.Session.GetString("IsEmployee") != "True" && HttpContext.Session.GetString("IsCustomer") != "True")
            {
                return RedirectToAction("Login", "Authentication");
            }
            else
            {
                List<Transactions> transactions = new List<Transactions>();
                HttpResponseMessage response = await _transactionService.GetAllTransactions(statementRequest, token);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JsonContent = await response.Content.ReadAsStringAsync();
                    transactions = JsonConvert.DeserializeObject<List<Transactions>>(JsonContent);
                    //return RedirectToAction("Transactions","Accounts", new { statementsOfAccounts = statements });
                    return View("GetAllTransactions",transactions);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    ViewBag.Message = "No Reccord Found";
                    return View("GetAllTransactions", transactions);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    ViewBag.Message = "Having server issue while adding record";
                    return View("GetAllTransactions", transactions);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    ViewBag.Message = "Internal Server Error! Please try again later";
                    return View("GetAllTransactions", transactions);
                }

                return View();
            }
        }
            [HttpGet]
        public IActionResult PerformTransactions(int accountId, string transactionMode)
        {

            if (HttpContext.Session.GetString("IsEmployee") != "true" && HttpContext.Session.GetString("IsCustomer") != "True")
            {
                return RedirectToAction("Login", "Authentication");
            }
            else
            {
                TransactionRequest transactionRequest = new TransactionRequest() { AccountId = accountId, CustomerId = (int)HttpContext.Session.GetInt32("userId") ,TransactionMode= transactionMode };

                return View(transactionRequest);
            }
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PerformTransactions(TransactionRequest transactionRequest)
        {
            token = HttpContext.Session.GetString("token");
            TransactionStatus transactionStatus = new TransactionStatus();

            var response = await _transactionService.Transactions(transactionRequest, token);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsoncontent = await response.Content.ReadAsStringAsync();
                transactionStatus = JsonConvert.DeserializeObject<TransactionStatus>(jsoncontent);
                return View("TransactionStatus", transactionStatus);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                ViewBag.Message = response.Content.ReadAsStringAsync().Result;
                return View(transactionRequest);
            }
          
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                ViewBag.Message = "Internal Server Error! Please try again later";
                return View(transactionRequest);
            }

            ModelState.AddModelError("", "Having some unexpected error while processing transaction");
            return View(transactionRequest);
        }
    } 

}
