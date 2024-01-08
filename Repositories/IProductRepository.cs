using Amazon_eCommerce_API.Models;

namespace Amazon_eCommerce_API.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(int id);

        Task<List<Product>> GetProductsAsync();
    }
}
