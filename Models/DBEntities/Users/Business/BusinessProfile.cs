namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessProfile   // whole business
    {

        public string BusinessUserId { get; set; }
        
        
        public string FirstName { get; set; }

        public string MiddleName { get; set; } = null;
        
        public string LastName { get; set; }
        
      //  public DateOnly DateOfBirth { get; set; }
        
        public bool ReceiveUpdates { get; set; } 
        
        
        public BusinessUser BusinessUser { get; set; }

    }
}
