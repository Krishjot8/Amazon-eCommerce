using Amazon_eCommerce_API.Models.DTO_s;

namespace Amazon_eCommerce_API.Services.Email
{
    public interface IEmailService
    {




        Task<bool> SendOtpEmailAsync(string email, string otp); // Send a one-time password (OTP) to the user via email.

        Task<bool> SendEmailVerificationAsync(UserVerifyEmailDto dto); //Initiates the email verification process for a user.

        Task<bool> VerifyEmailOtpAsync(UserVerifyEmailDto dto); // Checks if the OTP entered by the user matches the one stored in cache or database.

        Task<bool> ResendEmailVerificationOtpAsync(string email); // Sends a new verification OTP to the user if they request it.

        string GetEmailTemplate(string verificationCode); //Generates the HTML or text content of the email, including the OTP.




    }
}
