using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountServiceRepository.Models
{
    [Table("Account")]
    public class Account
    {
      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        [Required]
        [ForeignKey("AccountType")]
        public int AccountTypeCode { get; set; }
        public decimal CurrentBalance { get; set; }
        public int CustomerId { get; set; }
        public virtual AccountType AccountType { get; set; }
    }
}
