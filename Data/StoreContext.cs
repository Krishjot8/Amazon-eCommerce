using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Amazon_eCommerce_API.Models.DBEntities.Carts.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Orders;
using Amazon_eCommerce_API.Models.DBEntities.Preferences.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Products;
using Amazon_eCommerce_API.Models.DBEntities.Subscriptions;
using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller.TaxProfile;
using Amazon_eCommerce_API.Models.DBEntities.Shipments;

namespace Amazon_eCommerce_API.Data
{
    public class StoreContext(DbContextOptions options) : DbContext(options)
    {
        
        //Business Users
        
        public DbSet<BusinessUser> BusinessUsers { get; set; }
        
       
        public DbSet<BusinessPaymentProfile> BusinessPaymentProfiles { get; set; }
        
        public DbSet<BusinessProfile> BusinessProfiles { get; set; }
        
        public DbSet<BusinessStoreInformation> BusinessStoreInformation { get; set; }


        public DbSet<Category> Categories { get; set; }

        //Customer Users
        
        public DbSet<CustomerUser> CustomerUsers { get; set; }
        
        public DbSet<CustomerCart> CustomerCarts { get; set; }
        
        public DbSet<CustomerCartItem> CustomerCartItems { get; set; }
        
      
        public DbSet<CustomerPaymentProfile> CustomerPaymentProfiles { get; set; }
        
        public DbSet<CustomerPreferences> CustomerPreferences { get; set; }
        
        public DbSet<CustomerProfile> CustomerProfiles { get; set; }
        
        public DbSet<Order> Orders { get; set; }
        
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBrand> ProductBrands { get; set; }
        
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet <ProductType> ProductTypes { get; set; }
        
        public DbSet<ProductReview> ProductReviews { get; set; }
        
        public DbSet<ProductVariant> ProductVariants { get; set; }
        
       
        //Seller Users

        public DbSet<SellerUser> SellerUsers { get; set; }

        public DbSet<SellerBusinessInformation> SellerBusinessInformation { get; set; }
        
        public DbSet<SellerBusinessProfile> SellerBusinessProfiles { get; set; }
        
        public DbSet<SellerPaymentProfile> SellerPaymentProfiles { get; set; }
        
        public DbSet<SellerPrimaryContact> SellerPrimaryContacts { get; set; }
        
        public DbSet<SellerStoreInformation> SellerStoreInformation { get; set; }
        
        public DbSet<SellerVerification> SellerVerificationStatus { get; set; }
       
        public DbSet<SellerW9>  SellerW9 { get; set; }
        
        public DbSet<SellerTaxProfile>  SellerTaxProfiles { get; set; }

        public DbSet<Shipment> Shipments { get; set; }

        public DbSet<TaxAddress> TaxAddresses { get; set; }
        
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        public DbSet<UserSubscriptionPlan> UserSubscriptionPlans { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========================
            // BUSINESS
            // ========================

            modelBuilder.Entity<BusinessUser>()
                .HasIndex(u => u.BusinessEmail)
                .IsUnique();

            modelBuilder.Entity<BusinessProfile>()
                .HasOne(p => p.BusinessUser)
                .WithOne(u => u.BusinessProfile)
                .HasForeignKey<BusinessProfile>(p => p.BusinessUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BusinessStoreInformation>()
                .HasOne(p => p.BusinessUser)
                .WithOne(u => u.BusinessStoreInformation)
                .HasForeignKey<BusinessStoreInformation>(p => p.BusinessUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BusinessPaymentProfile>()
                .HasOne(p => p.BusinessUser)
                .WithMany(b => b.BusinessPaymentProfiles)
                .HasForeignKey(p => p.BusinessUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BusinessPaymentProfile>()
                .HasIndex(p => new { p.BusinessUserId, p.AccountNumber })
                .IsUnique();


            // ========================
            // CUSTOMER
            // ========================

            modelBuilder.Entity<CustomerUser>()
                .HasIndex(u => u.EmailAddress)
                .IsUnique();

            modelBuilder.Entity<CustomerProfile>()
                .HasOne(p => p.CustomerUser)
                .WithOne(u => u.CustomerProfile)
                .HasForeignKey<CustomerProfile>(p => p.CustomerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerProfile>()
                .HasIndex(p => p.CustomerUserId)
                .IsUnique();

            modelBuilder.Entity<CustomerPreferences>()
                .HasOne(p => p.CustomerUser)
                .WithOne(u => u.CustomerPreferences)
                .HasForeignKey<CustomerPreferences>(p => p.CustomerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerPreferences>()
                .HasIndex(p => p.CustomerUserId)
                .IsUnique();

            modelBuilder.Entity<CustomerPreferences>()
                .Property(p => p.PreferredCurrency)
                .HasConversion<string>();

            modelBuilder.Entity<CustomerPaymentProfile>()
                .HasOne(p => p.CustomerUser)
                .WithMany(u => u.PaymentProfiles)
                .HasForeignKey(p => p.CustomerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerPaymentProfile>()
                .HasIndex(p => p.CustomerUserId);

            modelBuilder.Entity<CustomerPaymentProfile>()
                .HasIndex(p => new { p.CustomerUserId, p.PaymentToken })
                .IsUnique();

            modelBuilder.Entity<CustomerCart>()
                .HasOne(c => c.CustomerUser)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.CustomerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerCartItem>()
                .HasOne(i => i.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CustomerCartItem>()
                .HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerCartItem>()
                .HasIndex(i => new { i.CartId, i.ProductId })
                .IsUnique();

            modelBuilder.Entity<CustomerCartItem>()
                .HasIndex(i => i.CartId);

            modelBuilder.Entity<CustomerCartItem>()
              .Property(p => p.Price)
               .HasPrecision(18, 2);

            // ========================
            // ORDERS
            // ========================

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
             .Property(o => o.TotalAmount)
             .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
             .Property(o => o.UnitPrice)
             .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.TotalPrice)
                .HasPrecision(18, 2);

            // ========================
            // PRODUCTS (CORE FIXED SECTION)
            // ========================

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductBrand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.ProductBrandId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductType)
                .WithMany(t => t.Products)
                .HasForeignKey(p => p.ProductTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.SellerUser)
                .WithMany()
                .HasForeignKey(p => p.SellerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
             .Property(p => p.Price)
             .HasPrecision(18, 2);

            
         

            // Product Images
            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductImage>()
                .HasIndex(pi => new { pi.ProductId, pi.IsPrimary })
                .HasFilter("[IsPrimary] = 1")
                .IsUnique();


            // Product Reviews
            modelBuilder.Entity<ProductReview>()
                .HasOne(r => r.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductReview>()
                .HasOne(r => r.CustomerUser)
                .WithMany()
                .HasForeignKey(r => r.CustomerUserId)
                .OnDelete(DeleteBehavior.Restrict);


            // Product Types
        


            // Product Variants
            modelBuilder.Entity<ProductVariant>()
                .HasOne(v => v.Product)
                .WithMany(p => p.Variants)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductVariant>()
             .Property(p => p.Price)
             .HasPrecision(18, 2);


            // ========================
            // SELLER
            // ========================

            modelBuilder.Entity<SellerUser>()
                .HasIndex(u => u.BusinessEmail)
                .IsUnique();

            modelBuilder.Entity<SellerBusinessProfile>()
                .HasOne(p => p.SellerUser)
                .WithOne(u => u.SellerBusinessProfile)
                .HasForeignKey<SellerBusinessProfile>(p => p.SellerUserId)
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
                .HasOne(p => p.SellerUser)
                .WithOne(u => u.SellerTaxProfile)
                .HasForeignKey<SellerTaxProfile>(p => p.SellerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SellerPaymentProfile>()
                .HasOne(p => p.SellerUser)
                .WithMany(u => u.SellerPaymentProfiles)
                .HasForeignKey(p => p.SellerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SellerPaymentProfile>()
                .HasIndex(p => new { p.SellerUserId, p.Last4Digits, p.CardBrand })
                .IsUnique();

            modelBuilder.Entity<SellerVerification>()
                .HasOne(v => v.SellerUser)
                .WithOne(u => u.SellerVerificationStatus)
                .HasForeignKey<SellerVerification>(v => v.SellerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SellerW9>()
                .HasOne(w => w.SellerUser)
                .WithOne(u => u.SellerW9)
                .HasForeignKey<SellerW9>(w => w.SellerUserId)
                .OnDelete(DeleteBehavior.Cascade);


            // ========================
            // SHIPPING
            // ========================

            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Order)
                .WithMany(o => o.Shipments)
                .HasForeignKey(s => s.OrderId)
                .OnDelete(DeleteBehavior.Cascade);


            // ========================
            // TAX
            // ========================

            modelBuilder.Entity<TaxAddress>()
                .HasOne(a => a.SellerTaxProfile)
                .WithMany(t => t.TaxAddresses)
                .HasForeignKey(a => a.SellerTaxProfileId)
                .OnDelete(DeleteBehavior.Cascade);


            // ========================
            // SUBSCRIPTIONS
            // ========================

            modelBuilder.Entity<UserSubscription>()
                .HasOne(s => s.CustomerUser)
                .WithOne(u => u.Subscription)
                .HasForeignKey<UserSubscription>(s => s.CustomerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserSubscription>()
                .HasOne(s => s.SubscriptionPlan)
                .WithMany()
                .HasForeignKey(s => s.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserSubscription>()
                .HasIndex(s => s.CustomerUserId)
                .IsUnique();

            modelBuilder.Entity<UserSubscriptionPlan>()
             .Property(p => p.Price)
             .HasPrecision(18, 2);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
        }
    }
}
