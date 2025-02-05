namespace Amazon_eCommerce_API.Models.DTO_s.Cache
{
    public class OtpCacheDto
    {


        public string Email { get; set; }

        public string Otp { get; set; }

        public DateTime ExpirationTime { get; set; }


        public int Attempts { get; set; }




    }
}
