using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration
{
    public class SellerUserPrimaryContactInformationDto
    {


        public string FirstName { get; set; }


        public string MiddleName { get; set; }

        public string LastName { get; set; }


        public string CountryOfCitizenship { get; set; }


        public string CountryOfBirth { get; set; }

        public string DateOfBirth { get; set; }


        public string IdentityDocumentType { get; set; }

       public string IdentityDocumentNumber { get; set; }


        public string CountryOfIssue { get; set; }


       public DateTime IdentityDocumentExpirationDate { get; set; }




        [Required]
        public string Country { get; set; }  //Prefilled


        [Required]

        public string AddressLine1 { get; set; }


        public string? AddressLine2 { get; set; }


        [Required]

        public string City { get; set; }


        [Required]
        public string State { get; set; }


        [Required]

        public string ZipCode { get; set; }

        [Required]
        public string ContactPhone { get; set; }


        [Required]
        public bool PointOfContactIsBusinessOwner {  get; set; }

        public bool PointOfContactIsLegalRepresentative { get; set; } // If not benificial Owner


        public bool PrimaryContactIsBusinessOwner { get; set; }



    }
}
