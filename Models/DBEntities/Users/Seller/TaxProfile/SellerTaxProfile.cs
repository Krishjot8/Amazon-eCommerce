using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration.TaxProfile;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller.TaxProfile
{
    public class SellerTaxProfile : BaseModel
    {
        
        
        public int SellerUserId { get; set; }
        
        public TaxClassification TaxClassification { get; set; }
        
        public string TaxCountryCode { get; set; }
        
        public DateTime SubmittedAt { get; set; }
        
        public Status Status { get; set; }
        
        public bool? IsUSPerson { get; set; }
        
        public bool? IsUSResidentEntity { get; set; }
        
        public BusinessFederalTaxClassification? BusinessFederalTaxClassification { get; set; }
        
        public LLCType? LLCType { get; set; }
        
        public string FullName {get; set;}
        
        public string? DBAName {get; set;}
        
        public TaxIdentificationType TaxpayerIdentificationType { get; set; }
        
        public string TaxpayerIdentificationNumber { get; set; }
        
        public List<TaxAddress> TaxAddresses { get; set; }
        
        public SellerUser SellerUser { get; set; }
      
    }
    
    public enum Status
    {
        Pending,
        Approved,
        Rejected,
    }
}