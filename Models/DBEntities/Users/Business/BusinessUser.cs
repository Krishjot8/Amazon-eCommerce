using System.ComponentModel.DataAnnotations;
using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessUser : BaseModel
    {

     
        public string FirstName { get; set; }

      
        public string LastName { get; set; }


        public string Email { get; set; }
        

        public string BusinessPhone { get; set; }


        public bool RecieveTextUpdates { get; set; } = false;


      
        public string BusinessName { get; set; }


       
        public string BusinessType { get; set; }

        public string StreetAddress { get; set; }


        public string? SuiteOrUnit { get; set; }

        public string ZipCode { get; set; }


        public string City { get; set; }

     
        public string State { get; set; }


        public string  PasswordHash { get; set; }


        public bool IsEmailVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
