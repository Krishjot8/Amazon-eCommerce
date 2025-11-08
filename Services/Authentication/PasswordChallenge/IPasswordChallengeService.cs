using Amazon_eCommerce_API.Models.DTO_s.UserAccount;

namespace Amazon_eCommerce_API.Services.Authentication.PasswordChallenge
{
    public interface IPasswordChallengeService
    {

        Task<BusinessUserPasswordChallengeResponseDto> GenerateOtpChallengeAsync(string emailOrPhone, string password);
        Task<bool> VerifyOtpAsync(BusinessUserPasswordChallengeVerifyDto verifyDto);
       

    }
}

