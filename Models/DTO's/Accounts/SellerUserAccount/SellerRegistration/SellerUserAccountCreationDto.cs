using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration
{
    public class SellerUserAccountCreationDto
    {


        [Required]

        public string Name { get; set; }

        [Required]
        [EmailAddress]

        public string Email { get; set; }

        [Required]

        public string Password { get; set; }

        [Required]


        public string ConfirmPassword { get; set; }




    }
}
