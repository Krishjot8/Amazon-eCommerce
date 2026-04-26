using System.ComponentModel.DataAnnotations;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.SellerOnboarding;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountVerification.Documents
{
    public class SellerUserVerificationStatusDto
 
    {
        // --- Identity Section ---
        [Required]
        [EnumDataType(typeof(IdentityDocumentType), ErrorMessage = "Invalid document type.")]
        public IdentityDocumentType DocumentType { get; set; }

        [Required]
        [StringLength(500)]
        [Url(ErrorMessage = "Document front URL must be a valid URL.")]
        public string DocumentFrontUrl { get; set; } = null!;

        [StringLength(500)]
        [Url(ErrorMessage = "Document back URL must be a valid URL.")]
        public string? DocumentBackUrl { get; set; } // Nullable because Passports don't have a back

        // --- Business Verification ---
        [Required]
        [Url(ErrorMessage = "Registration extract URL must be a valid URL.")] // Fixed error message
        public string RegistrationExtractUrl { get; set; } = null!;

        [Required]
        [EnumDataType(typeof(ProofOfAddressType), ErrorMessage = "Invalid business proof type.")]
        public ProofOfAddressType BusinessAddressProofType { get; set; }

        [Required]
        [StringLength(500)]
        [Url(ErrorMessage = "Business proof document URL must be a valid URL.")]
        public string BusinessAddressProofUrl { get; set; } = null!;

        // --- Primary Contact Address Verification ---
        [Required]
        [EnumDataType(typeof(ProofOfAddressType), ErrorMessage = "Invalid residential proof type.")]
        public ProofOfAddressType ResidentialAddressProofType { get; set; }

        [Required]
        [StringLength(500)]
        [Url(ErrorMessage = "Residential proof document URL must be a valid URL.")]
        public string ResidentialAddressProofUrl { get; set; } = null!;

        // --- Authorization ---
        [Required]
        [Url(ErrorMessage = "Letter of authorization URL must be a valid URL.")]
        public string LetterOfAuthorizationUrl { get; set; } = null!;
    }
    
   

}