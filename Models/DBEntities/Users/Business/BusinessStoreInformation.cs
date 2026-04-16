namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessStoreInformation
    {

        
        public int BusinessUserId { get; set; }
        
        public string BusinessName { get; set; }
        
        
        public string BusinessType { get; set; }
        
        
        public string StreetAddress { get; set; }


        public string SuiteUnitFloor { get; set; } = null;


        public string City { get; set; }

     
        public string State { get; set; }
        
        
        public string ZipCode { get; set; }
        
        
        public BusinessUser BusinessUser { get; set; }


    }
}
