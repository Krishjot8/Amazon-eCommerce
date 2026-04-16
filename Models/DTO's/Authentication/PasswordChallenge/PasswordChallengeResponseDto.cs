namespace Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge
{
    public class PasswordChallengeResponseDto
    {
        
        public string PendingAuthId { get; set; }
        
        public OtpChannel OtpChannel { get; set; }
        
        public string MaskedDestination { get; set; }
       
        public string Message { get; set; }
        
    }

    public enum OtpChannel
    {
        
        Email,
        SMS
        
    }
}