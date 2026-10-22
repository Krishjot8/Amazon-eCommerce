namespace Amazon_eCommerce_API.Models.DTO_s.Products
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public int StockQuantity { get; set; }
        
        public int CategoryId { get; set; }
        
        public int ProductBrandId { get; set; }
        
        public int ProductTypeId { get; set; }
        
        
        public IFormFileCollection ImageFiles { get; set; } = null!;
    }
}