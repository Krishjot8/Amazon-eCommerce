using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Subscriptions
{
    public class UserSubscriptionPlan: BaseModel
    {

        public string Name { get; set; } = null!;
        
        public decimal Price { get; set; }
        
        public int DurationInDays { get; set; }

        public string? Description { get; set; } 

    }
}
