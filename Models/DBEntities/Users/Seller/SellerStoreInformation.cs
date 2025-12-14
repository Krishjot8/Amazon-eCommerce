using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerStoreInformation
    {
        public string SellerUserId { get; set; }
        
        public string StoreName { get; set; }
        
        public bool HasUPCsForAllProducts { get; set; }
        
        public BrandOwnerShipStatus BrandOwnerShip { get; set; }
        
        public TrademarkStatus TrademarkStatus { get; set; }
        
        public SellerUser SellerUser { get; set; }
    }
}