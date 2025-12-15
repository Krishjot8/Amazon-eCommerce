using System.ComponentModel.DataAnnotations.Schema;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Customer
{
    public class CustomerProfile
    {
        public int CustomerUserId { get; set; }
        
        public string FirstName { get; set; }

        public string MiddleName { get; set; } = null;
        
        public string LastName { get; set; }
        
        public string PhoneNumber { get; set; }
        
        
        public string ProfilePictureUrl { get; set; }

        public string DefaultShippingAddress { get; set; } = null;
        
        [ForeignKey("CustomerUserId")]
        public CustomerUser CustomerUser { get; set; }
    }
}