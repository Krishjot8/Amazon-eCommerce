using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration.TaxProfile
{
    public class SellerUserTaxProfileDto
    {
        [Required] 
        public TaxClassification TaxClassification { get; set; }
        [Required]
        [StringLength(2)]
        public string TaxCountryCode { get; set; }
        
        [Required]
        public TaxIdentificationDto TaxIdentification { get; set; }
        
        [Required]
        public AddressDto PermanentAddress { get; set; }
        
        public AddressDto? MailingAddress { get; set; }
        
        public IndividualTaxDetailsDto? Individual { get; set; }
        
        public BusinessTaxDetailsDto? Business { get; set; }
    }

    public enum TaxClassification
    {
        Individual,
        Business
        
    }
    
}