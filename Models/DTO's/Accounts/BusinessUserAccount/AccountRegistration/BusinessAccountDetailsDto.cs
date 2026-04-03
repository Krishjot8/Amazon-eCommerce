using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountRegistration
{
    public class BusinessAccountDetailsDto
    {

     
        [Required]
        public string BusinessPhoneNumber { get; set; }


        public bool ReceiveUpdates { get; set; }

        [Required]
        public string BusinessName { get; set; }


        [Required]
        public string BusinessType { get; set; }


        [Required]
        public string StreetAddress { get; set; }


        public string?  SuiteUnitFloor { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }
       

    }
}

