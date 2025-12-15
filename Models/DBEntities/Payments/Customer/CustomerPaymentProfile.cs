namespace Amazon_eCommerce_API.Models.DBEntities.Users.Customer
{
    public class CustomerPaymentProfile
    {
        public string PaymentId { get; set; }
        
        public int CustomerUserId { get; set; }
        
        public CustomerUser CustomerUser { get; set; }
        
        public string PaymentToken { get; set; }
        
        public string CardType { get; set; }
        
        public string Last4Digits { get; set; }
        
        public DateTime ExpiryDate { get; set; }
        
        
        
        
    }
}