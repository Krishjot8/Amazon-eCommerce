using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s
{
    public class UserPasswordUpdateDto          
    {


        [Required(ErrorMessage = "Current Password is required")]
        public string CurrentPassword { get; set; }


        [Required(ErrorMessage = "New Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "New password must be at least 6 characters long")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one number, and one special character.")]

        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Please re-enter your new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]

        public string ConfirmNewPassword { get; set; }
    }
}
