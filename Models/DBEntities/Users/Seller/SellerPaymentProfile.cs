namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerPaymentProfile
    {
        public int SellerUserId { get; set; }
        
        public string PaymentProviderToken { get; set; }
        
        public string CardBrand { get; set; }
        
        public string CardHolderName { get; set; }
        
        public string Last4Digits { get; set; }
        
        public bool IsDefaultPaymentMethod { get; set; }
        
        public int ExpirationMonth { get; set; }
        
        public int ExpirationYear { get; set; }
        
        
        public string BillingAddressLine1 { get; set; }
        
        public string BillingAddressLine2 { get; set; } = null;
        
        
        public string BillingCity { get; set; }
        
        public string BillingState { get; set; }
        
        public string BillingZipCode { get; set; }
        
        public string BillingCountry { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public SellerUser SellerUser { get; set; }
    }
}