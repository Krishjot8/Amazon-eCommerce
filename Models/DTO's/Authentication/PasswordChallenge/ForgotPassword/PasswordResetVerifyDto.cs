using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge.ForgotPassword
{
    public class PasswordResetVerifyDto
    {
        [Required(ErrorMessage = "PendingAuthId or Identifier is required.")]
        public string PendingAuthId { get; set; }
        
        [Required(ErrorMessage = "OTP Code is required.")]
        public string Otp { get; set; }
        
        [Required(ErrorMessage = "Account Type is required.")]
        public AccountType AccountType { get; set; }
    }
}