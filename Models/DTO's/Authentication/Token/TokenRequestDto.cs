using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Authentication.Token
{
    public class TokenRequestDto
    {
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public string Email  { get; set; }
        
        [Required]
        public UserRole Role  { get; set; } // for different user types
        
        public string? DisplayName { get; set; } // for names and business names
        
        public string? StoreName { get; set; }  //For Seller Account
        
        
    }

    public enum UserRole
    {
        Customer,
        Business,
        Seller
        
    }
}