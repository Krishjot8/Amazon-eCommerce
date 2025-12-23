using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration
{
    public class SellerUserPaymentInformationDto
    {

        
        [Required]
        public string CardHolderName { get; set; }
        [Required]
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; }
        
        [Required]
        [Range(1,12)]
        public int ExpirationMonth { get; set; }
        
        [Required]
        [Range(2025,2100)]
        public int ExpirationYear { get; set; }
        
        [Required]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "Invalid CVV")]
        public string SecurityCode { get; set; }

        [Required]
        public string BillingAddressLine1 { get; set; }
       
        public string? BillingAddressLine2 { get; set; }
        
        [Required]
        public string BillingCity { get; set; }

        [Required]
        public string BillingState{ get; set; }
        
        [Required]
        public string BillingZipCode{ get; set; }
        
        [Required]
        public string BillingCountry{ get; set; }
        
    }
}
