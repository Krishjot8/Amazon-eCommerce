using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;

namespace Amazon_eCommerce_API.Models.DBEntities.Preferences.Customer
{
    public class CustomerPreferences : BaseModel
    {
        
        public int CustomerUserId { get; set; }

        public CustomerUser CustomerUser { get; set; } = null!;
        
        public bool SubscribeToNewsletter { get; set; }
        
        public string PreferredLanguage { get; set; } = "en-US";
        
        public Currency PreferredCurrency { get; set; } 
        
        public bool ReceivePromotions { get; set; }
        
    }

 

    public enum Currency
    {
        USD,
        EUR,
        GBP,
        INR,
        JPY
        
        
    }
}