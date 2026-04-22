using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;

namespace Amazon_eCommerce_API.Models.DBEntities.Products;

    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public decimal Price { get; set; }  // temporary
        public int StockQuantity { get; set; }  // temporary
        
        public string ImageUrl { get; set; } // Main Image

        public int SellerUserId { get; set; }
        
        public SellerUser SellerUser { get; set; }

        
        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; }
     

        public int ProductBrandId { get; set; }

        public ProductBrand ProductBrand { get; set; }
     

        public int CategoryId { get; set; }

        public Category Category { get; set; } 
     
        public bool IsActive { get; set; } = true;

        public List<ProductImage> Images { get; set; } = new();

        public List<ProductVariant> Variants { get; set; } = new();
    }

