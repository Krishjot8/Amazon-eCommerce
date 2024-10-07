using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.Users
{
    public class UserRegistration
    {

        [Required(ErrorMessage = "Your First Name is required")]



        public string FirstName { get; set; }


        [Required(ErrorMessage = "Your Last Name is required")]

        public string LastName { get; set; }


        [Required(ErrorMessage = "Your Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateofBirth { get; set; }

        [Required(ErrorMessage = "Your Email Address is Required")]
        [EmailAddress(ErrorMessage = "Invailid email format")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Your Username is Required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Username Must be between 3 and 20 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Your Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Your need to re-enter your password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool SubscribeToNewsLetter { get; set; }



     




    }
}
