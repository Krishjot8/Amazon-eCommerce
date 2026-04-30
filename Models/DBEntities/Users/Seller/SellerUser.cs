using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller.TaxProfile;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerUser : BaseModel
    {

        public string BusinessEmail { get; set; } = null!;

        public string BusinessPhoneNumber { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public bool IsEmailVerified { get; set; }
        
        public bool IsPhoneNumberVerified { get; set; }
        
        public SellerOnboardingStep OnboardingStep { get; set; }
        
        public SellerAccountStatus AccountStatus { get; set; }

        public SellerVerification SellerVerificationStatus { get; set; } = null!;

        public SellerBusinessProfile? SellerBusinessProfile { get; set; }

        public SellerPrimaryContact? SellerPrimaryContact { get; set; } 

        public SellerStoreInformation? SellerStoreInformation { get; set; } 

        public SellerTaxProfile? SellerTaxProfile { get; set; } 


        public SellerW9? SellerW9 { get; set; } 


        public List<SellerPaymentProfile> SellerPaymentProfiles { get; set; } = new();
        
        public ICollection<SellerPayoutAccount> SellerPayoutAccounts { get; set; } = new List<SellerPayoutAccount>();

        public ICollection<SellerVerificationDocument> VerificationDocuments { get; set; }
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
