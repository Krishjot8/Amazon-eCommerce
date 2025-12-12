using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.SellerAccount;

public class SellerUserDto        //Angular User Entity
{
   [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string UserName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]

    public DateOnly DateofBirth { get; set; }
    
    
    [Required]
    [Phone]
    public string? PhoneNumber { get; set; }

  

}
