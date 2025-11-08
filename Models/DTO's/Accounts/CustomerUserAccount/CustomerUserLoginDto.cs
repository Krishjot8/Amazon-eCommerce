using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Amazon_eCommerce_API.Models.DTO_s.CustomerAccount
{
    public class CustomerUserLoginDto  //Angular User Login Entity  
    {

        [Required(ErrorMessage = "Enter Your Mobile Number or Email Address")]
        [CustomerLoginIdValidation]
        public string EmailOrPhone { get; set; }

        [Required(ErrorMessage = "Enter Your Password")]
       
        public string Password { get; set; }  


    }

    public class CustomerLoginIdValidationAttribute : ValidationAttribute
    {


        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
           
            var loginId = value?.ToString()?.Trim();

          

            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";


            var phoneRegex = @"^[\d\-]{10,20}$";



            if (Regex.IsMatch(loginId, emailRegex))
                return ValidationResult.Success;


            if(Regex.IsMatch(loginId,phoneRegex))
                return ValidationResult.Success;


            if (loginId.Contains("@"))
                return new ValidationResult("Invalid Email Address");


            return new ValidationResult("Invalid Mobile Phone Number");
        }








    }
}

