using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessPaymentProfile : BaseModel
    {

        public int BusinessUserId { get; set; }
        
        public string BankName { get; set; } = null!;

        public string AccountNumber { get; set; } = null!;

        public string RoutingNumber { get; set; } = null!;

        public string TaxId { get; set; } = null!;

        public BusinessUser BusinessUser { get; set; } = null!;

    }
}
