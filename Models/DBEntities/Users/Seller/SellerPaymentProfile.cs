using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerPaymentProfile : BaseModel
    {
        public int SellerUserId { get; set; }
        
        public string PaymentProviderToken { get; set; } = null!;

        public string CardBrand { get; set; } = null!;

        public string CardHolderName { get; set; } = null!;

        public string Last4Digits { get; set; } = null!;

        public bool IsDefaultPaymentMethod { get; set; }
        
        public int ExpirationMonth { get; set; }
        
        public int ExpirationYear { get; set; }
        
        
        public string BillingAddressLine1 { get; set; } = null!;

        public string? BillingAddressLine2 { get; set; } 
        
        
        public string BillingCity { get; set; } = null!;

        public string BillingState { get; set; } = null!;

        public string BillingZipCode { get; set; } = null!;

        public string BillingCountry { get; set; } = null!;


        public SellerUser SellerUser { get; set; } = null!;
    }
}