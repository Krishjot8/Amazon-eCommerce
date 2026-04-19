using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Products
{
    public class ProductBrand : BaseModel

    {

        public string Name { get; set; }


        public string LogoUrl { get; set; }
        
        public List<Product> Products { get; set; }

    }
}
