using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration
{
    public class SellerUserBusinessProfileDto
    {
        [Required]

        public string BusinessName { get; set; }

        [Required]

        public string BusinessType { get; set; }


        [Required]

        public string Country { get; set; }



        [Required]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must agree to the terms.")]
        public bool AgreeToTerms {  get; set; }


    }
}
