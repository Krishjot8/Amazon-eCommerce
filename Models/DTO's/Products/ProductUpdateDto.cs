namespace Amazon_eCommerce_API.Models.DTO_s.Products
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        
        public string Description { get; set; } = null!;
        
        public decimal Price { get; set; }
        
        public int StockQuantity { get; set; }
        
        
        public int ProductTypeId { get; set; }
        
        public int ProductBrandId { get; set; }
        
        public int CategoryId { get; set; }
        
        
        public bool IsActive { get; set; } = true;
        
        public ICollection<IFormFile>? NewImages { get; set; }
        
        public List<int>? ExistingImageIdsToKeep { get; set; }
    }
}