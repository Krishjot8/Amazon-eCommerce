using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessProfile : BaseModel  // whole business
    {

        public int BusinessUserId { get; set; }
        
        
        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; } 
        
        public string LastName { get; set; } = null!;


        public bool ReceiveUpdates { get; set; } 
        
        
        public BusinessUser BusinessUser { get; set; } = null!;

    }
}
