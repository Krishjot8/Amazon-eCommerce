using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.SellerOnboarding
{
    public class SellerUserPrimaryContactInformationDto  //Step 4
    {

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; } = null;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        public string CountryOfCitizenship { get; set; }
        
        [Required]
        [StringLength(100)]
        public string CountryOfBirth { get; set; }

        //DOB
        [Required]
        [Range(1, 31, ErrorMessage = "Birth days must be between 1 and 31.")]
        public int BirthDay { get; set; }
        
        [Required]
        [Range(1, 12, ErrorMessage = "Birth Month must be between 1 and 12.")]
        public int BirthMonth { get; set; }
        
        [Required]
        [Range(1900,2100, ErrorMessage = "Birth Year must be between 1900 and 2100.")]
        public int BirthYear { get; set; }
        
        [Required]
        [EnumDataType(typeof(IdentityDocumentType), ErrorMessage = "Invalid identity document type.")]
        public IdentityDocumentType IdentityDocumentType { get; set; }

        [Required]
        [StringLength(50)]
       public string IdentityProofNumber { get; set; }

        [Required]
       public DateTime IdentityProofExpirationDate { get; set; }
        
        [Required] 
        [StringLength(100)]
        public string CountryOfIssue { get; set; }
        
      
        [Required]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; } = null;
        
        [Required]
        [StringLength(50)]
        public string City { get; set; }
        
        [Required]
        [StringLength(50)]
        public string State { get; set; }
        
        [Required]
        [StringLength(20)]
        public string ZipCode { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Country { get; set; }  //Prefilled

        [Required]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PersonalPhoneNumber { get; set; }
        
        // Business ownership / roles
        
        [Required]
        public bool PointOfContactIsBeneficialOwner {  get; set; }
        
        
        public bool PointOfContactIsLegalRepresentative { get; set; } // If not benificial Owner
        
        [Required]
        public bool PrimaryContactIsBeneficialOwner { get; set; }
        
        [Required]
        public bool ConfirmedActingOnBehalfOfBusiness { get; set; }
        
        
       
    }

    public enum IdentityDocumentType
    {
        
        Passport = 1,
        DriversLicense = 2,
        NationalId = 3,
        ResidencePermit = 4
        
    }
}
