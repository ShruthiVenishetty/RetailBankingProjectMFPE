using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionServiceRepository.Models
{
    public class StatementRequest
    {
        public int accountId { get; set; }
        public DateTime from_Date { get; set; }
        public DateTime to_Date { get; set; }
    }
}
