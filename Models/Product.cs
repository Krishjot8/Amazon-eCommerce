namespace Amazon_eCommerce_API.Models
{
    public class Product : BaseModel

    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string PictureUrl { get; set; }

        public ProductType ProductType { get; set; }

        public int ProductTypeId { get; set; }

        public ProductBrand productBrand { get; set; }

        public int ProductBrandId { get; set;}

    }
}
