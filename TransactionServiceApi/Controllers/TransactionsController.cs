using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TransactionServiceRepository;
using TransactionServiceRepository.Models;
using TransactionServiceRepository.Repository;

namespace TransactionServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {

        private readonly ITransactionRepo transactionRepo;
        HttpClient httpClient;
        public TransactionsController(HttpClient client,ITransactionRepo iTransactionRepo)
        {
            httpClient = client;
            transactionRepo = iTransactionRepo;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<TransactionStatus>> PerformTransaction(TransactionRequest transactionRequest)
        {
            try
            {
                TransactionStatus transactionStatus = new TransactionStatus();
                string baseurl = "http://localhost:9000/AccountService/AcccountTransactions";
                if(transactionRequest.TransactionMode=="Withdraw" || transactionRequest.TransactionMode == "Deposit")
                {
                    AccountRequest accountRequest = new AccountRequest() { AccountId = transactionRequest.AccountId, Amount = transactionRequest.Amount,TransactionMode=transactionRequest.TransactionMode };
                    HttpResponseMessage httpResponseMessage;
                    httpResponseMessage = await httpClient.PostAsJsonAsync<AccountRequest>(baseurl, accountRequest);
                    if(httpResponseMessage.IsSuccessStatusCode)
                    {
                        string JsonContent = await httpResponseMessage.Content.ReadAsStringAsync();
                        transactionStatus = JsonConvert.DeserializeObject<TransactionStatus>(JsonContent);
                        await transactionRepo.PerformTransactions(transactionStatus, transactionRequest);
                    }
                    else
                    {
                        throw new TransactionServiceException(httpResponseMessage.Content.ReadAsStringAsync().Result);
                    }
                }
                else if(transactionRequest.TransactionMode == "Transfer")
                {
                    HttpResponseMessage httpResponseMessageAccount = await httpClient.GetAsync("http://localhost:9000/AccountService/Acccounts/"+transactionRequest.CounterPartyId);
                    if (httpResponseMessageAccount.IsSuccessStatusCode)
                    {
                        AccountRequest accountRequest = new AccountRequest() { AccountId = transactionRequest.AccountId, Amount = transactionRequest.Amount, TransactionMode = transactionRequest.TransactionMode };
                        accountRequest.TransactionMode = "Withdraw";
                        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync<AccountRequest>(baseurl, accountRequest);
                        if (httpResponseMessage.IsSuccessStatusCode)
                        {

                            string JsonContent = await httpResponseMessage.Content.ReadAsStringAsync();
                            transactionStatus = JsonConvert.DeserializeObject<TransactionStatus>(JsonContent);
                            await transactionRepo.PerformTransactions(transactionStatus, transactionRequest);

                            transactionRequest.AccountId = transactionRequest.CounterPartyId;
                            transactionRequest.CounterPartyId = transactionStatus.AccountId;
                            accountRequest.AccountId = transactionRequest.AccountId;
                            accountRequest.TransactionMode = "Deposit";
                            HttpResponseMessage httpResponseMessagedeposit = await httpClient.PostAsJsonAsync<AccountRequest>(baseurl, accountRequest);
                            if (httpResponseMessage.IsSuccessStatusCode)
                            {
                                TransactionStatus transactionStatus1 = new TransactionStatus();
                                string jsonContent = await httpResponseMessagedeposit.Content.ReadAsStringAsync();
                                transactionStatus1 = JsonConvert.DeserializeObject<TransactionStatus>(jsonContent);
                                await transactionRepo.PerformTransactions(transactionStatus1, transactionRequest);
                                transactionStatus.TStatusMessage = $"Successfully Transfered {transactionRequest.Amount} to {transactionRequest.AccountId} from {transactionRequest.CounterPartyId}";
                            }
                        }
                    }
                    else
                    {
                        throw new TransactionServiceException("No Account with that Account Id Present to Transfer");
                    }
                }
                
                return Ok(transactionStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
       
        [HttpGet("GetFinancialTransaction/{accountId}/{startDate?}/{endDate?}")]
        [Authorize(Roles ="Customer,Employee")]
        public async Task<ActionResult<List<Transactions>>> GetFinancialTransaction(int accountId, DateTime startDate, DateTime endDate)
        {

            try
            {
                StatementRequest statementRequest = new StatementRequest();
                statementRequest.accountId = accountId;
                statementRequest.from_Date = startDate;
                statementRequest.to_Date = endDate ;
                List<Transactions> transactions = await transactionRepo.GetAllTransactions(statementRequest);
                if (transactions.Count != 0)
                {
                    return Ok(transactions);
                }
                else
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {
                    return new StatusCodeResult(500);
            }

        }


    }
}
