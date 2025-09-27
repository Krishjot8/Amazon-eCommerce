using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DTO_s.Cache;
using Amazon_eCommerce_API.Models.DTO_s.UserAccount;
using Amazon_eCommerce_API.Services.Cache;
using Amazon_eCommerce_API.Services.Communication.Sms;
using Amazon_eCommerce_API.Services.Email;
using Amazon_eCommerce_API.Services.Users;
using System.Security.Cryptography;

namespace Amazon_eCommerce_API.Services.Authentication.PasswordChallenge
{
    public class PasswordChallengeService : IPasswordChallengeService    //For Generating One time Password
    {

        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;
        private readonly IEmailService  _emailService;
        private readonly ISmsService  _smsService;

        public PasswordChallengeService(IUserService userService, ICacheService cacheService, IEmailService emailService, ISmsService smsService)
        {
            _userService = userService;
            _cacheService = cacheService;
            _emailService = emailService;
            _smsService = smsService;
        }

        public async Task<UserPasswordChallengeResponseDto> GenerateOtpChallengeAsync(string identifier,
            string password)
        {

            var user = await _userService.GetUserByEmailAsync(identifier) ??
                       await _userService.GetUserByPhoneNumberAsync(identifier);


            if (user == null || !await _userService.VerifyPasswordAsync(password, user.PasswordHash))
            {

                //add message Password is Incorrect

                return null;

            }

            var otp =  RandomNumberGenerator.GetInt32(100000, 1000000).ToString();



            var otpCache = new OtpCacheDto
            {

                Identifier = identifier,
                Otp = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                Attempts = 0

            };

            await _cacheService.SetOtpAsync(identifier, otpCache);


            string otpChannel;
            string maskedDestination;

            if (IsValidEmail(identifier))
            {
                otpChannel = "email";
                maskedDestination = MaskEmail(identifier);
                await _emailService.SendOtpEmailAsync(identifier, otp);
            }
            else
            {
                otpChannel = "sms";
                maskedDestination = MaskPhone(identifier);
                await _smsService.SendOtpSmsAsync(identifier, otp);
            }

            //return DTO

            return new UserPasswordChallengeResponseDto
            {

                PendingAuthId = identifier,
                OtpChannel = otpChannel,
                MaskedDestination = maskedDestination

            };

        }

        public async Task<bool> VerifyOtpAsync(string pendingAuthId, string otp)
        {
           var cachedOtp =  _cacheService.ValidateOtpAsync(pendingAuthId, otp);

           if (cachedOtp == null) return false;

           await _cacheService.RemoveOtpAsync(pendingAuthId);
           return true;

        }

        private bool IsValidEmail(string email)
        {
            try
            {
               var addr = new System.Net.Mail.MailAddress(email);
               return addr.Address == email;

            }
            catch
            {

                return false;

            }
        }

        private string MaskEmail(string email)
        {
            var parts = email.Split('@');
            if (parts[0].Length <= 2) return $"**@{parts[1]}";
            return $"{parts[0][0]}***{parts[0].Last()}@{parts[1]}";
        }

        private string MaskPhone(string phone)
        {
            if(phone.Length <= 4) return "****";
            return new string('*', phone.Length - 4) + phone.Substring(phone.Length - 4);
        }
    }
}
