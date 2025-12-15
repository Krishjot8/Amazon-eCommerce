using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerUser : BaseModel
    {

        public string SellerEmail { get; set; }
        
        public string PhoneNumber { get; set; }

        public string PasswordHash { get; set; }
        
        public bool IsEmailVerified { get; set; }
        
        
        public bool IsPhoneNumberVerified { get; set; }
        
        public SellerOnboardingStatus OnboardingStatus { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }


        

    }


    public enum SellerOnboardingStatus
    {
        AccountCreated = 1,
        BusinessInformationPending,
        BusinessInformationCompleted,
        PrimaryContactPending,
        IdentityVerificationPending,
        PaymentInformationPending,
        StoreInformationPending,
        UnderReview,
        Approved,
        Rejected,
        Suspended
        
        
    }
}
