namespace Amazon_eCommerce_API.Models.DTO_s.Products
{
    public class ProductDto
    {
        public int Id {get; set;}
        
        public string Name {get; set;}
        
        public string Description {get; set;}
        
        public decimal Price {get; set;}
        
        public int StockQuantity {get; set;}
        
        public string Brand {get; set;}
        
        public string Type {get; set;}
        
        public string Category {get; set;}
        
        public string MainImageUrl {get; set;}

        public List<ProductImageDto> Images { get; set; } = new();

        public List<ProductVariantDto> Variants { get; set; } = new();
        
        
        public decimal Rating {get; set;}
        
        public int ReviewCount {get; set;}
    }
}