namespace Amazon_eCommerce_API.Models.DTO_s.CustomerAccount
{
    public class CustomerUserVerifyEmailDto    //Verify Email After registration
    {

        public string Email { get; set; }

        public string emailOtp { get; set; }

        public bool IsResendRequest { get; set; }


    }
}
