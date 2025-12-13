

using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Password;

namespace Amazon_eCommerce_API.Services.Authentication.PasswordChallenge
{
    public interface IPasswordChallengeService
    {

        Task<CustomerUserPasswordChallengeResponseDto>GenerateOtpChallengeAsync(string emailOrPhone, string password);
        Task<bool> VerifyOtpAsync(CustomerUserPasswordChallengeVerifyDto verifyDto);
       

    }
}

