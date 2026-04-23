using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerBusinessInformation : BaseModel

    {

        public int SellerUserId { get; set; }

        public string BusinessName { get; set; } = null!;

        public string BusinessPhoneNumber { get; set; } = null!;

        public string CompanyRegistrationNumber { get; set; } = null!;


        public string CountryCode { get; set; } = null!;

        public string AddressLine1 { get; set; } = null!;

        public string? AddressLine2 { get; set; }

        public string City { get; set; } = null!;

        public string State { get; set; } = null!;

        public string ZipCode { get; set; } = null!;

        public SellerUser SellerUser { get; set; } = null!;
    }
}
