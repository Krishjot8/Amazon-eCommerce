using System.ComponentModel.DataAnnotations;
using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessUser : BaseModel
    {
        

        public string BusinessEmail { get; set; } = null!;

        public string BusinessPhoneNumber { get; set; } = null!;

        public string  PasswordHash { get; set; } = null!;

        public bool IsBusinessEmailVerified { get; set; } = false;

        public bool IsBusinessPhoneVerified { get; set; } = false;
        
        public bool IsDeleted { get; set; } 
        
        public DateTime? DeletedAt { get; set; }
        public BusinessProfile? BusinessProfile { get; set; }
        
        public BusinessStoreInformation? BusinessStoreInformation { get; set; }
        
        public List<BusinessPaymentProfile> BusinessPaymentProfiles { get; set; }  = new();
        
        
        
        
       


       
      

      
   

      
    
    }
}
