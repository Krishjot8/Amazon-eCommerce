using Amazon_eCommerce_API.Models.DTO_s.Cache;
using Amazon_eCommerce_API.Services.Cache;
using Amazon_eCommerce_API.Services.Communication.Sms;
using Amazon_eCommerce_API.Services.Email;
using Amazon_eCommerce_API.Services.Users.Customer;
using System.Security.Cryptography;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Password;

namespace Amazon_eCommerce_API.Services.Authentication.PasswordChallenge
{
    public class PasswordChallengeService(
        ICustomerUserService customerUserService,
        ICacheService cacheService,
        IEmailService emailService,
        ISmsService smsService)
        : IPasswordChallengeService //For Generating One time Password
    {
        //
        public async Task<CustomerUserPasswordChallengeResponseDto> GenerateOtpChallengeAsync(string identifier,
            string password)
        {

            var user = await customerUserService.GetUserByCustomerEmailAsync(identifier) ??
                       await customerUserService.GetUserByCustomerPhoneNumberAsync(identifier);


            if (user == null || !await customerUserService.VerifyCustomerPasswordAsync(password, user.PasswordHash))
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

            await cacheService.SetOtpAsync(identifier, otpCache);


            string otpChannel;
            string maskedDestination;

            if (IsValidEmail(identifier))
            {
                otpChannel = "email";
                maskedDestination = MaskEmail(identifier);
                await emailService.SendOtpEmailAsync(identifier, otp);
            }
            else
            {
                otpChannel = "sms";
                maskedDestination = MaskPhone(identifier);
                await smsService.SendOtpSmsAsync(identifier, otp);
            }

            //return DTO

            return new CustomerUserPasswordChallengeResponseDto
            {

                PendingAuthId = identifier,
                OtpChannel = otpChannel,
                MaskedDestination = maskedDestination

            };

        }

      
        

        public async Task<bool> VerifyOtpAsync(CustomerUserPasswordChallengeVerifyDto verifyDto)
        {
           var cachedOtp =  cacheService.ValidateOtpAsync(verifyDto.PendingAuthId, verifyDto.Otp);

           if (cachedOtp == null) return false;

           await cacheService.RemoveOtpAsync(verifyDto.PendingAuthId);
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
