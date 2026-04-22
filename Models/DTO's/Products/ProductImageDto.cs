namespace Amazon_eCommerce_API.Models.DTO_s.Products
{
    public class ProductImageDto
    {
        
        public int Id { get; set; }
      
        public string ImageUrl { get; set; }
        
        public bool IsPrimary { get; set; }
        
        public int DisplayOrder { get; set; }
        
    }
}