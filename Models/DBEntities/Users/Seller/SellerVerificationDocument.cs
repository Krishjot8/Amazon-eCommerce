using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerVerificationDocument : BaseModel
    {
        public int SellerUserId { get; set; }
        
        public VerificationCategory Category { get; set; }
        
        public int DocumentType { get; set; }
        
        public string DocumentUrl { get; set; }
        
        public DateTime? IssuanceDate { get; set; }
        
        public DateTime? ExpiryDate { get; set; }

        public DocumentStatus Status { get; set; } = DocumentStatus.Pending;
        
        public string? RejectionReason { get; set; }
            
        public DateTime? VerifiedAt { get; set; }

        public virtual SellerVerification SellerVerification { get; set; } = null!;
    }
            
            
    }

    public enum VerificationCategory
    {
        
        Identity = 1,
        BusinessAddress = 2,
        ResidentialAddress = 3,
        BusinessRegistration = 4,
        LetterOfAuthorization = 5
        
    }


    public enum DocumentStatus
    {
        
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Expired = 3,
        ActionRequired = 4
    }
