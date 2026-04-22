using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;

namespace Amazon_eCommerce_API.Models.DBEntities.Subscriptions
{
    public class UserSubscription : BaseModel
    
    {
        public int CustomerUserId { get; set; }
        
        public CustomerUser CustomerUser { get; set; }
        
        public SubscriptionType SubscriptionType { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        
        
    }

    public enum SubscriptionType
    {
        None = 0,
        Prime = 1
        
    }
}