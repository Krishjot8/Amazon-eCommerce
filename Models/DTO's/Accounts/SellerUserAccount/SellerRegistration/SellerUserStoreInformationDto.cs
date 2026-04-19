using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration
{
    public class SellerUserStoreInformationDto //Step 6
    {
        
        [Required]
        public string StoreName { get; set; }
        
        [Required]
        public bool HasUPCsForAllProducts { get; set; }
        
        [Required]
        public bool HasDiversityCertifications { get; set; }
        
        [Required]
        public BrandOwnershipStatus BrandOwnership { get; set; }
        [Required]
        
        public  TrademarkStatus TrademarkStatus { get; set; }
        
    }

    public enum BrandOwnershipStatus
    {
        Yes,
        No,
        Some
    }

    public enum TrademarkStatus
    {
        Yes,
        No,
        Some
        
    }

}