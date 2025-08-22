using Amazon_eCommerce_API.Models.DTO_s.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace Amazon_eCommerce_API.Services.Cache
{
    public class CacheService : ICacheService
    {

        private readonly IMemoryCache _memoryCache;
      

        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        
        }

        private readonly TimeSpan DefaultOtpExpiry = TimeSpan.FromMinutes(10);

        public async Task<bool> CanRequestOtpAsync(string email)
        {

            _memoryCache.TryGetValue($"otp_limit_{email}", out bool isLimited);

            return await Task.FromResult(!isLimited);

        }


        public async Task<OtpCacheDto> GetOtpAsync(string email)
        {
            _memoryCache.TryGetValue(email, out OtpCacheDto otpCacheDto);
            return await Task.FromResult(otpCacheDto);
        }




        public async Task<bool> IsOtpExpiredAsync(string email)
        {
            var otp = await GetOtpAsync(email);

            return otp == null || otp.ExpirationTime <= DateTime.UtcNow;
        }




        public async Task RemoveOtpAsync(string email)
        {
            _memoryCache.Remove(email);
             await Task.CompletedTask;
        }






        public async Task SetOtpAsync(string email, OtpCacheDto otpCacheDto)
        {
            _memoryCache.Set(email, otpCacheDto, DefaultOtpExpiry);

            await Task.CompletedTask;

        }

        public async Task SetOtpRequestLimitAsync(string email, OtpRequestLimitDto otpRequestLimitDto)
        {

            var key = $"opt_limit_{email}";

            
            _memoryCache.Set(key, otpRequestLimitDto, TimeSpan.FromMinutes(otpRequestLimitDto.ExpirationMinutes));

            await Task.CompletedTask;
        }

        public async Task<OtpRequestLimitDto> GetOtpRequestLimitAsync(string email)
        {
            var key = $"otp_limit_{email}";

            if(_memoryCache.TryGetValue(key, out OtpRequestLimitDto otpRequestLimitDto))
            {

                return await Task.FromResult(otpRequestLimitDto);
            }

            return await Task.FromResult<OtpRequestLimitDto>(null);
        }

        public async Task<bool> RemoveOtpRequestLimitAsync(string email)
        {
            var key = $"otp_limit_{email}";

            if (_memoryCache.TryGetValue(key, out _))
            {
                _memoryCache.Remove(key);
                return await Task.FromResult(true);

            }

            return await Task.FromResult(false);
        }
        






        public async Task<OtpCacheDto> ValidateOtpAsync(string email, string otp)
        {
            var cachedOtp = await GetOtpAsync(email);
          

            if(cachedOtp == null || cachedOtp.ExpirationTime < DateTime.UtcNow)
            {



                return null;

            }

            return cachedOtp.Otp == otp ? cachedOtp: null;

        }
    }
}

