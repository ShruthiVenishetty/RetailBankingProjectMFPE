using AccountServiceRepository.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AccountServiceRepository.Repository
{
    public class AccountRepo : IAccountRepo
    {
        AccountDbContext dc = new AccountDbContext();
       
        public async Task<TransactionStatus> Transaction(int aid, decimal amount,string action)
        {
            try
            {
                Account account = await GetAccountDetailsById(aid);
                var previousAmount = account.CurrentBalance;
                if(action=="Withdraw")
                {
                    account.CurrentBalance = account.CurrentBalance - amount;
                }
                else if(action=="Deposit")
                {
                    account.CurrentBalance = account.CurrentBalance + amount;
                }
                
                await dc.SaveChangesAsync();
                TransactionStatus transactionStatus = new TransactionStatus();
                if (account.CurrentBalance != previousAmount)
                {
                    transactionStatus.TransactionStatusId = 1;
                    transactionStatus.CustomerId = account.CustomerId;
                    transactionStatus.AccountId = account.AccountId;
                    transactionStatus.TStatusMessage = $"{action} of {amount} is Successfull";
                    transactionStatus.OpeningBalance = previousAmount;
                    transactionStatus.ClosingBalance = account.CurrentBalance;
                }
                else
                {
                    transactionStatus.TransactionStatusId = 0;
                    transactionStatus.CustomerId = account.CustomerId;
                    transactionStatus.AccountId = account.AccountId;
                    transactionStatus.TStatusMessage = $"{action} of {amount} is Not Successfull";
                    transactionStatus.OpeningBalance = previousAmount;
                    transactionStatus.ClosingBalance = account.CurrentBalance;
                }
                return transactionStatus;
            }
            catch (Exception)
            {
                throw new AccountServiceException($"Error while {action} into account");
            }
        }

        public async Task<List<Account>> GetAccountDetailsBycid(int coustomeId)
        {
            try
            {
                List<Account> accounts = await (from a in dc.Accounts where a.CustomerId == coustomeId select a).ToListAsync();
                return accounts;
            }
            catch (Exception)
            {
                throw new AccountServiceException("No Account with such id Found");
            }

        }

        public async Task<Account> GetAccountDetailsById(int accountId)
        {
            try
            {
                Account account = await dc.Accounts.Where(x => x.AccountId == accountId).FirstAsync();
                return account;
            }
            catch (Exception)
            {
                throw new AccountServiceException("No Account with such id Found");
            }
        }

      
        public async Task<AccountCreationStatus> InsertAccount(int customerId)
        {
            try
            {
                List<int> accountTypes = await (from types in dc.AccountType select types.AccountTypeCode).ToListAsync();
                foreach (int accountTypeId in accountTypes)
                {
                    Account newAccount = new Account();
                    newAccount.CustomerId = customerId;
                    newAccount.AccountTypeCode = accountTypeId;
                    newAccount.CurrentBalance = 500;
                    await dc.Accounts.AddAsync(newAccount);
                    await dc.SaveChangesAsync();
                }
                //write here
                List<Account> accounts = await GetAccountDetailsBycid(customerId);
                AccountCreationStatus accountCreationStatus = new AccountCreationStatus();
                if (accounts != null)
                {
                    accountCreationStatus.StatusId = 1;
                    accountCreationStatus.StatusMessage = $"Successfully Created {accounts.Count} for {customerId}";
                }
                else
                {
                    accountCreationStatus.StatusId = 0;
                    accountCreationStatus.StatusMessage = $"Not Created";
                }
                return accountCreationStatus;

            }
            catch (Exception)
            {
                throw new AccountServiceException("Error While Creating and Updating an Account");
            }
        }

      
    }
}
