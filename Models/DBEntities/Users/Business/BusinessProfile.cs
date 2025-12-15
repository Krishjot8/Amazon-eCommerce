namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessProfile
    {

        public string BusinessUserId { get; set; }
        
        
        public string FirstName { get; set; }

        public string MiddleName { get; set; } = null;
        
        public string LastName { get; set; }
        
        public DateOnly DateOfBirth { get; set; }
        
        public bool ReceiveUpdates { get; set; } = false;
        
        
        public BusinessUser BusinessUser { get; set; }

    }
}
