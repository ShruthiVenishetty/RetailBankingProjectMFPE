using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerServiceRepository.Models
{
  
    public class CustomerCreationStatus
    {
       public int StatusId { get; set; }
        public int CustomerId { get; set; }

        public string StatusMessage { get; set; }
      


    }
}
