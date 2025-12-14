namespace Amazon_eCommerce_API.Models.DBEntities.Users.Seller
{
    public class SellerPrimaryContact
    {
        public int SellerUserId { get; set; }
        
        public string FirstName { get; set; }

        public string MiddleName { get; set; } = null;
        
        public string LastName { get; set; }
        
        public string IdentityDocumentType { get; set; }
        
        public string IdentityDocumentNumber { get; set; }
        
        public DateTime IdentityDocumentExpirationDate { get; set; }
        
        public string CountryOfIssue {get; set;}
        
        public bool IsBusinessOwner { get; set; }
        
        public bool IsLegalRepresentative { get; set; }
        
        public SellerUser SellerUser { get; set; }
        
    }
}