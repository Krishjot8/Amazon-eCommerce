namespace Amazon_eCommerce_API.Models.DBEntities.Products
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        
        public int ProductId { get; set; }
        
        public Product Product { get; set; }
        
        public string ImageUrl { get; set; }
        
        public bool isPrimary { get; set; }
        
        public int DisplayOrder { get; set; }
        
        
    }
}