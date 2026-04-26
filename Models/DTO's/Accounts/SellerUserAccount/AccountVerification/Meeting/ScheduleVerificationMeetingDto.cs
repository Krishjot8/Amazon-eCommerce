using System.ComponentModel.DataAnnotations;

namespace Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountVerification.Meeting
{
    public class ScheduleVerificationMeetingDto
    {
        [Required]
        public int SellerUserId { get; set; }

        [Required]
        public VerificationMeetingType MeetingType { get; set; }

        [Required]
        public DateTime ScheduledDateTime { get; set; }
        
        [Required]
        [StringLength(50)]
        public string PreferredLanguage { get; set; } = "English"; // Amazon requirement

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