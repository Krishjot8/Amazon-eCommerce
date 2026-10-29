using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Products;

namespace Amazon_eCommerce_API.Services.Products
{
    public class CatalogService : ICatalogService
    {
        
        private readonly StoreContext _storeContext;
        
        public CatalogService(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _storeContext.Categories.Add(category);
            await _storeContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(int id, Category category)
        {
            var existing = await _storeContext.Categories.FindAsync(id);
            if (existing == null) return null;
            
            existing.Name = category.Name;
            await _storeContext.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _storeContext.Categories.FindAsync(id);
            if (category == null) return false;
            
            _storeContext.Categories.Remove(category);
            return await _storeContext.SaveChangesAsync() > 0;
          
        }

        public async Task<ProductBrand> CreateProductBrandAsync(ProductBrand brand)
        {
            _storeContext.ProductBrands.Add(brand);
            await _storeContext.SaveChangesAsync();
            return brand;
        }

        public async Task<ProductBrand> UpdateProductBrandAsync(int id, ProductBrand brand)
        {
            var existingBrand = _storeContext.ProductBrands.Find(id);
            if (existingBrand == null) return null;
            existingBrand.Name = brand.Name;
            await _storeContext.SaveChangesAsync();
            return existingBrand;
        }

        public async Task<bool> DeleteProductBrandAsync(int id)
        {
            var brand = _storeContext.ProductBrands.Find(id);
            if (brand == null) return false;
            _storeContext.ProductBrands.Remove(brand);
            return await _storeContext.SaveChangesAsync() > 0;
        }

        public async Task<ProductType> CreateProductTypeAsync(ProductType type)
        {
            _storeContext.ProductTypes.Add(type);
            await _storeContext.SaveChangesAsync();
            return type;
        }

        public async Task<ProductType> UpdateProductTypeAsync(int id, ProductType type)
        {
            var existingType = _storeContext.ProductTypes.Find(id);
            
            if (existingType == null) return null;
            
            existingType.Name = type.Name;
            await _storeContext.SaveChangesAsync();
            return existingType;
        }

        public async Task<bool> DeleteProductTypeAsync(int id)
        {
            var productType = _storeContext.ProductTypes.Find(id);
            if (productType == null) return false;
            _storeContext.ProductTypes.Remove(productType);
            return await _storeContext.SaveChangesAsync() > 0;
        }
    }
}