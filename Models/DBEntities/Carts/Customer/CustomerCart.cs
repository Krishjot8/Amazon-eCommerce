using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;

namespace Amazon_eCommerce_API.Models.DBEntities.Carts.Customer
{
    public class CustomerCart
    {
        
        public int CartId { get; set; }
        
        public int CustomerUserId { get; set; }
        
        public CustomerUser CustomerUser { get; set; }
        
        public List <CustomerCartItem> Items{ get; set; }
        
        
        
    }
}