using System.ComponentModel.DataAnnotations.Schema;
using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Customer
{
    public class CustomerProfile : BaseModel
    {
        public int CustomerUserId { get; set; }
        
        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; } 
        
        public string LastName { get; set; } = null!;

        public string? PhoneNumber { get; set; }
        
        
        public string? ProfilePictureUrl { get; set; }

        public string? DefaultShippingAddress { get; set; } 
        
      
        public CustomerUser CustomerUser { get; set; } = null!;
    }
}