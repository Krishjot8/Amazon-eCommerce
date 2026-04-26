using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountVerification.Address
{
    public class VerifyAddressCodeDto
    {
        [Required]
        
        public int SellerUserId { get; set; }
        
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Code must be 6 digits long")]
        public string VerificationCode { get; set; }
    }
}