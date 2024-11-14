using Amazon_eCommerce_API.Models.Users;

namespace Amazon_eCommerce_API.Services
{
    public interface ITokenService
    {

          public string GenerateToken(User user);
    }
}
