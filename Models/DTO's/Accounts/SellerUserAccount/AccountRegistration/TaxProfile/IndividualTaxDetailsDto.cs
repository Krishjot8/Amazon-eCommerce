using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.TaxProfile
{
    public class IndividualTaxDetailsDto
    {
        [Required]
        [StringLength(200, MinimumLength = 2)]
        public string FullName { get; set; }
        
        
        [Required]
        public bool? IsUSPerson { get; set; }

        
        public string? CountryOfCitizenship { get; set; } //If Individual Non US Resident

     
        
        public bool IsReceivingPaymentsOnBehalfOfAnother {get; set;} //If Business Non-US Resident Or Individual Classification

    }
}