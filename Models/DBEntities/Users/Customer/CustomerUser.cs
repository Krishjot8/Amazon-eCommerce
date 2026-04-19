using System.ComponentModel.DataAnnotations;
using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Carts.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Preferences.Customer;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Customer
{
    public class CustomerUser : BaseModel // To store user credentials to database after registering.
    {
    
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        
        public DateOnly DateOfBirth { get; set; }
        
        public string EmailAddress { get; set; }


        public string? PhoneNumber { get; set; }

        public string PasswordHash { get; set; }
        
        public bool IsEmailVerified { get; set; } = false;

        public CustomerProfile CustomerProfile { get; set; }
        public CustomerPreferences CustomerPreferences { get; set; }
        
        public List<CustomerPaymentProfile> PaymentProfiles { get; set; } = new();
        
        public List<CustomerCart> Carts { get; set; } = new();
        
      
        
        
        
      
    }
}
