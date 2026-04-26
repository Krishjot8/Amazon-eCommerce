using Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;

namespace Amazon_eCommerce_API.Services.Authentication.PasswordChallenge
{
    public interface IPasswordChallengeService
    {

        Task<PasswordChallengeResponseDto>GenerateOtpChallengeAsync(string identifier, string password, UserRole role);
        Task<bool> VerifyOtpAsync(PasswordChallengeVerifyDto verifyDto);
       

    }
}

