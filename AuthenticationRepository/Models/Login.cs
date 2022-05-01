using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationRepository.Models
{
    [Table("Login")]
    public partial class Login
    {
       
            [Key]
            public string Email { get; set; }
            public int UserId { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        
    }
}
