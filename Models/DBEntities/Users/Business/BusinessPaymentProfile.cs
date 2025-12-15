namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessPaymentProfile
    {

        public int BusinessUserId { get; set; }
        
        public int BankName { get; set; }
        
        public int AccountNumber { get; set; }
        
        public string RoutingNumber { get; set; }
        
        public string TaxId { get; set; }
        
        public BusinessUser BusinessUser { get; set; }

    }
}
