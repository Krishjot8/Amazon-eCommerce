using Amazon_eCommerce_API.Models.DTO_s.Cache;

namespace Amazon_eCommerce_API.Services.Cache
{
    public interface ICacheService
    {
        Task SetOtpAsync(string email, OtpCacheDto otpCacheDto);


        Task<OtpCacheDto> GetOtpAsync(string email);


        Task RemoveOtpAsync(string email);


        Task SetOtpRequestLimitAsync(string email, OtpRequestLimitDto otpRequestLimitDto);



        Task<OtpRequestLimitDto> GetOtpRequestLimitAsync(string email);


        Task<bool> RemoveOtpRequestLimitAsync(string email);
        

        Task<bool> IsOtpExpiredAsync(string email);


        Task<bool> CanRequestOtpAsync(string email);


        Task<OtpCacheDto> ValidateOtpAsync(string email, string otp);


      


    }
}
