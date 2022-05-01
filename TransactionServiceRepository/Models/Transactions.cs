using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionServiceRepository.Models
{
    public class Transactions
    {
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public int CounterPartyId { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionType { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public string TransactionMode { get; set; }
        public decimal AmountOfTransaction { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal CurrentBalance { get; set; }
    }
}
