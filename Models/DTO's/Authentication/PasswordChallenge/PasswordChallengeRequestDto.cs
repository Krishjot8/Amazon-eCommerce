using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;

namespace Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge
{
    public class PasswordChallengeRequestDto
    {
        
        public string EmailOrPhone { get; set; }
        
        public string Password { get; set; }
        
        public UserRole Role { get; set; }
    }
}