namespace Amazon_eCommerce_API.Models.DTO_s
{
    public class UserVerifyEmailDto    //Verify Email After registration
    {

        public string Email { get; set; }

        public string OTP { get; set; }

        public bool IsResendRequest { get; set; }


    }
}
