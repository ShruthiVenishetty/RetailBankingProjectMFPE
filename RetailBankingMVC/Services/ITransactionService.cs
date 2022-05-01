using RetailBankingMVC.Models.AccountModels;
using RetailBankingMVC.Models.TransactionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RetailBankingMVC.Services
{
    public interface ITransactionService
    {
        Task<HttpResponseMessage> GetAllTransactions(StatementRequest statementRequest, string tokenn);

        Task<HttpResponseMessage> Transactions(TransactionRequest transactionRequest, string token);
    }
}
