using Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge;

namespace Amazon_eCommerce_API.Models.DTO_s.Cache
{
    public class OtpCacheDto
    {


        public string Identifier { get; set; }

        public string Otp { get; set; }

        public DateTime ExpirationTime { get; set; }


        public int Attempts { get; set; }
        
        public DateTime LastRequestTime { get; set; }

        public OtpChannel OtpChannel { get; set; }
        public string MaskedDestination { get; set; }


    }
}
