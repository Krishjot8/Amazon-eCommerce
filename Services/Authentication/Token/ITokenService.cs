using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;

namespace Amazon_eCommerce_API.Services
{
    public interface ITokenService
    {

          public string GenerateToken(CustomerUser user);
    }
}
