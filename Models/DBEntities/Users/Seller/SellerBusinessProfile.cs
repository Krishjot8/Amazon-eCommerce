using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerBusinessProfile
    {
     
        public int SellerUserId { get; set; }
        
        public BusinessType BusinessType { get; set; } 
        
        public bool AgreedToTerms { get; set; }
        
        public DateTime TermsAcceptedAt { get; set; }
        
        public SellerUser SellerUser { get; set; }
        
    }
}