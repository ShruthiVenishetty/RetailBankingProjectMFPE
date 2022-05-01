using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerServiceRepository.Models
{
    [Table("Customer")]
    public partial class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
       
        public int CustomerId { get; set; }
       
        public string Name { get; set; }
       
        public string Address { get; set; }
    
      
        public DateTime DOB { get; set; }
      
      
        public string PAN_Number { get; set; }
       
        public string Email { get; set; }
       
        public DateTime CreatedAt { get; set; } 
    }
   
}
