using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s
{
    public class UserForgotPasswordDto
    {


        [Required]
        [EmailAddress]
       public string Email { get; set; }


        [Phone]
        public string? PhoneNumber { get; set; }




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
