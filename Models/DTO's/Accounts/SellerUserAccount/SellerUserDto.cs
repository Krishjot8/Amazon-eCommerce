using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.SellerAccount;

public class SellerUserDto        //Angular User Entity
{
   
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

 

    public DateOnly DateofBirth { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

  

}
