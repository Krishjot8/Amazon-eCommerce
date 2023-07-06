

using Amazon_eCommerce_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {


        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet <ProductType> ProductTypes { get; set; }
    }
}
