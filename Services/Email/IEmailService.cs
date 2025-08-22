using Amazon_eCommerce_API.Models.DTO_s;

namespace Amazon_eCommerce_API.Services.Email
{
    public interface IEmailService
    {




        Task<bool> SendOtpEmailAsync(string email, string ptp);

        Task<bool> SendEmailVerificationAsync(UserVerifyEmailDto dto);

        Task<bool> VerifyEmailOtpAsync(UserVerifyEmailDto dto);

        Task<bool> ResendEmailVerificationOtpAsync(string email);

        string GetEmailTemplate(string verificationCode);




    }
}
