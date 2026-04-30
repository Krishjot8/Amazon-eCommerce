using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountRegistration
{
    public class CustomerUserRegistrationDto          //Angular User Registration Entity
    {

        [Required(ErrorMessage = "Your First Name is required")]
        [RegularExpression(@"^[A-Za-z' ]+([- ][A-Za-z' ]+)*( (IV|V|VI|VII|VIII|IX|X|XI|XII))?$",        
        ErrorMessage = "Invalid first name format. Use letters, spaces, apostrophes, hyphens, and optional titles like 'IV' or 'XII'.")]
        [StringLength(45)]
        public string FirstName { get; set; } = null!;


        [Required(ErrorMessage = "Your Last Name is required")]
        [RegularExpression(@"^[A-Za-z' ]+([- ][A-Za-z' ]+)*( (IV|V|VI|VII|VIII|IX|X|XI|XII))?$",
        ErrorMessage = "Invalid last name format. Use letters, spaces, apostrophes, hyphens, and optional titles like 'IV' or 'XII'.")]
         [StringLength(45)]
        public string LastName { get; set; } = null!;



        [Required(ErrorMessage = "Your Email Address is Required")]
        [EmailAddress(ErrorMessage = "Invailid email format")]
        public string Email { get; set; } = null!;


        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^(\+?\d{1,3}[-. ]?)?(\(?\d{1,4}\)?[-. ]?)?(\d{1,4}[-. ]?)?(\d{1,4}[-. ]?)?(\d{1,9})$",
      ErrorMessage = "Invalid phone number format.")]
        public string? MobileNumber { get; set; }


        [Required(ErrorMessage = "Your password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]

        public string Password { get; set; } = null!;
        

        [Required(ErrorMessage = "please re-enter your password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;
        
        public bool SubscribeToNewsLetter { get; set; }



     




    }
}
