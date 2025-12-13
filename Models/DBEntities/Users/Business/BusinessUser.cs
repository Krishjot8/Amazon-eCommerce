using System.ComponentModel.DataAnnotations;
using Amazon_eCommerce_API.Models.BaseEntities;

namespace Amazon_eCommerce_API.Models.DBEntities.Users.Business
{
    public class BusinessUser : BaseModel
    {

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }
        

        [Phone]
        [StringLength(20)]
        [Required]
        public string BusinessPhone { get; set; }


        public bool RecieveTextUpdates { get; set; } = false;


        [Required]
        [StringLength(100)]
        public string BusinessName { get; set; }


        [Required]
        [StringLength(50)]

        public string BusinessType { get; set; }

        [Required]
        [StringLength(100)]
        public string StreetAddress { get; set; }


        [StringLength(20)]
        public string? SuiteOrUnit { get; set; }


        [Required]
        [StringLength(20)]
        public string ZipCode { get; set; }


        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }


        [Required]
        public string  PasswordHash { get; set; }


        public bool IsEmailVerified { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
