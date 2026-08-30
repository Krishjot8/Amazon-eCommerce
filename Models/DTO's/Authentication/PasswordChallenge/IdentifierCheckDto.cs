namespace Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge
{
    public class PasswordResetIdentifierDto
    {
        
        public string Identifier { get; set; }
        
        public AccountType AccountType { get; set; }
    }
}