using Amazon_eCommerce_API.Models.DTO_s.Authentication.Verification;

namespace Amazon_eCommerce_API.Services.Communication.Email
{
    public interface IEmailService
    {

        
        Task<bool> SendOtpEmailAsync(string email,string otp,string providerName = null); // Send a one-time password (OTP) to the user via email.
        
        Task<bool> SendPasswordResetOtpEmailAsync(
            string email,
            string otp,
            string timestamp,
            string userName = "Customer",
            string deviceDetails = "generic web browser",
            string locationDetails = "United States",
            string providerName = null);
        Task<bool> VerifyEmailOtpAsync(VerifyEmailDto dto); // Checks if the OTP entered by the user matches the one stored in cache or database.

        Task<bool> ResendEmailVerificationOtpAsync(string email, AccountType accountType); // Sends a new verification OTP to the user if they request it.

        string GetEmailTemplate(string verificationCode); //Generates the HTML or text content of the email, including the OTP.




    }
}
