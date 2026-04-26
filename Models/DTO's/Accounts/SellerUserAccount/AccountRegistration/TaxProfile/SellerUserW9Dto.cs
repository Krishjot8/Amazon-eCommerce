using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.TaxProfile
{
    public class SellerUserW9Dto
    {
        [Required]
        [StringLength(100,ErrorMessage ="Legal name cannot exceed 100 characters.")]
        public string LegalName { get; set; }
        
        [Required]
        [StringLength(100,ErrorMessage ="Business name cannot exceed 100 characters.")]
        public string? BusinessName { get; set; }
        
        [EnumDataType(typeof(BusinessFederalTaxClassification),ErrorMessage = "Invalid federal tax classification")]
        public BusinessFederalTaxClassification ? BusinessFederalTaxClassification { get; set; }
        
        [EnumDataType(typeof(LLCFederalTaxCode),ErrorMessage = "Invalid LLC federal tax code.")]
        public LLCFederalTaxCode? LLCFederalTaxCode { get; set; }
        
        //Line 3B
        public bool? HasForeignPartnersOrOwners { get; set; }
        
        [StringLength(10,ErrorMessage ="Exempt payee code cannot exceed 10 characters.")]
        public string? ExemptPayeeCode { get; set; }
        
        [StringLength(10,ErrorMessage ="FATCA exemption code cannot exceed 10 characters.")]
        public string? FATCAExemptionCode { get; set; }
        
        [Required]
        public AddressDto Address { get; set; }
        
        [StringLength(200,ErrorMessage ="Requester name/address cannot exceed 200 characters.")]
        public string? RequesterNameAddress {get; set;}
        [StringLength(50,ErrorMessage ="Account numbers cannot exceed 50 characters.")]
        public string? AccountNumbers {get; set;}
        
        [EnumDataType(typeof(TaxIdentificationType),ErrorMessage = "Invalid TIN Type.")]
        public TaxIdentificationType? TINType { get; set; }
        
        [Required]
        [RegularExpression(@"^\d{9}$",ErrorMessage = "TIN must be exactly 9 digits.")]
        public string TIN { get; set; }
        
        [Required]
        [StringLength(100,ErrorMessage ="Signee cannot exceed 100 characters.")]
        public string SignedBy { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime SignedDate { get; set; }

        public bool IsElectronicallySigned { get; set; }
    }

    public enum LLCFederalTaxCode
    {
        C,
        S,
        P
        
    }
}