using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TransactionServiceRepository.Models;

namespace TransactionServiceRepository.Repository
{
    public class TransactionRepo : ITransactionRepo
    {
        TransactionDBContext dc = new TransactionDBContext();
        
        public async Task<int> GetPaymentMethodId(string paymentMethod)
        {
            try
            {            
                 int paymentMethodCode = await (from paymethod in dc.PaymentMethods where paymethod.PaymentMethodName.Equals(paymentMethod) select paymethod.PaymentMethodCode).FirstAsync();
                 return paymentMethodCode;
            }
            catch
            {
                throw new TransactionServiceException("No Such Payment Method Found");
            }
        }
        public async Task<string> GetPaymentMethodName(int paymentMethodId)
        {
            try
            {
                string paymentMethod = await (from paymethod in dc.PaymentMethods where paymethod.PaymentMethodCode== paymentMethodId select paymethod.PaymentMethodName).FirstAsync();
                return paymentMethod;
            }
            catch
            {
                throw new TransactionServiceException("No Such Payment Method Found");
            }
        }
        public async Task<int> GetTransactionTypeId(string transactionType)
        {
            try
            {
                int transacTiontypeId = await (from types in dc.TransactionTypes where types.TransactionTypeDescription == transactionType select types.TransactionTypeCode).FirstAsync();
                return transacTiontypeId;
            }
            catch
            {
                throw new TransactionServiceException("No Such Transaction Type Found");
            }
        }
        public async Task<string> GetTransactionTypeName(int transactionType)
        {
            try
            {
                string transacTiontypeCode = await (from types in dc.TransactionTypes where types.TransactionTypeCode == transactionType select types.TransactionTypeDescription).FirstAsync();
                return transacTiontypeCode;
            }
            catch
            {
                throw new TransactionServiceException("No Such Transaction Type Found");
            }
        }


        public async Task<List<Transactions>> GetAllTransactions(StatementRequest statementRequest)
        {
            try
            {
                if (statementRequest.to_Date == DateTime.MinValue)
                {
                    statementRequest.to_Date = DateTime.Now;
                    if (statementRequest.from_Date == DateTime.MinValue)
                    {
                        statementRequest.from_Date = DateTime.Now.AddDays(-30);
                    }

                }
                List<FinancialTransaction> financialTransactions = await (from trans in dc.financialTransactions where (trans.AccountId == statementRequest.accountId && ((trans.DateOfTransaction <= statementRequest.to_Date) && (trans.DateOfTransaction >= statementRequest.from_Date))) select trans).ToListAsync();
                List<Transactions> transactions = new List<Transactions>();

              foreach(FinancialTransaction financialTransaction in financialTransactions)
                {
                    Transactions transaction = new Transactions();
                    transaction.AccountId = financialTransaction.AccountId;
                    transaction.AmountOfTransaction = financialTransaction.AmountOfTransaction;
                    transaction.CounterPartyId = financialTransaction.CounterPartyId;
                    transaction.CurrentBalance = financialTransaction.CurrentBalance;
                    transaction.CustomerId = financialTransaction.CustomerId;
                    transaction.DateOfTransaction = financialTransaction.DateOfTransaction;
                    transaction.PaymentMethod = await GetPaymentMethodName(financialTransaction.PaymentMethodCode);
                    transaction.PreviousBalance = financialTransaction.PreviousBalance;
                    transaction.TransactionId = financialTransaction.TransactionId;
                    transaction.TransactionType = await GetTransactionTypeName(financialTransaction.TransactionTypeCode);
                    transaction.TransactionMode = financialTransaction.TransactionMode;
                    transactions.Add(transaction);
                }
                return transactions;         
            }
            catch
            {
                throw new TransactionServiceException("Error While Retreiving Transaction details");
            }
        }
      

        public async Task PerformTransactions(TransactionStatus transactionStatus, TransactionRequest transactionRequest)
        {
            FinancialTransaction financialTransaction = new FinancialTransaction();
            financialTransaction.AccountId = transactionRequest.AccountId;
            financialTransaction.CounterPartyId = transactionRequest.CounterPartyId;
            financialTransaction.CurrentBalance = transactionStatus.ClosingBalance;
            financialTransaction.PreviousBalance = transactionStatus.OpeningBalance;
            financialTransaction.PaymentMethodCode = await GetPaymentMethodId(transactionRequest.PaymentMethod);
            financialTransaction.TransactionTypeCode = await GetTransactionTypeId(transactionRequest.TransactionType);
            financialTransaction.TransactionMode = transactionRequest.TransactionMode;
            financialTransaction.CustomerId = transactionStatus.CustomerId;
            financialTransaction.DateOfTransaction = DateTime.Now;
            financialTransaction.AmountOfTransaction = transactionRequest.Amount;
            await dc.financialTransactions.AddAsync(financialTransaction);
            await dc.SaveChangesAsync();
        }
    }
}
