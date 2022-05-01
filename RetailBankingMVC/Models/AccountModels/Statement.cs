using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailBankingMVC.Models.AccountModels
{
    public class Statement
    {
        public int StatementId { get; set; }
        public DateTime ValueDate { get; set; }
        public decimal Withdrawl { get; set; }
        public decimal Deposit { get; set; }
        public decimal ClosingBalance { get; set; }
        public int AccountId { get; set; }
    }
}
