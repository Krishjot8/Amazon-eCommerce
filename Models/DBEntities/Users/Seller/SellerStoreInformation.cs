using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerStoreInformation : BaseModel
    {
        public int SellerUserId { get; set; }
        
        public string StoreName { get; set; } = null!;

        public bool HasUPCsForAllProducts { get; set; }
        
        public bool HasDiversityCertifications { get; set; }
        
        public BrandOwnershipStatus BrandOwnership { get; set; }
        
        public TrademarkStatus TrademarkStatus { get; set; }
        
        public SellerUser SellerUser { get; set; } = null!;
    }
}