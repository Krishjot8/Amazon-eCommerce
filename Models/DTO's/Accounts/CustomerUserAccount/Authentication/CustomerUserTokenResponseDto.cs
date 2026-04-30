namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication
{
    public class CustomerUserTokenResponseDto //User Token Details
    {

        public int UserId {  get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Token { get; set; }
        
        public string FullName  => $"{FirstName} {LastName}";



    }
}
