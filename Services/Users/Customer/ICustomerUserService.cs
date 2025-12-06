using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.DTO_s.CustomerAccount;
using Amazon_eCommerce_API.Models.Users;

namespace Amazon_eCommerce_API.Services.Users.Customer
{
    public interface ICustomerUserService
    {


        Task<IEnumerable<CustomerUsers>> GetAllCustomerUsersAsync();

        Task<CustomerUsers> RegisterCustomerUserAsync(CustomerUserRegistrationDto userRegistrationDto ,string roleName);

        Task<CustomerUserTokenResponseDto> CustomerAuthenticateUserAsync(CustomerUserLoginDto userLoginDto);

        Task<CustomerUsers> GetUserByCustomerIdAsync(int userId);

        Task<CustomerUsers> GetUserByCustomerEmailAsync(string email);

        Task<CustomerUsers> GetUserByCustomerPhoneNumberAsync(string phoneNumber);

        Task<CustomerUsers> GetUserByCustomerUsernameAsync(string username);

        Task<bool> UpdateCustomerUserAsync(int userId, CustomerUsers user);


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
