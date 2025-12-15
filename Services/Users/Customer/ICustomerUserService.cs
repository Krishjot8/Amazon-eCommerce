using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Password;

namespace Amazon_eCommerce_API.Services.Users.Customer
{
    public interface ICustomerUserService
    {


        Task<IEnumerable<CustomerUser>> GetAllCustomerUsersAsync();

        Task<CustomerUser> RegisterCustomerUserAsync(CustomerUserRegistrationDto userRegistrationDto ,string roleName);

        Task<CustomerUserTokenResponseDto> CustomerAuthenticateUserAsync(CustomerUserLoginDto userLoginDto);

        Task<CustomerUser> GetUserByCustomerIdAsync(int userId);

        Task<CustomerUser> GetUserByCustomerEmailAsync(string email);

        Task<CustomerUser> GetUserByCustomerPhoneNumberAsync(string phoneNumber);
        

        Task<bool> UpdateCustomerUserAsync(int userId, CustomerUser user);


        Task<bool> DeleteCustomerUserAsync(int userId);


        Task<bool> SubscribeToNewsLetterAsync(int userId);

        Task<bool> IsCustomerIdentifierTakenAsync(string identifier);

        Task<bool> IsCustomerUsernameTakenAsync(string username);

     

        Task<string> HashCustomerPasswordAsync(string password);

        Task<bool> VerifyCustomerPasswordAsync(string enteredPassword, string storedHash);



        Task<bool> ChangeCustomerPasswordAsync(int userId, CustomerUserPasswordUpdateDto userPasswordUpdateDto);

        Task<bool> UpdateCustomerEmailAsync(int userId, string newEmail);

        Task<bool> ResetCustomerPasswordAsync(CustomerUserForgotPasswordDto forgotPasswordDto);


        

    }
}
