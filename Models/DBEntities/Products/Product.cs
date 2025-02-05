
using Amazon_eCommerce_API.Models;


    public class Product : BaseModel

    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
        public string PictureUrl { get; set; }


        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }


        public ProductBrand ProductBrand { get; set; }
        public int ProductBrandId { get; set;}


        public Category Category { get; set; }
        public int CategoryId { get; set; }

       

    }

