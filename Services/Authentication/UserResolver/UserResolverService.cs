using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.UserResolver;
using Amazon_eCommerce_API.Services.Users.Business;
using Amazon_eCommerce_API.Services.Users.Customer;
using Amazon_eCommerce_API.Services.Users.Seller;

namespace Amazon_eCommerce_API.Services.Authentication.UserResolver
{
    
    
    public class UserResolverService : IUserResolverService
    {
        private readonly ICustomerUserService  _customerUserService;
        private readonly IBusinessUserService  _businessUserService;
        private readonly ISellerUserService  _sellerUserService;

        public UserResolverService(ICustomerUserService customerUserService, IBusinessUserService businessUserService, ISellerUserService sellerUserService)
        {
            _customerUserService = customerUserService;
            _businessUserService = businessUserService;
            _sellerUserService = sellerUserService;
        }


        public async Task<ResolvedUserResult> ResolveAndValidateAsync(string identifier,string password, UserRole role)
        {
            switch (role)
            {

                case UserRole.Customer:
                {

                    var customerUser = await _customerUserService.GetUserByCustomerEmailAsync(identifier)
                               ?? await _customerUserService.GetUserByCustomerPhoneNumberAsync(identifier);

                    if (customerUser == null)
                        return new ResolvedUserResult {User = null, IsPasswordValid = false};

                    var isValid =
                        await _customerUserService.VerifyCustomerPasswordAsync(password, customerUser.PasswordHash);
                    

                    return new ResolvedUserResult
                    {
                        User = customerUser,
                        IsPasswordValid = isValid

                    };
                }

                case UserRole.Business:
                {
                    var businessUser = await _businessUserService.GetUserByBusinessEmailAsync(identifier)
                                       ?? await _businessUserService.GetUserByBusinessPhoneNumberAsync(identifier);
                    
                    if (businessUser == null)
                        return new ResolvedUserResult {User = null, IsPasswordValid = false};

                    var isValid =
                        await _businessUserService.VerifyBusinessPasswordAsync(password, businessUser.PasswordHash);
                    

                    return new ResolvedUserResult
                    {
                        User = businessUser,
                        IsPasswordValid = isValid

                    };
                }

                case UserRole.Seller:

                {
                    var sellerUser = await _sellerUserService.GetUserByBusinessEmailAsync(identifier)
                                       ?? await _sellerUserService.GetUserByBusinessPhoneNumberAsync(identifier);

                    
                    if (sellerUser == null)
                        return new ResolvedUserResult {User = null, IsPasswordValid = false};

                    var isValid =
                        await _sellerUserService.VerifySellerPasswordAsync(password, sellerUser.PasswordHash);
                    

                    return new ResolvedUserResult
                    {
                        User = sellerUser,
                        IsPasswordValid = isValid

                    };
                } 
                    
                    default:
                    return new ResolvedUserResult {User = null, IsPasswordValid = false};
                    
                
                
                
                
            }
                
        
            
        }

        public async Task<object> ResolveUserAsync(string identifier, UserRole role)
        {
            return role switch
            {

                UserRole.Customer =>
                    await _customerUserService.GetUserByCustomerEmailAsync(identifier)
                    ?? await _customerUserService.GetUserByCustomerPhoneNumberAsync(identifier),
                    
                    UserRole.Business =>
                    await _businessUserService.GetUserByBusinessEmailAsync(identifier)
                    ?? await _businessUserService.GetUserByBusinessPhoneNumberAsync(identifier),
                    
                    
                    UserRole.Seller =>
                    await _sellerUserService.GetUserByBusinessEmailAsync(identifier)
                    ?? await _sellerUserService.GetUserByBusinessPhoneNumberAsync(identifier),
                    
                    _=> null
                    


            };
        }
    }
}