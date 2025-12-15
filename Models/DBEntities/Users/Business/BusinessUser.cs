using System.ComponentModel.DataAnnotations;
using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessUser : BaseModel
    {
        

        public string BusinessEmail { get; set; }
        
        
        public string  PasswordHash { get; set; }
        
        
        public string BusinessPhoneNumber { get; set; }

        public bool IsBusinessEmailVerified { get; set; } = false;

        public bool IsBusinessPhoneVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        
        public BusinessProfile BusinessProfile { get; set; }
        
        public BusinessStoreInformation BusinessStoreInformation { get; set; }
        
        BusinessPaymentProfile BusinessPaymentProfile { get; set; }
        
        
        
        
       


       
      

      
   

      
    
    }
}
