

using Amazon_eCommerce_API.Models;
using Amazon_eCommerce_API.Models.DBEntities.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Amazon_eCommerce_API.Models.DBEntities.Carts.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Orders.BusinessOrders;
using Amazon_eCommerce_API.Models.DBEntities.Orders.CustomerOrders;
using Amazon_eCommerce_API.Models.DBEntities.Orders.SellerOrders;
using Amazon_eCommerce_API.Models.DBEntities.Preferences.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;

namespace Amazon_eCommerce_API.Data
{
    public class StoreContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Products> Products { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet <ProductType> ProductTypes { get; set; }

        public DbSet<Category> Categories { get; set; }

        //Customer Users
        
        public DbSet<CustomerCart> CustomerCarts { get; set; }
        
        public DbSet<CustomerCartItem> CustomerCartItems { get; set; }
        
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        
        public DbSet<CustomerOrderItem> CustomerOrderItems { get; set; }
        
        public DbSet<CustomerPaymentProfile> CustomerPaymentProfiles { get; set; }
        public DbSet<CustomerPreferences> CustomerPreferences { get; set; }
        
        public DbSet<CustomerProfile> CustomerProfiles { get; set; }
        public DbSet<CustomerUser> CustomerUsers { get; set; }
        
        //Business Users
        
        public DbSet<BusinessOrder> BusinessOrders { get; set; }
        
        public DbSet<BusinessOrderItem> BusinessOrderItems { get; set; }
        
        public DbSet<BusinessPaymentProfile> BusinessPaymentProfiles { get; set; }
        
        public DbSet<BusinessProfile> BusinessProfiles { get; set; }
        
        public DbSet<BusinessStoreInformation> BusinessStoreInformation { get; set; }
        public DbSet<BusinessUser> BusinessUsers { get; set; }
        
        //Seller Users

        public DbSet<SellerOrder> SellerOrders { get; set; }
        
        public DbSet<SellerOrderItem> SellerOrderItems { get; set; }
        
        public DbSet<SellerBusinessInformation> SellerBusinessInformation { get; set; }
        
        public DbSet<SellerBusinessProfile> SellerBusinessProfiles { get; set; }
        
        public DbSet<SellerPaymentProfile> SellerPaymentProfiles { get; set; }
        
        public DbSet<SellerPrimaryContact> SellerPrimaryContacts { get; set; }
        
        public DbSet<SellerProfile> SellerProfiles { get; set; }
        
        public DbSet<SellerStoreInformation> SellerStoreInformation { get; set; }
        public DbSet<SellerUser> SellerUsers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerUser>()
              .HasIndex(u => u.EmailAddress)
              .IsUnique();


          
            modelBuilder.Entity<BusinessUser>()
          .HasIndex(u => u.BusinessEmail)
          .IsUnique();


        

            modelBuilder.Entity<SellerUser>()
                      .HasIndex(u => u.SellerEmail)
                      .IsUnique();


           



            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}
