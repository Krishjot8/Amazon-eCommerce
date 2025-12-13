using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Registration
{
    public class BusinessAccountEmailDto       
    {
       
        
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    

    }
}
