using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionServiceRepository.Models
{
    public class TransactionRequest
    {
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int CounterPartyId { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionType { get; set; }
        public string TransactionMode { get; set; }
    }
}
