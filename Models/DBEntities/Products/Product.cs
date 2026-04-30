using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;

namespace Amazon_eCommerce_API.Models.DBEntities.Products;

    public class Product : BaseModel
    {
        public string Name { get; set; } = null!;
        
         public string Description { get; set; } = null!;

        public decimal Price { get; set; }  // temporary
        public int StockQuantity { get; set; }  // temporary
        
        public string ImageUrl { get; set; } = null!; // Main Image

        public int SellerUserId { get; set; }
        
        public SellerUser SellerUser { get; set; } = null!;


    public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = null!;


    public int ProductBrandId { get; set; }

        public ProductBrand ProductBrand { get; set; } = null!;


    public int CategoryId { get; set; }

        public Category Category { get; set; } = null!;

    public bool IsActive { get; set; } = true;

    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public ICollection<ProductReview> Reviews { get; set; } = new List<ProductReview>();
}

