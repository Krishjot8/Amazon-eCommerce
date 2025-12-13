namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Password
{
public class SellerUserPasswordChallengeResponseDto
{
    public string PendingAuthId { get; set; }
    
    public string OtpChannel { get; set; }
    
    public string MaskedDestination { get; set; }
}

}