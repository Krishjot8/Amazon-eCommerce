using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Products
{
    public class ProductAttributeValue : BaseModel
    {
        public string Value { get; set; }
        
        public int ProductId { get; set; }
        
        public Product Product { get; set; } = null!;
        
        public int ProductAttributeId { get; set; }
        
        public ProductAttribute ProductAttribute { get; set; } = null!;
    }
}