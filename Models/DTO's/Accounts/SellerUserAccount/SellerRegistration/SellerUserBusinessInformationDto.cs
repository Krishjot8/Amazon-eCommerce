using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration
{

 

    public class SellerUserBusinessInformationDto   //Step3
    {

        [Required]
        public string BusinessName { get; set; }  //Prefilled
        
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
        public ProofOfAddressType ProofOfAddress { get; set; }
        
        [Required]
        public string ProofOfAddressDocumentUrl { get; set; } 
        [Required]
        
        public string? RegistrationExtractUrl { get; set; }
        
        [Required] 
        public string State { get; set; }
        
        [Required]
        public string ZipCode { get; set; }


        
        [Required]
        public PinDeliveryMethod PinDeliveryMethod { get; set; }

        [Required]
        [Phone]
        public string VerificationPhoneNumber { get; set; } 
        [Required] 
        public string VerificationLanguage { get; set; }


        [Required]
        public string CaptchaInput { get; set; }

        
    }

    public enum ProofOfAddressType
    {
        BankAccountStatement,
        ElectricityBill,
        GasBill,
        InternetBill,
        RentBill,
        TelephoneBill,
        TVBill,
        WaterBill
    }

    public enum PinDeliveryMethod
    {
        Sms = 1,
        Call = 2
    }

}
