using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailBankingMVC.Models.AccountModels
{
    public class Account
    {
        public int AccountId { get; set; }
        public int AccountTypeCode { get; set; }
        public decimal CurrentBalance { get; set; }
        public int CustomerId { get; set; }
    }
}
