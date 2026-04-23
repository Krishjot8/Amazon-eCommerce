using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration.TaxProfile;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller.TaxProfile
{
    public class SellerTaxProfile : BaseModel
    {
        
        
        public int SellerUserId { get; set; }
        
        public TaxClassification TaxClassification { get; set; } 
        
        public string TaxCountryCode { get; set; } = null!;
        
        public DateTime SubmittedAt { get; set; }
        
        public TaxProfileStatus Status { get; set; }
        
        public bool? IsUSPerson { get; set; }
        
        public bool? IsUSResidentEntity { get; set; }
        
        public BusinessFederalTaxClassification? BusinessFederalTaxClassification { get; set; }
        
        public LLCType? LLCType { get; set; }
        
        public string FullName {get; set;} = null!;

        public string? DBAName {get; set;} 
        
        public TaxIdentificationType TaxpayerIdentificationType { get; set; }
        
        public string TaxpayerIdentificationNumber { get; set; } = null!;

        public List<TaxAddress> TaxAddresses { get; set; } = new();

        public SellerUser SellerUser { get; set; } = null!;

    }
    
    public enum TaxProfileStatus
    {
        Pending,
        Approved,
        Rejected,
    }
}