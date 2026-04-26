using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.TaxProfile
{
    public class TaxIdentificationDto
    {
        [Required]
        [StringLength(2,MinimumLength = 2)]
        public string CountryCode { get; set; }
        
        [Required]
        public TaxIdentificationType TaxIdentificationType { get; set; } //If Business US Resident

        
        [Required]
        [StringLength(110, MinimumLength = 9)]
        public string TaxIdentificationNumber { get; set; } //If Business US Resident

    }
    
    public enum TaxIdentificationType
    {
        
        SSN,
        EIN,
        ITIN,
        Other
        
    }

}