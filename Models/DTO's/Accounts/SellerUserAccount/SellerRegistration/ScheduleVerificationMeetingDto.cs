using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration
{
    public class ScheduleVerificationMeetingDto
    {
        [Required]
        public int SellerUserId { get; set; }

        [Required]
        public VerificationMeetingType MeetingType { get; set; }

        [Required]
        public DateTime ScheduledDateTime { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
    
    public enum VerificationMeetingType
    {
        LiveVideoCall,
        AtLocation,
        AmazonDesignatedSite
    }
}