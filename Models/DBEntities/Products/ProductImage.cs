using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Products
{
    public class ProductImage : BaseModel
    {
        public int ProductId { get; set; }
        
        public Product Product { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
        
        public bool IsPrimary { get; set; }

        public int DisplayOrder { get; set; } = 0;
        
        
    }
}