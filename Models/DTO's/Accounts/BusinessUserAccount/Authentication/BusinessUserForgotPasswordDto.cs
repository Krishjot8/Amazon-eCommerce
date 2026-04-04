using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Authentication
{
    public class BusinessUserForgotPasswordDto
    {


        [Required]
        [EmailAddress]
       public string BusinessEmail { get; set; }


        [Phone]
        public string? BusinessPhoneNumber { get; set; }




        [Required]
        public string? Otp { get; set; }


        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }


        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords must match")]
        public string ReEnterPassword { get; set; }

    }
}
