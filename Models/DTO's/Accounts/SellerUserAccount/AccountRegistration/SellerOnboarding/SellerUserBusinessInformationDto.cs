#nullable enable
using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.SellerOnboarding
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
        public string State { get; set; }
        
        
        [Required]
        [DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }


      
        [Required]
        public ProofOfAddressType ProofOfAddress { get; set; }
        
        [Required]
        [Url]
        public string ProofOfAddressDocumentUrl { get; set; } 
     
        [Url]
        public string? RegistrationExtractUrl { get; set; }
        
        
        [Required]
        public PinDeliveryMethod PinDeliveryMethod { get; set; }

        [Required]
        [Phone]
        public string VerificationPhoneNumber { get; set; } 
        [Required] 
        public string VerificationLanguage { get; set; }


        [Required]
        public string CaptchaInput { get; set; }

        [Required]
        public string CaptchaKey { get; set; }

        
    }

    public enum ProofOfAddressType
    {
        // Financial Statements
        BankAccountStatement,
        CreditCardStatement,
        BuildingSocietyStatement,
        MortgageStatement,
        LoanStatement,
        PayoneerStatement, // Very common for Amazon sellers
        HyperWalletStatement,

        // Utility Bills (The most common category)
        ElectricityBill,
        WaterBill,
        GasBill,
        InternetBill,
        TvBill,
        TelephoneBill, // Landline
        MobilePhoneBill,

        // Housing/Rent
        RentBill,
        LeaseAgreement,
        PropertyTaxReceipt,
    
        // Government/Other
        TaxIdentityDocument,
        CouncilTaxBill, // Common in UK/EU regions
        Other
    }

    public enum PinDeliveryMethod
    {
        Sms = 1,
        Call = 2
    }

}
