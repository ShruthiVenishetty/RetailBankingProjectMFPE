using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountServiceRepository.Models
{
    public class AccountCreationStatus
    {
        public int StatusId { get; set; }
        public string StatusMessage { get; set; }
    }
}
