namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Verification
{
    public class SellerUserVerifyEmailDto    //Verify Email After registration
    {

        public string Email { get; set; }

        public string EmailOtp { get; set; }

        public bool IsResendRequest { get; set; }


    }
}
