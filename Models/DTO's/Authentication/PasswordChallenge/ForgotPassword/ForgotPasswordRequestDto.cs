namespace Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge.ForgotPassword
{
    public class ForgotPasswordRequestDto
    {
        
        public string Identifier {get; set;}
        
        
        public AccountType AccountType {get; set;}
    }
}