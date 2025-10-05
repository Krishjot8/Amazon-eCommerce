using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace Amazon_eCommerce_API.Models.Users
{
    public class User : BaseModel // To store user credentials to database after registering.
    {
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20,MinimumLength = 5)]
      
        public string Username { get; set; }


        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [Required(ErrorMessage = "Email is required.")]

        public string Email { get; set; }


        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone Number cannot exceed 20 characters.")]
        [RegularExpression(@"^\+?[1-9]\d{0,14}$",
     ErrorMessage = "Invalid phone number format. Please enter a valid international phone number.")]
        public string PhoneNumber { get; set; }


        public string PasswordHash { get; set; }


        public int RoleId { get; set; }


        [Required(ErrorMessage = "Role is required")]   

            public UserRole Role { get; set; }


        public bool SubscribeToNewsLetter { get; set; } = false;


        public bool IsEmailVerified { get; set; } = false;


    }
}
