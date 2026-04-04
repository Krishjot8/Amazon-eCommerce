namespace Amazon_eCommerce_API.Models.CacheStates.Authentication
{
    public class BusinessRegistrationState
    {

        //Identity

        public string Email { get; set; } = string.Empty;


        public bool IsEmailVerified { get; set; } = false;


        // Account Setup
        public string? FullName { get; set; }


        public string? PasswordHash { get; set; }


        //Business Details
        public BusinessDetailsState? BusinessDetails { get; set; } 


        // Tracking

        public RegistrationStep CurrentStep { get; set; }  = RegistrationStep.Step1_Identity;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;




    }

    public enum RegistrationStep
    {
        Step1_Identity = 1,
        Step2_AccountSetup = 2,
        Step3_BusinessDetails = 3,
        Completed = 4
    }
}
