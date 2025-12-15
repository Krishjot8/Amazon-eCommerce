using Amazon_eCommerce_API.Models.DBEntities.Products;

namespace Amazon_eCommerce_API.Models.DBEntities.Carts.Customer
{                                   // items in each cart
    public class CustomerCartItem
    {
        public int CartItemId { get; set; }
        
        public int CartId { get; set; }
        
        public CustomerCart  Cart { get; set; }
        
        public int ProductId { get; set; }
        
        public Products.Products Products { get; set; }
        
        public int Quantity { get; set; }
        
        public decimal Price { get; set; }
        
    }
}