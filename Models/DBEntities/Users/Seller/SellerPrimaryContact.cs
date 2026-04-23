using Amazon_eCommerce_API.Models.BaseEntities;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerPrimaryContact  : BaseModel
    {
        public int SellerUserId { get; set; }
        
        
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; } 
        public string LastName { get; set; } = null!;




        public DateOnly DateOfBirth { get; set; }
        public string CountryOfCitizenship { get; set; } = null!;

        public string CountryOfBirth { get; set; } = null!;
        
        public IdentityDocumentType IdentityDocumentType { get; set; }
        public string IdentityProofNumber { get; set; } = null!;
        public DateTime? IdentityProofExpirationDate { get; set; }
        public string CountryOfIssue {get; set;} = null!;


        // Primary Address

        public string AddressLine1 { get; set; } = null!;

        public string? AddressLine2 { get; set; } 
        
        public string City { get; set; } = null!;

        public string State { get; set; } = null!;
        
        public string ZipCode { get; set; } = null!;
        
        public string CountryCode { get; set;} = null!;
        
        
        
        public string PersonalPhoneNumber { get; set; } = null!;
        
        
        
        public bool IsBusinessOwner { get; set; }
        
        public bool IsLegalRepresentative { get; set; }

        public bool ConfirmedActingOnBehalfOfBusiness { get; set; }
        
        
        
        public SellerUser SellerUser { get; set; } = null!;

    }
}