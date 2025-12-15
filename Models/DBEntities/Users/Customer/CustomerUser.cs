using System.ComponentModel.DataAnnotations;
using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Carts.Customer;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Customer
{
    public class CustomerUser : BaseModel // To store user credentials to database after registering.
    {
    
        public string FirstName { get; set; }

     
        public string LastName { get; set; }
        
        
        public DateOnly DateOfBirth { get; set; }
        
        public string EmailAddress { get; set; }


        public string PhoneNumber { get; set; } = null;

        public string PasswordHash { get; set; }
        
        public bool IsEmailVerified { get; set; } = false;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt {  get; set; } = DateTime.UtcNow;
        
        
        public CustomerPaymentProfile PaymentProfile { get; set; }
        
        public CustomerCart Cart { get; set; }
        
        
        
      
    }
}
