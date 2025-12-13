namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Verification
{
    public class CustomerUserVerifyEmailDto    //Verify Email After registration
    {

        public string Email { get; set; }

        public string EmailOtp { get; set; }

        public bool IsResendRequest { get; set; }


    }
}
