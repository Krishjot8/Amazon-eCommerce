using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessStoreInformation : BaseModel
    {

        
        public int BusinessUserId { get; set; }
         
        public string BusinessName { get; set; } = null!;


        public string BusinessType { get; set; } = null!;


        public string StreetAddress { get; set; } = null!;


        public string? SuiteUnitFloor { get; set; } 


        public string City { get; set; } = null!;


        public string State { get; set; } = null!;


        public string CountryCode { get; set; } = null!;

        public string ZipCode { get; set; } = null!;



        public BusinessUser BusinessUser { get; set; }  = null!;


    }
}
