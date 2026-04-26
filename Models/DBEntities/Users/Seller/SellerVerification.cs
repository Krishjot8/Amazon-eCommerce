using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.SellerOnboarding;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountVerification.Meeting;


namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerVerification : BaseModel  //Active Case File
    {
        public int SellerUserId { get; set; }
        
        public IdentityDocumentType DocumentType { get; set; }

        public string? DocumentFrontUrl { get; set; } 
        
        public string? DocumentBackUrl { get; set; } 
        
        public ProofOfAddressType? ProofOfAddress { get; set; }
        
        public string? ProofOfAddressDocumentUrl { get; set; }

        public VerificationMeetingType VerificationMethod { get; set; }

        public DateTime? ScheduledDateTime { get; set; }
        
        public string? Notes { get; set; }
        

        public VerificationState VerificationStatus { get; set; } 
        
      

        public SellerUser SellerUser { get; set; } = null!;
        
        public ICollection<SellerVerificationDocument> Documents { get; set; } = new List<SellerVerificationDocument>();
    }
    
    public enum VerificationState
    {
        Pending,
        Approved,
        Rejected
    }
}