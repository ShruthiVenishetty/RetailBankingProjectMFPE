using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailBankingMVC.Models.CustomerModels
{
    public class CustomerCreationStatus
    {
        public int StatusId { get; set; }
        public int CustomerId { get; set; }
        public string StatusMessage { get; set; }
    }
}
