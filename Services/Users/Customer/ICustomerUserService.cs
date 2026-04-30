using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication;

namespace Amazon_eCommerce_API.Services.Users.Customer
{
    public interface ICustomerUserService
    {


        Task<IEnumerable<CustomerUser>> GetAllCustomerUsersAsync(); //6

        Task<CustomerUser> RegisterCustomerUserAsync(CustomerUserRegistrationDto userRegistrationDto);

        Task<CustomerUserTokenResponseDto> CustomerAuthenticateUserAsync(CustomerUserLoginDto userLoginDto);// 1



        Task<CustomerUser> GetUserByCustomerIdAsync(int userId); //8

        Task<CustomerUser> GetUserByCustomerEmailAsync(string email); //7

        Task<CustomerUser> GetUserByCustomerPhoneNumberAsync(string phoneNumber); //9
        



        Task<bool> UpdateCustomerUserAsync(int userId, CustomerUser user);


        Task<bool> DeleteCustomerUserAsync(int userId); //5


        Task<bool> SubscribeToNewsLetterAsync(int userId);

        Task<bool> IsCustomerIdentifierTakenAsync(string identifier);
        
     

        Task<string> HashCustomerPasswordAsync(string password); //4

        Task<bool> VerifyCustomerPasswordAsync(string enteredPassword, string storedHash); //2



        Task<bool> ChangeCustomerPasswordAsync(int userId, UpdateCustomerUserPasswordDto userPasswordUpdateDto);//3


        Task<bool> UpdateCustomerEmailAsync(int userId, string newEmail);

        Task<bool> ResetCustomerPasswordAsync(CustomerUserForgotPasswordDto forgotPasswordDto);


        

    }
}
