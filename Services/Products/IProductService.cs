using Amazon_eCommerce_API.Models.DTO_s.Products;
using Product = Amazon_eCommerce_API.Models.DBEntities.Products.Product;

namespace Amazon_eCommerce_API.Services.Products
{
    public interface IProductService
    {
        
        Task<Product> CreateProductAsync(ProductCreateDto createProductDto);
        
        Task<Product?> UpdateProductAsync(int id, ProductUpdateDto updateProductDto);   
        
        Task<bool> DeleteProductAsync(int productId);
    }
}
