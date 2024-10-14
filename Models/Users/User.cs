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

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }




    }
}
