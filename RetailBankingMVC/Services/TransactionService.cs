using RetailBankingMVC.Models.AccountModels;
using RetailBankingMVC.Models.TransactionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RetailBankingMVC.Services
{
    public class TransactionService : ITransactionService
    {
		private string baseurl = "http://localhost:9000/TransactionService/Transactions";

        public async Task<HttpResponseMessage> Transactions(TransactionRequest transactionRequest, string token)
        {
			using (HttpClient client = new HttpClient())
			{
				
				client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);
                TransactionData transactionData = new TransactionData()
                {
                    AccountId = transactionRequest.AccountId,
                    CustomerId = transactionRequest.CustomerId,
                    Amount = transactionRequest.Amount,
                    PaymentMethod = transactionRequest.PaymentMethod.ToString(),
                    TransactionType = transactionRequest.TransactionType.ToString(),
                    CounterPartyId = transactionRequest.CounterPartyId,
                    TransactionMode = transactionRequest.TransactionMode
                };
              

                var response = await client.PostAsJsonAsync<TransactionData>(baseurl, transactionData);
				return response;
			}
		}

        public async Task<HttpResponseMessage> GetAllTransactions(StatementRequest statementRequest, string token)
        {
			using (HttpClient client = new HttpClient())
			{
				MediaTypeWithQualityHeaderValue ContentType = new MediaTypeWithQualityHeaderValue("application/json");
				if (statementRequest.FromDate != null || statementRequest.ToDate != null)
				{
					string startdate= statementRequest.FromDate.Value.ToString("yyyy-MM-dd");
					string enddate = statementRequest.ToDate.Value.ToString("yyyy-MM-dd");
					client.BaseAddress = new Uri(baseurl + $"/GetFinancialTransaction/{statementRequest.accountId}/{startdate}/{enddate}");
				}
				else
				{
					client.BaseAddress = new Uri(baseurl + $"/GetFinancialTransaction/{statementRequest.accountId}");
				}
				
				client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + token);

				client.DefaultRequestHeaders.Accept.Add(ContentType);
				HttpResponseMessage response = await client.GetAsync("");
				return response;
			}
		}
    }
}
