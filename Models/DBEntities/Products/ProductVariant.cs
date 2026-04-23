using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Products
{
    public class ProductVariant : BaseModel
    {
        public int ProductId { get; set; }
        
        public Product Product { get; set; } = null!;
        
        public string SKU { get; set; } = null!;

        public decimal Price { get; set; }
        
        public int StockQuantity { get; set; }
        
        public string Color { get; set; } = null!;

        public string Size { get; set; } = null!;

        public bool IsDefault { get; set; } = false;
        
    }
}