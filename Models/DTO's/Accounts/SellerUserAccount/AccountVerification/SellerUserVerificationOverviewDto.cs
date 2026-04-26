using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountVerification
{
    public class SellerUserVerificationOverviewDto
    {
        
        public int SellerUserId { get; set; }
        
        public SellerVerificationState CurrentState { get; set; }
        
        
        public bool IdentityDocsApproved { get; set; }
        
        public bool BusinessAddressPostcardVerified { get; set; }

        public bool VideoMeetingCompleted { get; set; }
        
        public bool TaxInterviewValidated { get; set; }
        
        public bool PayoutMethodCleared { get; set; }
        
        public DateTime? FinalApprovalDate { get; set; }
    }
}