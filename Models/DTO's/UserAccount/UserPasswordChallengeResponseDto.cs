namespace Amazon_eCommerce_API.Models.DTO_s.UserAccount;

public class UserPasswordChallengeResponseDto
{
    public string PendingAuthId { get; set; }
    public string OtpChannel { get; set; }
    public string MaskedDestination { get; set; }
}