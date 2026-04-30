using System.ComponentModel.DataAnnotations;
using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Carts.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Preferences.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Subscriptions;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Customer
{
    public class CustomerUser : BaseModel // To store user credentials to database after registering.
    {
    
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        
        public string EmailAddress { get; set; } = null!;


        public string? MobileNumber { get; set; }

        public string PasswordHash { get; set; } = null!;

        public bool IsEmailVerified { get; set; } = false;

        public CustomerProfile CustomerProfile { get; set; } = null!;

        public CustomerPreferences CustomerPreferences { get; set; } = null!;

        public UserSubscription Subscription { get; set; } = null!;

        public List<CustomerPaymentProfile> PaymentProfiles { get; set; } = new();
        
        public List<CustomerCart> Carts { get; set; } = new();
        
      
        
        
        
      
    }
}
