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
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller.TaxProfile;

namespace Amazon_eCommerce_API.Data
{
    public class StoreContext(DbContextOptions options) : DbContext(options)
    {
        
        //Business Users
        
        public DbSet<BusinessUser> BusinessUsers { get; set; }
        
        public DbSet<BusinessOrder> BusinessOrders { get; set; }
        
        public DbSet<BusinessOrderItem> BusinessOrderItems { get; set; }
        
        public DbSet<BusinessPaymentProfile> BusinessPaymentProfiles { get; set; }
        
        public DbSet<BusinessProfile> BusinessProfiles { get; set; }
        
        public DbSet<BusinessStoreInformation> BusinessStoreInformation { get; set; }


        public DbSet<Category> Categories { get; set; }

        //Customer Users
        
        public DbSet<CustomerUser> CustomerUsers { get; set; }
        
        public DbSet<CustomerCart> CustomerCarts { get; set; }
        
        public DbSet<CustomerCartItem> CustomerCartItems { get; set; }
        
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        
        public DbSet<CustomerOrderItem> CustomerOrderItems { get; set; }
        
        public DbSet<CustomerPaymentProfile> CustomerPaymentProfiles { get; set; }
        
        public DbSet<CustomerPreferences> CustomerPreferences { get; set; }
        
        public DbSet<CustomerProfile> CustomerProfiles { get; set; }


        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }
        
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet <ProductType> ProductTypes { get; set; }

      
        
       
        //Seller Users

        public DbSet<SellerUser> SellerUsers { get; set; }
        
        public DbSet<SellerOrder> SellerOrders { get; set; }
        
        public DbSet<SellerOrderItem> SellerOrderItems { get; set; }
        
        public DbSet<SellerBusinessInformation> SellerBusinessInformation { get; set; }
        
        public DbSet<SellerBusinessProfile> SellerBusinessProfiles { get; set; }
        
        public DbSet<SellerPaymentProfile> SellerPaymentProfiles { get; set; }
        
        public DbSet<SellerPrimaryContact> SellerPrimaryContacts { get; set; }
        
        public DbSet<SellerStoreInformation> SellerStoreInformation { get; set; }
        
        public DbSet<SellerVerificationStatus> SellerVerificationStatus { get; set; }
       
        public DbSet<SellerW9>  SellerW9 { get; set; }
        
        public DbSet<SellerTaxProfile>  SellerTaxProfiles { get; set; }
        
        public DbSet<TaxAddress> TaxAddresses { get; set; }
        
        
        


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            
            
            modelBuilder.Entity<BusinessPaymentProfile>()
                .HasOne(p => p.BusinessUser)
                .WithMany(b => b.BusinessPaymentProfiles)
                .HasForeignKey(p => p.BusinessUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BusinessProfile>()
                .HasKey(p => p.BusinessUserId);
            
            
            modelBuilder.Entity<BusinessProfile>()
                .HasOne(p => p.BusinessUser)
                .WithOne(u => u.BusinessProfile)
                .HasForeignKey<BusinessProfile>(p => p.BusinessUserId)
                .OnDelete(DeleteBehavior.Cascade);
                
                modelBuilder.Entity<BusinessStoreInformation>()
                    .HasKey(p => p.BusinessUserId);
                
                modelBuilder.Entity<BusinessStoreInformation>()
                    .HasOne(p => p.BusinessUser)
                    .WithOne(u => u.BusinessStoreInformation)
                    .HasForeignKey<BusinessStoreInformation>(p => p.BusinessUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            
            
                
            modelBuilder.Entity<BusinessUser>()
                .HasIndex(u => u.BusinessEmail)
                .IsUnique();


            modelBuilder.Entity<CustomerUser>()
              .HasIndex(u => u.EmailAddress)
              .IsUnique();
          
            
            modelBuilder.Entity<CustomerCart>()
                .HasOne(cart => cart.CustomerUser)
                .WithMany(u => u.Carts)
                .HasForeignKey(cart => cart.CustomerUserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<CustomerCartItem>()
                .HasOne(item => item.Cart)
                .WithMany(cart => cart.Items)
                .HasForeignKey(item => item.CartId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<CustomerCartItem>()
                .HasOne(item => item.Products)
                .WithMany()
                .HasForeignKey(item => item.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<CustomerCartItem>()
                .HasIndex(item => new { item.CartId, item.ProductId })
                .IsUnique();
                
            modelBuilder.Entity<CustomerCartItem>()
                .HasIndex(item => item.CartId);

         //   modelBuilder.Entity( < CustomerOrderItems >


         modelBuilder.Entity<CustomerProfile>()
             .HasKey(p => p.CustomerUserId);
         
         modelBuilder.Entity<CustomerProfile>()
             .HasOne(p => p.CustomerUser)
             .WithOne(u => u.CustomerProfile)
             .HasForeignKey<CustomerProfile>(p => p.CustomerUserId)
             .OnDelete(DeleteBehavior.Cascade);

         modelBuilder.Entity<CustomerPaymentProfile>()
             .HasOne(p => p.CustomerUser)
             .WithMany(u => u.PaymentProfiles)
             .HasForeignKey(p => p.CustomerUserId)
             .OnDelete(DeleteBehavior.Cascade);
             
             modelBuilder.Entity<CustomerPaymentProfile>()
                 .HasKey(p => p.PaymentId);
             
             modelBuilder.Entity<CustomerPaymentProfile>()
                 .HasIndex(p => p.PaymentToken)
                 .IsUnique();
             
             modelBuilder.Entity<CustomerPreferences>()
                 .HasOne(p => p.CustomerUser)
                 .WithOne(u => u.CustomerPreferences)
                 .HasForeignKey<CustomerPreferences>(p => p.CustomerUserId)
                 .OnDelete(DeleteBehavior.Cascade);
             
             modelBuilder.Entity<CustomerPreferences>()
                 .HasIndex(p => p.CustomerUserId)
                 .IsUnique();
             
           
       

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductBrand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.ProductBrandId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Product>()
                .HasOne(p=> p.ProductType)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);


           modelBuilder.Entity<ProductImage>()
               .HasOne(pi => pi.Product)
               .WithMany(p => p.Images)
               .HasForeignKey(pi => pi.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
           
           modelBuilder.Entity<ProductImage>()
               .HasIndex(pi => new { pi.ProductId, pi.isPrimary })
               .HasFilter("[isPrimary] = 1")
               .IsUnique();

           
           modelBuilder.Entity<SellerBusinessInformation>()
               .HasOne(b=> b.SellerUser)
               .WithOne(u => u.SellerBusinessInformation)
               .HasForeignKey<SellerBusinessInformation>(b => b.SellerUserId)
               .OnDelete(DeleteBehavior.Cascade);
           
           
           modelBuilder.Entity<SellerBusinessProfile>()
               .HasOne(p => p.SellerUser)
               .WithOne(u => u.SellerBusinessProfile)
               .HasForeignKey<SellerBusinessProfile>(p => p.SellerUserId)
               .OnDelete(DeleteBehavior.Cascade);
           
           
           modelBuilder.Entity<SellerPaymentProfile>()
               .HasOne(p => p.SellerUser)
               .WithMany(u => u.SellerPaymentProfiles)
               .HasForeignKey(p => p.SellerUserId)
               .OnDelete(DeleteBehavior.Cascade);
           
           
           modelBuilder.Entity<SellerPrimaryContact>()
               .HasOne(p => p.SellerUser)
               .WithOne(u => u.SellerPrimaryContact)
               .HasForeignKey<SellerPrimaryContact>(p => p.SellerUserId)
               .OnDelete(DeleteBehavior.Cascade);
           
           modelBuilder.Entity<SellerStoreInformation>()
               .HasOne(p => p.SellerUser)
               .WithOne(u => u.SellerStoreInformation)
               .HasForeignKey<SellerStoreInformation>(p => p.SellerUserId)
               .OnDelete(DeleteBehavior.Cascade);
           
           modelBuilder.Entity<SellerTaxProfile>()
               .HasOne(p=> p.SellerUser)
               .WithOne(u => u.SellerTaxProfile)
               .HasForeignKey<SellerTaxProfile>(p => p.SellerUserId)
               .OnDelete(DeleteBehavior.Cascade);

           modelBuilder.Entity<SellerUser>()
               .HasIndex(u => u.BusinessEmail)
               .IsUnique();
           
           modelBuilder.Entity<SellerVerificationStatus>()
               .HasOne(p => p.SellerUser)
               .WithOne(u => u.SellerVerificationStatus)
               .HasForeignKey<SellerVerificationStatus>(p => p.SellerUserId)
               .OnDelete(DeleteBehavior.Cascade);
           
           modelBuilder.Entity<SellerW9>()
               .HasOne(p => p.SellerUser)
               .WithOne(u => u.SellerW9)
               .HasForeignKey<SellerW9>(p => p.SellerUserId)
               .OnDelete(DeleteBehavior.Cascade);
           
           modelBuilder.Entity<TaxAddress>()
               .HasOne(a => a.SellerTaxProfile)
               .WithMany(t => t.TaxAddresses)
               .HasForeignKey(a => a.SellerTaxProfileId)
               .OnDelete(DeleteBehavior.Cascade);
           

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}
