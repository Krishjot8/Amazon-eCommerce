namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Authentication
{
    public class BusinessUserTokenResponseDto //User Token Details
    {

        public int UserId {  get; set; }

        public string BusinessName { get; set; } = String.Empty;
        

        public string Token { get; set; } 



    }
}
