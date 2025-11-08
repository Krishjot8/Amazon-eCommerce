namespace Amazon_eCommerce_API.Models.DTO_s.BusinessAccount

{ 

public class BusinessUserPasswordChallengeResponseDto
{
    public string PendingAuthId { get; set; }
    
    public string OtpChannel { get; set; }
    
    public string MaskedDestination { get; set; }
}

}