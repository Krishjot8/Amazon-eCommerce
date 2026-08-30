using Amazon_eCommerce_API.Models.DTO_s.Cache;
using Amazon_eCommerce_API.Services.Cache;
using Amazon_eCommerce_API.Services.Communication.Sms;
using Amazon_eCommerce_API.Services.Users.Customer;
using System.Security.Cryptography;
using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Services.Authentication.UserResolver;
using Amazon_eCommerce_API.Services.Communication.Email;

namespace Amazon_eCommerce_API.Services.Authentication.PasswordChallenge
{
    public class PasswordChallengeService(
        IUserResolverService  userResolverService,
        ICacheService cacheService,
        IEmailService emailService,
        ISmsService smsService)
        : IPasswordChallengeService //For Generating One time Password
    {
        //
        public async Task<PasswordChallengeResponseDto> GenerateOtpChallengeAsync(string identifier, string password, UserRole role)
        {

            var result = await userResolverService.ResolveAndValidateAsync(identifier, password, role);

            
            if(result.User == null || !result.IsPasswordValid)
                return null;
            

         

            var otp =  RandomNumberGenerator.GetInt32(100000, 1000000).ToString();

            
            
            var otpCache = new OtpCacheDto
            {

                Identifier = identifier,
                Otp = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                Attempts = 0,
                LastRequestTime = DateTime.UtcNow

            };

            await cacheService.SetOtpAsync(identifier, otpCache);


            OtpChannel otpChannel;
            string maskedDestination;

            if (IsValidEmail(identifier))
            {
                otpChannel = OtpChannel.Email;
                maskedDestination = MaskEmail(identifier);
                await emailService.SendOtpEmailAsync(identifier, otp);
            }
            else
            {
                otpChannel = OtpChannel.SMS;
                maskedDestination = MaskPhone(identifier);
                await smsService.SendOtpSmsAsync(identifier, otp);
            }

            //return DTO

            return new PasswordChallengeResponseDto
            {

                PendingAuthId = identifier,
                OtpChannel = otpChannel,
                MaskedDestination = maskedDestination

            };

        }

      
        

        public async Task<bool> VerifyOtpAsync(PasswordChallengeVerifyDto verifyDto)
        {
           var cachedOtp =  cacheService.ValidateOtpAsync(verifyDto.PendingAuthId, verifyDto.Otp);

           if (cachedOtp == null) return false;

           await cacheService.RemoveOtpAsync(verifyDto.PendingAuthId);
           return true;

        }

        public async Task<ResendOtpResponseDto> ResendOtpAsync(ResendOtpRequestDto request)
        {
            var cache = await cacheService.GetOtpAsync(request.PendingAuthId);
            

            if (cache == null)
            {
                return new ResendOtpResponseDto
                {
                    Success = false,
                    Message = "Session expired, Please login again."
                };
            }

            var cooldownSeconds = cache.Attempts == 0 ? 60 : 90;

            var nextAllowedTime = cache.LastRequestTime.AddSeconds(cooldownSeconds);

            if (DateTime.UtcNow < nextAllowedTime)
            {
                var remaining = (nextAllowedTime - DateTime.UtcNow).Seconds;
                
                return new ResendOtpResponseDto
                {
                    Success = false,
                    Message = $"Please wait {remaining} seconds before requesting another code.",
                    CooldownSeconds = remaining
                };
            }
            
            

            var newOtp = RandomNumberGenerator.GetInt32(100000, 1000000).ToString();
            
            cache.Otp = newOtp;
            cache.ExpirationTime = DateTime.UtcNow.AddMinutes(5);
            cache.LastRequestTime = DateTime.UtcNow;
            cache.Attempts++;
            
            await cacheService.SetOtpAsync(request.PendingAuthId, cache);
            
            
            if (cache.OtpChannel == OtpChannel.Email)
            {
                await emailService.SendOtpEmailAsync(cache.Identifier, newOtp);
            }
            else
            {
                await smsService.SendOtpSmsAsync(cache.Identifier, newOtp);
            }

            return new ResendOtpResponseDto
            {
                Success = true,
                Message = "OTP resent successfully.",
                PendingAuthId = request.PendingAuthId,
                OtpChannel = cache.OtpChannel,
                MaskedDestination = cache.MaskedDestination,
                CooldownSeconds = cooldownSeconds
                
            };
        }

        public async Task GeneratePasswordResetOtpAsync(string identifier, AccountType accountType)
        {

            var normalizedIdentifier = identifier.Trim().ToLowerInvariant();
            var user = await userResolverService.ResolveUserAsync(normalizedIdentifier, (UserRole)accountType);
            if (user == null) return;


            var otpCode = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            var otpCacheDto = new OtpCacheDto
            {
                Identifier = normalizedIdentifier,
                Otp = otpCode,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10),
                Attempts = 0,
                LastRequestTime = DateTime.UtcNow
            };

            await cacheService.SetOtpAsync(normalizedIdentifier, otpCacheDto);

            string displayName = user switch
            {
                CustomerUser customer => $"{customer.FirstName} {customer.LastName}".Trim(),
                BusinessUser business => business.BusinessProfile != null
                    ? $"{business.BusinessProfile.FirstName} {business.BusinessProfile.LastName}".Trim()
                    : "Business Partner",
                _ => "Valued User"

            };


            var centralZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var centralTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, centralZone);
            string formattedTimestamp = $"{centralTime:MMM dd, yyyy hh:mm tt} Central Daylight Time";
            
            await emailService.SendPasswordResetOtpEmailAsync(
                email: normalizedIdentifier,
                otp: otpCode,
                timestamp: formattedTimestamp,
                userName: displayName,
                deviceDetails: "generic web browser macOS (Desktop)",
                locationDetails: "Texas, United States");
        }




        public async Task<bool> VerifyPasswordResetOtpAsync(string pendingAuthId, string otp, AccountType accountType)
        {
         var normalizedIdentifier = pendingAuthId.Trim().ToLowerInvariant();
         
         
         var cachedOtpDto = await cacheService.GetOtpAsync(normalizedIdentifier);
         
         if(cachedOtpDto == null || cachedOtpDto.Otp != otp)
         {
             return false;
         }

         
         await cacheService.RemoveOtpAsync(normalizedIdentifier);
         
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
