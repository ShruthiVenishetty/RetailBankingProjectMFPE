using AccountServiceRepository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AccountServiceRepository.Repository
{
    public interface IAccountRepo
    {
        Task<Account> GetAccountDetailsById(int accountId);
        Task<List<Account>> GetAccountDetailsBycid(int customerId);
        Task<AccountCreationStatus> InsertAccount(int customerId);
       
        Task<TransactionStatus> Transaction(int accountId, decimal amount,string action);
      
    }
}
