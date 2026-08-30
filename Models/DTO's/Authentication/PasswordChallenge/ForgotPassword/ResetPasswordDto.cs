using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge.ForgotPassword
{
    public class ResetForgotPasswordDto
    {
        [Required(ErrorMessage = "Identifier is required")]
        public string Identifier {get; set;} = String.Empty;
        
        
        [Required(ErrorMessage = "Reset token is required")]
        public string ResetToken { get; set; } = String.Empty;
        
        [Required(ErrorMessage = "New password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
            ErrorMessage = "Passwords must be at least 6 characters long, and contain at least one uppercase letter, one number, and one special character.")]
        public string NewPassword { get; set; } = String.Empty;


        [Required(ErrorMessage = "Please confirm your new password.")]
        [DataType(DataType.Password)]
        
        [Compare("NewPassword", ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; } = String.Empty;
        
        
        public AccountType AccountType { get; set; }

    }
}