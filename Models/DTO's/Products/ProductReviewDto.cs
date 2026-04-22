namespace Amazon_eCommerce_API.Models.DTO_s.Products
{
    public class ProductReviewDto
    {
        
        public int Id { get; set; }
        
        public string CustomerName { get; set; }
        
        public int Rating { get; set; }
        
        public string Comment { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
    }
}