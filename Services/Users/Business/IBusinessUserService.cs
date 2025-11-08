using Amazon_eCommerce_API.Models.DBEntities.Users;
using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.Users;

namespace Amazon_eCommerce_API.Services.Users
{
    public interface IBusinessUserService
    {


        Task<IEnumerable<BusinessUsers>> GetAllBusinessUsersAsync();

        Task<BusinessUsers> RegisterCustomerUserAsync(BusinessUserRegistrationDto userRegistrationDto ,string roleName);

        Task<BusinessUserTokenResponseDto> CustomerAuthenticateUserAsync(BusinessUserLoginDto userLoginDto);

        Task<BusinessUsers> GetUserByCustomerIdAsync(int userId);

        Task<BusinessUsers> GetUserByCustomerEmailAsync(string email);

        Task<BusinessUsers> GetUserByCustomerPhoneNumberAsync(string phoneNumber);

        Task<BusinessUsers> GetUserByCustomerUsernameAsync(string username);

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
