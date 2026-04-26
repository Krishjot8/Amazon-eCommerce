using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountVerification.Documents
{
    public class SellerVerificationDocumentDto
    {
        public int DocumentId { get; set; }
        
        [Required]
        public VerificationCategory Category { get; set; }
        
        [Required]
        public int DocumentType { get; set; }

        [Required] [Url] [StringLength(500)] public string DocumentUrl { get; set; } = null!;
        
        [Url]
        [StringLength(500)]
        public string? DocumentBackUrl { get; set; }
        
        public DateTime? IssuanceDate { get; set; }
        
        public DateTime? ExpiryDate { get; set; }

        public DocumentStatus Status { get; set; } = DocumentStatus.Pending;
        
        public string? RejectionReason { get; set; }

    }
}