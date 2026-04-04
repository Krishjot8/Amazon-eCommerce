using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate
{
    public class UpdateBusinessProfileDto
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

        public DateOnly DateOfBirth { get; set; }

    }
}
