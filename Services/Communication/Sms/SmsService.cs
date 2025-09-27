using Amazon_eCommerce_API.Models.DTO_s.Cache;
using Amazon_eCommerce_API.Models.DTO_s.UserAccount;
using Amazon_eCommerce_API.Services.Cache;
using Amazon_eCommerce_API.Services.Users;

namespace Amazon_eCommerce_API.Services.Communication.Sms;

public class SmsService : ISmsService
{
    private readonly ICacheService _cacheService;
    private readonly IUserService _userService;

    public SmsService(IUserService userService, ICacheService cacheService)
    {
        _userService = userService;
        _cacheService = cacheService;
    }

    public async Task<bool> SendOtpSmsAsync(string phoneNumber, string otp)
    {
        var otpCache = new OtpCacheDto()
        {
            Identifier = phoneNumber,
            Otp = otp,
            ExpirationTime = DateTime.UtcNow.AddMinutes(10),
            Attempts = 0

        };
        await _cacheService.SetOtpAsync(phoneNumber, otpCache);
        
        //Call real SMS provider here
        
        Console.WriteLine($"[DEBUG] Sending OTP: {otp} to phone number: {phoneNumber}");
     
        return true;
    }

    public async Task<bool> SendSmsVerificationAsync(UserVerifySmsDto dto)
    {
        var otp = new Random().Next(100000,999999).ToString();

        return await SendOtpSmsAsync(dto.PhoneNumber, otp);
    }

    public async Task<bool> VerifySmsOtpAsync(UserVerifySmsDto dto)
    {
        var otpCache = await _cacheService.GetOtpAsync(dto.PhoneNumber);

        if (otpCache == null || otpCache.ExpirationTime < DateTime.UtcNow)
            return false;

        if (otpCache.Otp == dto.SmsOtpCode)
            return true;

        return false;

    }

    public async Task<bool> ResendSmsVerificationOtpAsync(string phoneNumber)
    {
        var dto = new UserVerifySmsDto {PhoneNumber = phoneNumber};
        return await SendSmsVerificationAsync(dto);
    }

    public string GetSmsTemplate(string verificationCode)
    {
        return $"Your verification code is {verificationCode}. Don't share it. If you didn't request it, deny here.. ";
        //will add denylink later
    }
}