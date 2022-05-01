using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServiceApi.Models
{
    public class TransactionRequest
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionMode { get; set; }
    }
}
