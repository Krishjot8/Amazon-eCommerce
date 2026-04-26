using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.SellerOnboarding
{
    public class SellerUserPayoutSetupDto
    {

        [Required] [StringLength(100)] 
        
        public string BankAccountHolderName { get; set; } = null!;
        
        [Required]
        [StringLength(100)]
        public string BankName { get; set; } = null!;
        
        
        [Required] 
        [EnumDataType(typeof(BankAccountType))]
        public BankAccountType BankAccountType { get; set; }
        
        [Required]
        [StringLength(20)]
        public string RoutingNumber { get; set; } = null!;
        
        [Required]
        [StringLength(30)]
        public string AccountNumber { get; set; } = null!;
        
        
        [StringLength(11)]
        
        public string? SwiftBic { get; set; }
        
        [Required]
        [StringLength(2)]
        public string Country { get; set; } = null!;
        
        [Required]
        [StringLength(3)]
        public string Currency { get; set; } = null!;
       
        public bool IsDefaultMethod { get; set; }
        
        [StringLength(50)]
        public string? AccountNickname { get; set; }
    }
    
    public enum BankAccountType
    {
        Checking = 0,
        Savings = 1,
    }
}