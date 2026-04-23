using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Customer
{
    public class CustomerPaymentProfile : BaseModel
    {
        
        public int CustomerUserId { get; set; }
        
        public CustomerUser CustomerUser { get; set; } = null!;
        
        public string PaymentToken { get; set; }
        
        public string CardType { get; set; }
        
        public string Last4Digits { get; set; }
        
        public DateTime ExpiryDate { get; set; }
        
        public bool IsDefault { get; set; }   

       
        
        
        
    }
}