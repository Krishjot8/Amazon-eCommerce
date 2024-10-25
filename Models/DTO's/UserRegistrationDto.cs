using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.Users
{
    public class UserRegistrationDto          //Angular User Registration Entity
    {

        [Required(ErrorMessage = "Your First Name is required")]
        [RegularExpression(@"^[A-Za-z' ]+([- ][A-Za-z' ]+)*( (IV|V|VI|VII|VIII|IX|X|XI|XII))?$",        
        ErrorMessage = "Invalid first name format. Please use letters, hyphens, apostrophes, and spaces only for First Name.")]
        [StringLength(45)]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Your Last Name is required")]
        [RegularExpression(@"^[A-Za-z' ]+([- ][A-Za-z' ]+)*( (IV|V|VI|VII|VIII|IX|X|XI|XII))?$",
        ErrorMessage = "Invalid first name format. Please use letters, hyphens, apostrophes, and spaces only for Last Name.")]
         [StringLength(45)]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Your Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateOnly DateofBirth { get; set; }


        [Required(ErrorMessage = "Your Username is Required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username Must be between 5 and 20 characters.")]
        public string UserName { get; set; }



        [Required(ErrorMessage = "Your Email Address is Required")]
        [EmailAddress(ErrorMessage = "Invailid email format")]
        public string Email { get; set; }


        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^(\+?\d{1,3}[-. ]?)?(\(?\d{1,4}\)?[-. ]?)?(\d{1,4}[-. ]?)?(\d{1,4}[-. ]?)?(\d{1,9})$",
      ErrorMessage = "Invalid phone number format. Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Your Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Your need to re-enter your password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        public bool SubscribeToNewsLetter { get; set; }



     




    }
}
