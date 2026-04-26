using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.TaxProfile;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerW9 : BaseModel
    {
        public int SellerUserId { get; set; }
        
        public string LegalName { get; set; } = null!;

        public string? BusinessName { get; set; }
        
        public BusinessFederalTaxClassification? BusinessFederalTaxClassification { get; set; }
        
        public LLCFederalTaxCode? LLCFederalTaxCode { get; set; }
        
        
        public bool HasForeignPartnersOrOwners {get; set;}
        
        public string? ExemptPayeeCode { get; set; }
        
        public string? FATCAExemptionCode { get; set; }
        
        
        public string AddressCountryCode { get; set; } = null!;

        public string AddressLine1 { get; set; } = null!;

        public string? AddressLine2 { get; set; }
        
        public string AddressCity { get; set; } = null!;
        
        public string AddressState { get; set; } = null!;

        public string AddressZipCode { get; set; } = null!;

        public string? RequesterNameAddress {get; set;}
        public string? AccountNumber {get; set;}
        
        public TaxIdentificationType TaxpayerIdentificationType { get; set; }
        
        public string TaxpayerIdentificationNumber { get; set; } = null!;

        public string SignedBy { get; set; } = null!;
        public DateTime SignedDate { get; set; }
        public bool IsElectronicallySigned { get; set; }
        
        public SellerUser SellerUser { get; set; } = null!;



    }
}