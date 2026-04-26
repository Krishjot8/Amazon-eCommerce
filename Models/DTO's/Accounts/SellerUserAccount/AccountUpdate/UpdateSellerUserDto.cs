using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountUpdate;


    public class UpdateSellerUserDto            //Angular User Update Entity without Password after logging in
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
        


        [Required(ErrorMessage = "Your Email Address is Required")]
        [EmailAddress(ErrorMessage = "Invailid email format")]

        public string Email { get; set; }
        

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^(\+?\d{1,3}[-. ]?)?(\(?\d{1,4}\)?[-. ]?)?(\d{1,4}[-. ]?)?(\d{1,4}[-. ]?)?(\d{1,9})$",
   ErrorMessage = "Invalid phone number format. Please enter a valid phone number.")]
        public string PhoneNumber { get; set; }


 

      

    }

