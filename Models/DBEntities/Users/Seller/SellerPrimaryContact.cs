using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerPrimaryContact
    {
        public int SellerUserId { get; set; }
        
        
        public string FirstName { get; set; }
        public string MiddleName { get; set; } = null;
        public string LastName { get; set; }

        
        
        public string CountryOfCitizenship { get; set; }
        public string CountryOfBirth { get; set; }
        
        
        public DateOnly DateOfBirth { get; set; }
        
        public IdentityProof IdentityProof { get; set; }
        
        public string IdentityProofNumber { get; set; }
        public DateTime IdentityProofExpirationDate { get; set; }
        public string CountryOfIssue {get; set;}
        
        
        // Primary Address
        
        public string AddressLine1 { get; set; }
        
        public string AddressLine2 { get; set; } = null;
        
        public string City { get; set; }
        
        public string State { get; set; }
        
        public string ZipCode { get; set; }
        
        public string Country {get; set;}
        
        
        
        public string PersonalPhoneNumber { get; set; }
        
        
        
        public bool IsBusinessOwner { get; set; }
        
        public bool IsLegalRepresentative { get; set; }

        public bool ConfirmedActingOnBehalfOfBusiness { get; set; }
        
        
        
        public SellerUser SellerUser { get; set; }
        
    }
}