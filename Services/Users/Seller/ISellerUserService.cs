using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.Users;

namespace Amazon_eCommerce_API.Services.Users
{
    public interface IBusinessUserService
    {


        Task<IEnumerable<CustomerUsers>> GetAllCustomerUsersAsync();

        Task<CustomerUsers> RegisterCustomerUserAsync(BusinessUserRegistrationDto userRegistrationDto ,string roleName);

        Task<BusinessUserTokenResponseDto> CustomerAuthenticateUserAsync(BusinessUserLoginDto userLoginDto);

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



        Task<bool> ChangeCustomerPasswordAsync(int userId, BusinessUserPasswordUpdateDto userPasswordUpdateDto);

        Task<bool> UpdateCustomerEmailAsync(int userId, string newEmail);

        Task<bool> ResetCustomerPasswordAsync(BusinessUserForgotPasswordDto forgotPasswordDto);


        

    }
}
