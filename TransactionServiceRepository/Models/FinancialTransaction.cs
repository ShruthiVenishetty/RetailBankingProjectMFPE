using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TransactionServiceRepository.Models
{
    [Table("FinancialTransaction")]
    public class FinancialTransaction
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public int CounterPartyId { get; set; }
        [ForeignKey("PaymentMethod")]
        public int PaymentMethodCode { get; set; }
        [ForeignKey("TransactionType")]
        public int TransactionTypeCode { get; set; }
        public string TransactionMode { get; set; }
        public DateTime DateOfTransaction { get; set; }
        public decimal AmountOfTransaction { get; set; } 
        public decimal PreviousBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual TransactionType TransactionType { get; set; }
      


    }
}
