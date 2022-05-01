using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationRepository.Models
{
    public class Register
    {
        //[Required(ErrorMessage = "Email is required")]
        //[EmailAddress]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Password is required")]
        //[StringLength(16, ErrorMessage = "Password length must be between 6 and 16.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
        //[Required(ErrorMessage = "Password is required")]
        //[StringLength(16, ErrorMessage = "Password length must be between 6 and 16.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Compare("Password")]
        public string ReEnterPassWord { get; set; }
    }
   
}
