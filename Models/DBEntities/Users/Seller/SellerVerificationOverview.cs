using System.ComponentModel.DataAnnotations;
using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerVerificationOverview: BaseModel   //Account Passport
    {
        
        [Required]
        public int SellerUserId { get; set; }

        [Required]
        public SellerVerificationState CurrentState { get; set; } = SellerVerificationState.Incomplete;
        
        public bool IdentityDocsApproved { get; set; } = false;
        
        public bool BusinessAddressPostcardVerified { get; set; } = false;
        
        public bool VideoMeetingCompleted { get; set; } = false;
        
        public bool TaxInterviewValidated { get; set; } = false;

        public bool PayoutMethodCleared { get; set; } = false;
        
        public DateTime? FinalApprovalDate { get; set; }
        
        
    }
    
    public enum SellerVerificationState
    {
        
        Incomplete = 0,
        UnderReview = 1,
        ActionRequired = 2,
        Verified = 3,
        Rejected = 4,
        Suspended = 5 
    }
}