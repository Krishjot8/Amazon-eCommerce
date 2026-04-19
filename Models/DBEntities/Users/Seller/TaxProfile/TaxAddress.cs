using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller.TaxProfile
{
    public class TaxAddress : BaseModel
    {
        public int SellerTaxProfileId { get; set; }
        
        public AddressType AddressType { get; set; }
        
      
        public string CountryCode { get; set; }
        
        public string AddressLine1 { get; set; }
       
        public string? AddressLine2 { get; set; }
        
        public string City { get; set; }
      
        public string State { get; set; }
        
        public string ZipCode { get; set; }
        
        public SellerTaxProfile SellerTaxProfile { get; set; }
        
    }

    public enum AddressType
    {
        
        Permanent,
        Mailing
    }
}