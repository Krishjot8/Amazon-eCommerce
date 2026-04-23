using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Products;

namespace Amazon_eCommerce_API.Models.DBEntities.Carts.Customer
{                                   // items in each cart
    public class CustomerCartItem : BaseModel
    {
    
        public int CartId { get; set; }
        
        public CustomerCart  Cart { get; set; } = null!;

        public int ProductId { get; set; }
        
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
        
        public decimal Price { get; set; } //price at the time of adding to cart, for price changes in future

    }
}