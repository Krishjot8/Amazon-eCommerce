using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;

namespace Amazon_eCommerce_API.Models.DBEntities.Products
{
    public class ProductReview : BaseModel
    {
        
        public int ProductId { get; set; }
        
        public Product Product { get; set; }
        
        public int CustomerUserId { get; set; }
        
        public CustomerUser CustomerUser { get; set; }
        
        public int Rating { get; set; }
        
        public string? Comment { get; set; }
        
        public bool IsVerifiedPurchase { get; set; }
        
        
    }
}