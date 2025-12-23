namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerBusinessInformation
    {
        
        public int SellerUserId { get; set; }
        
        public string BusinessName { get; set; }
        
        public string BusinessPhoneNumber { get; set; }
        
        public string CompanyRegistrationNumber { get; set; }
        
        public string Country {get; set;}
        
        
        public string AddressLine1 {get; set;}
        
        public string? AddressLine2 {get; set;}
        
        public string City {get; set;}
        
        public string State {get; set;}
        
        public string ZipCode {get; set;}
        
        public SellerUser SellerUser { get; set; }
    }
}