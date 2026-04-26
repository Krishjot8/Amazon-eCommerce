using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.SellerOnboarding
{
    public class SellerUserBusinessProfileDto  //Step2
    {
        [Required]

        public string BusinessName { get; set; }

        [Required]
        [EnumDataType(typeof(BusinessType), ErrorMessage = "Invalid business type.")]
      
        
        public BusinessType BusinessType { get; set; }
        
        
        [Required]
        public string Country { get; set; }



        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the terms.")]
        public bool AgreeToTerms { get; set; }


    }

    public enum BusinessType
    {
        StateOwnedBusiness,
        PrivatelyOwnedBusiness,
        Charity,
        Individual
        
        
    }
}
