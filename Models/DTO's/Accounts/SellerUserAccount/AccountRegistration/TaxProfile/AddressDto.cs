using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.TaxProfile
{
    public class AddressDto
    {
         
        [Required]
        [StringLength(100)]
        public string Country { get; set; }
        [Required]
        [StringLength(200)]
        public string AddressLine1 { get; set; }
        [StringLength(200)]
        public string? AddressLine2 { get; set; }
        
        [Required]
        [StringLength(100)]
        public string City { get; set; }
        
        [Required]
        [StringLength(100)]
        public string State { get; set; }
        
        [Required]
        [StringLength(20)]
        public string ZipCode { get; set; }
    }
}