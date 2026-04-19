using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Products
{
    public class Category : BaseModel
    {

        public string Name { get; set; } 
        
        public List<Product> Products { get; set; }

    }
}