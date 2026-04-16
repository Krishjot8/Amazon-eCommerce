using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.UserResolver;

namespace Amazon_eCommerce_API.Services.Authentication.UserResolver
{
    public interface IUserResolverService
    {
        Task<ResolvedUserResult> ResolveAndValidateAsync(string identifier, string password, UserRole role);
        
        Task<object?> ResolveUserAsync(string identifier, UserRole role);
    }
}