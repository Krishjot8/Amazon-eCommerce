using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Models.DTO_s.Products;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Services.Products
{
    public class ProductService : IProductService
    {
        
        
        private readonly StoreContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductService(StoreContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<Product> CreateProductAsync(ProductCreateDto createProductDto)
        {
            var product = new Product
            {

                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                ProductTypeId = createProductDto.ProductTypeId,
                ProductBrandId = createProductDto.ProductBrandId,
                CategoryId = createProductDto.CategoryId,
                IsActive = true

            };
            
            // mapping and handling image uploads handling variances, and dynamic attributes will come later.
            
            

            _context.Products.Add(product);
            _context.SaveChanges();
            
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateProductAsync(int id, ProductUpdateDto updateProductDto)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .Include(p => p.Variants)
                .Include(p => p.AttributeValues)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return null;
            
            
            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.StockQuantity = updateProductDto.StockQuantity;
            product.ProductTypeId = updateProductDto.ProductTypeId;
            product.ProductBrandId = updateProductDto.ProductBrandId;
            product.CategoryId = updateProductDto.CategoryId;
            product.IsActive = updateProductDto.IsActive;
            
            
            
            await _context.SaveChangesAsync();
            
            return product;
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _context.Products
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}