using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionServiceRepository.Models
{
    public class TransactionStatus
    {
        public int TransactionStatusId { get; set; }
        public int AccountId { get; set; }
        public int CustomerId { get; set; }
        public string TStatusMessage { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }

    }
}
