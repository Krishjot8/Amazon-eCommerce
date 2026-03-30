using Amazon_eCommerce_API.Models.DTO_s.Authentication;

namespace Amazon_eCommerce_API.Services.Communication.Sms;

public interface ISmsService
{

    Task<bool> SendOtpSmsAsync(string phoneNumber, string otp);   // Sends a one-time password (OTP) to the user via SMS.
    Task<bool> SendSmsVerificationAsync(VerifySmsDto dto);  /// Initiates the SMS verification process for a user.
    Task<bool> VerifySmsOtpAsync(VerifySmsDto dto);
    Task<bool> ResendSmsVerificationOtpAsync(string phoneNumber);
    string GetSmsTemplate(string verificationCode);
}