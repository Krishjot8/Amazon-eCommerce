using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration
{
    public class SellerUserTaxProfileDto
    {
        [Required]
        public TaxClassification TaxClassification {get; set;}
        [Required]
        public TaxResidencyStatus TaxResidencyStatus {get; set;}


        public bool? IsUSResidentEntity { get; set; } 
        
        [Required]
        public BusinessFederalTaxClassification BusinessFederalTaxClassification {get; set;}

        public LLCType? LLCType { get; set; } 

        [Required]
        public string FullName { get; set; }
      
        public string? DBAName { get; set; }
        
        [Required]
        public TaxpayerIdentificationType TaxpayerIdentificationType { get; set; }

        [Required]
        public string TaxpayerIdentificationNumber { get; set; }
        
        //Address
        public AddressDto Address { get; set; }
        
    }


    public class AddressDto
    {
        
        [Required]
        public string Country { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        
        public string? AddressLine2 { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required]
        public string State { get; set; }
        
        [Required]
        public string ZipCode { get; set; }
    }

    public enum TaxClassification
    {
        Individual,
        Business
        
    }

    public enum TaxResidencyStatus
    {
        Yes,
        No,
        NotSure
        
    }

    public enum BusinessFederalTaxClassification
    {
        [Display(Name = "C Corporation")]
        C_Corporation,
        [Display(Name = "S Corporation")]
        S_Corporation,
        Partnership,
        [Display(Name = "Trust / Estate")]
        TrustOrEstate,
        [Display(Name = "Limited Liability Company")]
        LimitedLiabilityCompany,
        Other
        
    }


    public enum LLCType
    {
        
        [Display(Name = "S Corporation")]
        S_Corporation,
        
        [Display(Name = "C Corporation")]
        C_Corporation,
       
        Partnership
    }

    public enum TaxpayerIdentificationType
    {
        
        SocialSecurityNumber,
        EmployerIdentificationNumber,
        IndividualTaxpayerIdentificationNumber,
        Other
        
    }
}