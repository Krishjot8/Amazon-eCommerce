using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerUser : BaseModel
    {

        public string BusinessEmail { get; set; }
        
        public string BusinessPhoneNumber { get; set; }

        public string PasswordHash { get; set; }
        
        public bool IsEmailVerified { get; set; }
        
        public bool IsPhoneNumberVerified { get; set; }
        
        public SellerOnboardingStep OnboardingStep { get; set; }
        
        public SellerAccountStatus AccountStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }


        

    }


    public enum SellerOnboardingStep
    {
        AccountCreated = 1,
        BusinessInformation = 2,
        BusinessProfile = 3,
        PrimaryContact = 4,
        PaymentInformation = 5,
        StoreInformation = 6,
        VerificationSubmitted = 7,
        MeetingScheduled = 8,
        Completed = 9
        
        
    }

    public enum SellerAccountStatus
    {
        Pending = 1,
        UnderReview = 2,
        Approved = 3 ,
        Rejected = 4,
        Suspended = 5
        
    }
}
