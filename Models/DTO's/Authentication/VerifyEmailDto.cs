using System.Security.Principal;

namespace Amazon_eCommerce_API.Models.DTO_s.Authentication

{ 
    public class VerifyEmailDto    //Verify Email After registration
    {

        public string Email { get; set; }

        public string EmailOtp { get; set; }

        public bool IsResendRequest { get; set; }


        public AccountType AccountType { get; set; } 

    }



    public enum AccountType
    {
        Customer,
       Business,
      
    }
}
