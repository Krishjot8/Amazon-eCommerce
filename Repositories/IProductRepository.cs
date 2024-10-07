using Amazon_eCommerce_API.Models;

namespace Amazon_eCommerce_API.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);

        Task<List<Product>> GetProductsAsync();

        Task<List<ProductBrand>> GetProductBrandsAsync();

        Task<List<ProductType>> GetProductTypesAsync();

        Task<ProductBrand> GetProductBrandByIdAsync(int id);

        Task<ProductType> GetProductTypeByIdAsync(int id);
    }
}
