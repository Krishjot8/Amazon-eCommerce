namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Authentication
{
    public class SellerUserTokenResponseDto //User Token Details
    {

        public int UserId {  get; set; }

        public string FullName { get; set; }
        
        public string? StoreName { get; set; }
        
        public string Token { get; set; }



    }
}
