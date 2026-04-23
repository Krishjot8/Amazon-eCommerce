using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller.TaxProfile
{
    public class TaxAddress : BaseModel
    {
        public int SellerTaxProfileId { get; set; }
        
        public AddressType AddressType { get; set; }
        
      
        public string CountryCode { get; set; } = null!;

        public string AddressLine1 { get; set; } = null!;

        public string? AddressLine2 { get; set; }
        
        public string City { get; set; } = null!;

        public string State { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public SellerTaxProfile SellerTaxProfile { get; set; } = null!;

    }

    public enum AddressType
    {
        
        Permanent,
        Mailing
    }
}