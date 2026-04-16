using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;

namespace Amazon_eCommerce_API.Services.Authentication.Token
{
    public interface ITokenService
    {

          public string GenerateToken(TokenRequestDto request);
    }
}
