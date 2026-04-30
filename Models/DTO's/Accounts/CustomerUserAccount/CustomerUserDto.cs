using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount
{
    public class CustomerUserDto        
    {
        
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;
        
        public string LastName { get; set; }  = null!;
        
        public string Email { get; set; }  = null!;
        
        public string? PhoneNumber { get; set; } 
        
        public bool IsEmailVerified { get; set; }

        public bool SubscribeToNewsLetter { get; set; }
       

    }
}
