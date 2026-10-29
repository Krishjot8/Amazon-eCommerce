using Amazon_eCommerce_API.Models.DBEntities.Products;

namespace Amazon_eCommerce_API.Services.Products
{
    public interface ICatalogService
    {
        
        
        
        Task<Category> CreateCategoryAsync(Category category);
        
        Task<Category?>UpdateCategoryAsync(int id, Category category);
        
        Task<bool> DeleteCategoryAsync(int id);
        
        
        Task<ProductBrand?> CreateProductBrandAsync(ProductBrand brand);
        
        Task<ProductBrand?> UpdateProductBrandAsync(int id, ProductBrand brand);

        Task<bool> DeleteProductBrandAsync(int id);

        
        Task<ProductType> CreateProductTypeAsync(ProductType type);

        Task<ProductType?> UpdateProductTypeAsync(int id, ProductType type);

        Task<bool> DeleteProductTypeAsync(int id);
    }
}