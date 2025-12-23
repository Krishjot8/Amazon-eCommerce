using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration
{

 

    public class SellerUserBusinessInformationDto
    {

        [Required]
        public string BusinessName { get; set; }  //Prefilled
        [Required]
        [Phone]
        public string BusinessPhoneNumber { get; set; } //Preselected

        [Required]
        public string CompanyRegistrationNumber { get; set; }

        [Required]
        public string Country {  get; set; }  //Prefilled
        
        [Required]
        public string AddressLine1 { get; set; }
        
        public string? AddressLine2 { get; set; }
        
        [Required]
        public string City { get; set; }
        
        [Required] 
        public string State { get; set; }
        
        [Required]
        public string ZipCode { get; set; }


        [Required]
        public PinDeliveryMethod PinDeliveryMethod { get; set; }


        [Required] 
        public string VerificationLanguage { get; set; }


        [Required]
        public string CaptchaInput { get; set; }

        
    }


    public enum PinDeliveryMethod
    {
        Sms = 1,
        Call = 2
    }

}
