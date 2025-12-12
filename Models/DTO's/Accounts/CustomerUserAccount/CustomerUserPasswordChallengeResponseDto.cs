namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount

{

    public class CustomerUserPasswordChallengeResponseDto
    {
        public string PendingAuthId { get; set; }

        public string OtpChannel { get; set; }

        public string MaskedDestination { get; set; }
    }

}