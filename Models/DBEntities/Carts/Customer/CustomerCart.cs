using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;

namespace Amazon_eCommerce_API.Models.DBEntities.Carts.Customer
{
    public class CustomerCart : BaseModel
    {
        
        
        public int CustomerUserId { get; set; }
        
        public CustomerUser CustomerUser { get; set; } = null!;

        public List <CustomerCartItem> Items{ get; set; } = new();
        
        
        public CartStatus CartStatus { get; set; }
        
        
        
    }

    public enum CartStatus
    {
        Active,
        Saved,
        Abandoned,
        CheckedOut
    }
}