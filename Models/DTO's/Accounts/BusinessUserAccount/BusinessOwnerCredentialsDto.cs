using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.BusinessAccount
{
    public class BusinessOwnerCredentialsDto       //Angular User Entity
    {
       
       
        public string FullName { get; set; }

      
        public string Password { get; set; }


        public string ConfirmPassword { get; set; }
     


    }
}
