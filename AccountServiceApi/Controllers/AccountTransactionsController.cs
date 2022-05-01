using AccountServiceApi.Models;
using AccountServiceRepository;
using AccountServiceRepository.Models;
using AccountServiceRepository.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AccountServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountTransactionsController : ControllerBase
    {
        private readonly IAccountRepo accountRepo;
        HttpClient httpClient;
        public AccountTransactionsController(HttpClient client, IAccountRepo iaccountRepo)
        {
            httpClient = client;
            httpClient.BaseAddress = new Uri("http://localhost:9000/RulesService/Rules");
            accountRepo = iaccountRepo;
        }
        [HttpPost]
        public async Task<ActionResult<TransactionStatus>> Transaction(TransactionRequest transactionRequest)
        {

            try
            {
                TransactionStatus transactionStatus = new TransactionStatus();
                

                if (transactionRequest.TransactionMode == "Withdraw")
                {
                    Account account = await accountRepo.GetAccountDetailsById(transactionRequest.AccountId);
                    MinimumBalanceRequest minimumBalanceRequest = new MinimumBalanceRequest();
                    minimumBalanceRequest.AccountId = account.AccountId;
                    minimumBalanceRequest.Balance = account.CurrentBalance;
                    minimumBalanceRequest.AmmountRequest = transactionRequest.Amount;
                    HttpResponseMessage response = await httpClient.GetAsync(httpClient.BaseAddress + $"/EvaluateMinBal/AccountId-{minimumBalanceRequest.AccountId}/balance-{minimumBalanceRequest.Balance}/ammountRequest-{minimumBalanceRequest.AmmountRequest}").ConfigureAwait(false);
                    RuleStatus ruleStatus = new RuleStatus();
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        ruleStatus = JsonConvert.DeserializeObject<RuleStatus>(result);

                        if (ruleStatus.Status == "Allowed")
                        {
                            string action = transactionRequest.TransactionMode;
                            transactionStatus = await accountRepo.Transaction(transactionRequest.AccountId, transactionRequest.Amount,action);
                        }
                        else if (ruleStatus.Status == "Declined")
                        {
                            throw new AccountServiceException("Balance is not Sufficient to Perform Transaction");
                        }
                        return transactionStatus;
                    }
                    else
                    {
                        throw new AccountServiceException("Unable to Check for Minimum Balance Requirements Try later");
                    }
                }
                else if (transactionRequest.TransactionMode == "Deposit")
                {
                    string action = transactionRequest.TransactionMode;
                    transactionStatus = await accountRepo.Transaction(transactionRequest.AccountId, transactionRequest.Amount,action);
                }
                return Ok(transactionStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
