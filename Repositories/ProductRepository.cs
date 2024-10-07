using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<ProductBrand> GetProductBrandByIdAsync(int id)
        {

            return await _context.ProductBrands.FindAsync(id);
        }

        public async Task<List<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var typeId = 1;
            var product = _context.Products.Where(x => x.ProductTypeId == typeId).Include(x => x.ProductType).ToListAsync();


            return await _context.Products
                .Include(p => p.ProductType)
                .Include(x => x.ProductBrand)
                .ToListAsync();
        }

        public async Task<ProductType> GetProductTypeByIdAsync(int id)
        {
            return await _context.ProductTypes.FindAsync(id);
        }

        public async Task<List<ProductType>> GetProductTypesAsync()
        {
           return await _context.ProductTypes.ToListAsync();
        }
    }
}
