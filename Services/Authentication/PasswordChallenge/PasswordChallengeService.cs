using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DTO_s.Cache;
using Amazon_eCommerce_API.Models.DTO_s.UserAccount;
using Amazon_eCommerce_API.Services.Cache;
using Amazon_eCommerce_API.Services.Users;

namespace Amazon_eCommerce_API.Services.Authentication.PasswordChallenge
{
    public class PasswordChallengeService : IPasswordChallengeService    //For Generating One time Password
    {

        private readonly IUserService _userService;

        private readonly ICacheService _cacheService;

        public PasswordChallengeService(IUserService userService, ICacheService cacheService)
        {
            _userService = userService;
            _cacheService = cacheService;
        }

        public async Task<UserPasswordChallengeResponseDto> GenerateOtpChallengeAsync(string identifier, string password)
        {

            var user = await _userService.GetUserByEmailAsync(identifier) ?? await _userService.GetUserByPhoneNumberAsync(identifier);


            if (user == null || !await _userService.VerifyPasswordAsync(password, user.PasswordHash)) {

                //add message Password is Incorrect

                return null; 
            
            }

            var otp = new Random().Next(1000000, 999999).ToString();

            var otpCache = new OtpCacheDto
            {

                Identifier = identifier,
                Otp = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(15),
                Attempts = 0

            };

            await _cacheService.SetOtpAsync(identifier,otpCache,TimeSpan.FromMinutes(5));

        }

        public Task<bool> VerifyOtpAsync(string pendingAuthId, string otp)
        {
            throw new NotImplementedException();
        }
    }
}
