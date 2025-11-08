

using Amazon_eCommerce_API.Models;
using Amazon_eCommerce_API.Models.DBEntities.Users;
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

        public DbSet<CustomerUsers> CustomerUsers { get; set; }

        public DbSet<BusinessUsers> BusinessUsers { get; set; }

        public DbSet<SellerUsers> SellerUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerUsers>()
              .HasIndex(u => u.Email)
              .IsUnique();


            modelBuilder.Entity<CustomerUsers>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<BusinessUsers>()
          .HasIndex(u => u.Email)
          .IsUnique();


        

            modelBuilder.Entity<SellerUsers>()
                      .HasIndex(u => u.Email)
                      .IsUnique();


            modelBuilder.Entity<SellerUsers>()
                .HasIndex(u => u.Username)
                .IsUnique();




            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}
