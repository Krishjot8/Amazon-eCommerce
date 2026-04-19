using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessPaymentProfile : BaseModel
    {

        public int BusinessUserId { get; set; }
        
        public string BankName { get; set; }
        
        public string AccountNumber { get; set; }
        
        public string RoutingNumber { get; set; }
        
        public string TaxId { get; set; }
        
        public BusinessUser BusinessUser { get; set; }

    }
}
