

using Amazon_eCommerce_API.Models;
using Amazon_eCommerce_API.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

        public DbSet<Category> Categories { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
              .HasIndex(u => u.Email)
              .IsUnique();


            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();



            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}
