using System.ComponentModel.DataAnnotations;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;

namespace Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge.ForgotPassword
{
    public class PasswordResetGenerateDto
    {
        
        [Required(ErrorMessage = "Identifier is required.")]
        public string Identifier { get; set; }
        
        public AccountType AccountType { get; set; }
    }
}