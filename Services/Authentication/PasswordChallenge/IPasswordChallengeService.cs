using Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge.ForgotPassword;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Models.Enums;

namespace Amazon_eCommerce_API.Services.Authentication.PasswordChallenge
{
    public interface IPasswordChallengeService
    {

        Task<PasswordChallengeResponseDto>GenerateOtpChallengeAsync
            (string identifier,
                string password,
                UserRole role,
                OtpPurpose otpPurpose = OtpPurpose.SignIn,
                string deviceDetails = "generic web browser",
                string locationDetails = "United States",
                string denyLink = "https://www.amazon.com");
        
        Task<bool> VerifyOtpAsync(PasswordChallengeVerifyDto verifyDto);
        
        Task<ResendOtpResponseDto> ResendOtpAsync(ResendOtpRequestDto request);
        // password reset methods
        
        Task GeneratePasswordResetOtpAsync(string identifier, AccountType accountType);
        Task<bool> VerifyPasswordResetOtpAsync(string pendingAuthId, string otp, AccountType accountType);
        
        Task<bool> ResetForgotPasswordAsync(ResetForgotPasswordDto request);

    }
}

