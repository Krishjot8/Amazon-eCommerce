using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Products
{
    public class ProductVariant : BaseModel
    {
        public int ProductId { get; set; }
        
        public Product Product { get; set; }
        
        public string SKU { get; set; }
        
        public decimal Price { get; set; }
        
        public int StockQuantity { get; set; }
        
        public string Color { get; set; }
        
        public string Size { get; set; }
        
        public bool IsDefault { get; set; } = false;
        
    }
}