using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountServiceRepository.Models
{
    public class MinimumBalanceRequest
    {
        public int AccountId { get; set; }
        public decimal Balance { get; set; }
        public decimal AmmountRequest { get; set; }
    }
}
