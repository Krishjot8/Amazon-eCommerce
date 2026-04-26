using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerPayoutAccount : BaseModel
    {
        public int SellerUserId { get; set; }
        
        public string BankName { get; set; } = null!;
        
        public string AccountHolderName { get; set; } = null!;
        
        public string AccountNumber { get; set; } = null!;
        
        public string Last4Digits { get; set; } = null!;
        
        public string RoutingNumber { get; set; } = null!;
        
       
        
        public string Country { get; set; } = null!;
        
        public bool IsDefault { get; set; }

        public SellerUser SellerUser { get; set; } = null!;
    }
}