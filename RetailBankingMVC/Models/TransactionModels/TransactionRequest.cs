using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetailBankingMVC.Models.TransactionModels
{
    public class TransactionRequest
    {
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        [Required]
        [Range(0, 99999, ErrorMessage = "Should be less than 100000")]
        public decimal Amount { get; set; }
        [Range(100000, 999999, ErrorMessage = "Should be 6 digit number")]
        public int CounterPartyId { get; set; }
        public PaymentMethods PaymentMethod { get; set; }
        public TransactionTypes TransactionType { get; set; }
        public string TransactionMode { get; set; }
    }
    public enum PaymentMethods
    {
       Cash= 302, BankTransfer= 301, Amex=300, DinersClub= 303, MasterCard= 304, Visa= 305
    }
    public enum TransactionTypes
    {
        Adjustment = 40001, Payment = 4002, Refund = 4003, Self = 4004
    }
}
