using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.Users
{
    public class UserRegistrationDto
    {

        [Required(ErrorMessage = "Your First Name is required")]
        [RegularExpression(@"^[A-Za-z' ]+([- ][A-Za-z' ]+)*( (IV|V|VI|VII|VIII|IX|X|XI|XII))?$",
        ErrorMessage = "Invalid first name format. Please use letters, hyphens, apostrophes, and spaces only for First Name.")]

        public string FirstName { get; set; }


        [Required(ErrorMessage = "Your Last Name is required")]
        [RegularExpression(@"^[A-Za-z' ]+([- ][A-Za-z' ]+)*( (IV|V|VI|VII|VIII|IX|X|XI|XII))?$",
        ErrorMessage = "Invalid first name format. Please use letters, hyphens, apostrophes, and spaces only for Last Name.")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Your Date of Birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])/\d{4}$", ErrorMessage = "Invalid format. Please use MM/DD/YYYY for Date of Birth.")]

        public DateTime DateofBirth { get; set; }


        [Required(ErrorMessage = "Your Username is Required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username Must be between 5 and 20 characters.")]
        public string UserName { get; set; }



        [Required(ErrorMessage = "Your Email Address is Required")]
        [EmailAddress(ErrorMessage = "Invailid email format")]
        public string Email { get; set; }


      
        [Phone(ErrorMessage = "Invalid phone number format")]
        [RegularExpression(@"^\+?[1-9]\d{0,14}$",
        ErrorMessage = "Invalid phone number format. Please enter a valid international phone number.")]
        public string? PhoneNumber { get; set; }


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
