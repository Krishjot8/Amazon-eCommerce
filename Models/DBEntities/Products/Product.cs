using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Products;

namespace Amazon_eCommerce_API.Models.DBEntities.Products;

    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
        
        public string ImageUrl { get; set; }


        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }


        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set; }


        public Category Category { get; set; }
        public int CategoryId { get; set; }


        public List<ProductImage> Images { get; set; } = new();
    }

