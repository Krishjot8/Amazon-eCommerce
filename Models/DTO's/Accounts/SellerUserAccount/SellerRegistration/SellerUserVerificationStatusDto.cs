using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration
{
    public class SellerUserVerificationStatusDto
    {
        
        [Required]
        [EnumDataType(typeof(IdentityProof),ErrorMessage = "Invalid document type.")]
        public IdentityProof DocumentType { get; set; }
        
     
        [StringLength(500, ErrorMessage = "Document front URL cannot exceed 500 characters.")]
        [Url(ErrorMessage = "Document front URL must be a valid URL.")]
        public string DocumentFrontUrl { get; set; } = null;
        
       
        [StringLength(500, ErrorMessage = "Document back URL cannot exceed 500 characters.")]
        [Url(ErrorMessage = "Document back URL must be a valid URL.")]
        public string DocumentBackUrl { get; set; } = null;
        
        [Required]
        [EnumDataType(typeof(ProofOfAddress),ErrorMessage = "Invalid proof of address type.")]
        public  ProofOfAddress ProofOfAddress { get; set; }
        
     
        [StringLength(500, ErrorMessage = "Proof of address document URL cannot exceed 500 characters.")]
        [Url(ErrorMessage = "Proof of address document URL must be a valid URL.")]
        public string ProofOfAddressDocumentUrl { get; set; } = null;

        [Required]
        public VerificationMeetingType ScheduledVerificationType { get; set; }
        
        [EnumDataType(typeof(VerificationStatus))]
        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;

        
    }

    public enum ProofOfAddress
    {
        BankStatement,
        UtilityBill,
        LeaseAgreement,
        Other
        
    }

    public enum VerificationStatus
    {
        Pending,
        Approved,
        Rejected
        
    }

    public enum VerificationMeetingType
    {
        LiveVideoCall,
        AtLocation,
        AmazonDesignatedSite
    }

}