namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerProfile
    {
        
        public int SellerUserId { get; set; }
        
        public string FirstName { get; set; }

        public string? MiddleName { get; set; } = null;
        
        public string LastName { get; set; }
        
        public DateOnly DateOfBirth { get; set; }
        
        public SellerUser SellerUser  { get; set; }
    }
}