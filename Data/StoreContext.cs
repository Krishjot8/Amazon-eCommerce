

using Amazon_eCommerce_API.Models;
using Amazon_eCommerce_API.Models.DBEntities.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;

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

        public DbSet<CustomerUser> CustomerUsers { get; set; }

        public DbSet<BusinessUser> BusinessUsers { get; set; }

        public DbSet<SellerUser> SellerUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerUser>()
              .HasIndex(u => u.Email)
              .IsUnique();


            modelBuilder.Entity<CustomerUser>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<BusinessUser>()
          .HasIndex(u => u.Email)
          .IsUnique();


        

            modelBuilder.Entity<SellerUser>()
                      .HasIndex(u => u.Email)
                      .IsUnique();


            modelBuilder.Entity<SellerUser>()
                .HasIndex(u => u.Username)
                .IsUnique();




            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}
