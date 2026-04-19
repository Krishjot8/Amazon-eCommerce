using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerVerificationStatus : BaseModel
    {
        public int SellerUserId { get; set; }
        
        public IdentityProof DocumentType{ get; set; }

        public string? DocumentFrontUrl { get; set; } 
        
        public string? DocumentBackUrl { get; set; } 
        
        public ProofOfAddress? ProofOfAddress { get; set; }
        
        public string? ProofOfAddressDocumentUrl { get; set; }
        
        public DateTime? ScheduledDateTime { get; set; }
        
        public string? Notes { get; set; }
        
        public VerificationMeetingType VerificationMethod { get; set; }

        public VerificationStatus VerificationStatus { get; set; } 
        
      

        public SellerUser SellerUser { get; set; }
    }
    
    public enum VerificationStatus
    {
        Pending,
        Approved,
        Rejected
    }
}