using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerVerificationStatus
    {
        public int SellerUserId { get; set; }
        
        public IdentityProof DocumentType{ get; set; }

        public string DocumentFrontUrl { get; set; } = null;
        
        public string DocumentBackUrl { get; set; } = null;
        
        public ProofOfAddress ProofOfAddress { get; set; }
        
        public string ProofOfAddressDocumentUrl { get; set; }
        
        public VerificationMeetingType ScheduledVerificationType { get; set; }

        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;

        public SellerUser SellerUser { get; set; }
    }
}