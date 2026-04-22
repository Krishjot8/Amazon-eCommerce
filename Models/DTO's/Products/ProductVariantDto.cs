namespace Amazon_eCommerce_API.Models.DTO_s.Products
{
    public class ProductVariantDto
    {
        public int Id {get; set;}
        
        public int ProductId {get; set;}
        
        public string Name {get; set;}
        
        public decimal Price {get; set;}
        
        public int StockQuantity {get; set;}
    }
}