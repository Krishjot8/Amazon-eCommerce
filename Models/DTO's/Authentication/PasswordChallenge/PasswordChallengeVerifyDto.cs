using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;

namespace Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge
{
    public class PasswordChallengeVerifyDto
    {
        public string PendingAuthId { get; set; }
        
        public string Otp { get; set; }
        
        public UserRole Role { get; set; }
    }
}