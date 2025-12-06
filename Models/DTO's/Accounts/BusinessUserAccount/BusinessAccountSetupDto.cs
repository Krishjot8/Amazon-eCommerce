using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.BusinessAccount
{
    public class BusinessAccountSetupDto       //Angular User Entity
    {

        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
     


    }
}
