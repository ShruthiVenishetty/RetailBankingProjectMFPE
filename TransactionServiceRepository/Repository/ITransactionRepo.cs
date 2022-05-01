using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TransactionServiceRepository.Models;

namespace TransactionServiceRepository.Repository
{
   public interface ITransactionRepo
    {
       
        Task<List<Transactions>> GetAllTransactions(StatementRequest statementRequest);
        Task PerformTransactions(TransactionStatus transactionStatus,TransactionRequest transactionRequest);
    }
}
