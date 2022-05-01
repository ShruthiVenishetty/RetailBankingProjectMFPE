using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetailBankingMVC.Models.CustomerModels
{
    public class Customer
    {
        [Required(ErrorMessage = "Customer Id is required")]
        [RegularExpression("^[0-9]{6}$", ErrorMessage = "Customer Id Must be 6 digit")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Customer Name is required")]
        [StringLength(30)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [Required(ErrorMessage = "date of birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [AgeValidator]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "PAN Number is required")]
        [RegularExpression("^([A-Za-z]){5}([0-9]){4}([A-Za-z]){1}$", ErrorMessage = "Invalid Pan Card Number")]
        public string PAN_Number { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }
    }
    public class AgeValidator : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "Age should be above 18 years";
        }

        protected override ValidationResult IsValid(object objValue,
                                                       ValidationContext validationContext)
        {
            DateTime dateValue = objValue as DateTime? ?? new DateTime();

            //alter this as needed. I am doing the date comparison if the value is not null

            if (dateValue.Year > DateTime.Today.AddYears(-18).Year)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
