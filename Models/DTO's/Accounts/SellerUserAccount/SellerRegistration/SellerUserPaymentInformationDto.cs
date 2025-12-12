using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration
{
    public class SellerUserPaymentInformationDto
    {

        [Required]
        public string CardNumber { get; set; }

    }
}
