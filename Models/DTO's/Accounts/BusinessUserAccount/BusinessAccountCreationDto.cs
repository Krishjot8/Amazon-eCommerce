using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.BusinessAccount
{
    public class BusinessAccountCreationDto       
    {
       
        
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    

    }
}
