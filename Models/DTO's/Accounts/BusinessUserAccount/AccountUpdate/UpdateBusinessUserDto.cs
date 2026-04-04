using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate
{
    public class UpdateBusinessUserDto
    {


        [Required(ErrorMessage = "Your Email Address is Required")]
        [EmailAddress(ErrorMessage = "Invailid email format")]
        public string BusinessEmail { get; set; }



        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^(\+?\d{1,3}[-. ]?)?(\(?\d{1,4}\)?[-. ]?)?(\d{1,4}[-. ]?)?(\d{1,4}[-. ]?)?(\d{1,9})$",
   ErrorMessage = "Invalid phone number format. Please enter a valid phone number.")]

        public string BusinessPhoneNumber { get; set; }
        
    }
}