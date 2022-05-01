using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionServiceRepository.Models
{
    public class AccountRequest
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionMode { get; set; }
    }
}
