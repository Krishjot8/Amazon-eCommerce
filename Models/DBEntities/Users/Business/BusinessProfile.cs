namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessProfile   // whole business
    {

        public int BusinessUserId { get; set; }
        
        
        public string FirstName { get; set; }

        public string? MiddleName { get; set; } 
        
        public string LastName { get; set; }
        
      //  public DateOnly DateOfBirth { get; set; }
        
        public bool ReceiveUpdates { get; set; } 
        
        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
        
        public BusinessUser BusinessUser { get; set; }

    }
}
