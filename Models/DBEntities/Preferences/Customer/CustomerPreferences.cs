using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;

namespace Amazon_eCommerce_API.Models.DBEntities.Preferences.Customer
{
    public class CustomerPreferences
    {
        public int CustomerPreferencesId { get; set; }
        
        public int CustomerUserId { get; set; }
        
        public CustomerUser CustomerUser { get; set; }
        
        public bool SubscribeToNewsletter { get; set; }
        
        public string PreferredLanguage { get; set; }
        
        public string PreferredCurrency { get; set; }
        
        public bool ReceivePromotions { get; set; }
        
    }
}