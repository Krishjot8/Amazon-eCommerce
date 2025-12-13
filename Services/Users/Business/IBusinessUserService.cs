using Amazon_eCommerce_API.Models.DBEntities.Users;
using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Password;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Registration;
using Amazon_eCommerce_API.Models.Users;

namespace Amazon_eCommerce_API.Services.Users
{
    public interface IBusinessUserService
    {


        Task<IEnumerable<BusinessUsers>> GetAllBusinessUsersAsync();


        Task<bool> IsBusinessEmailAvailableAsync(string email);


        Task<BusinessUsers> RegisterBusinessAccountAsync(BusinessAccountSetupDto setupDto);


        Task<BusinessUsers> AddBusinessDetailsAsync(int userId, BusinessAccountDetailsDto detailsDto);


        Task<BusinessUserTokenResponseDto> BusinessAuthenticateUserAsync(BusinessUserLoginDto userLoginDto);



        Task<BusinessUsers> GetUserByBusinessIdAsync(int userId);

        Task<BusinessUsers> GetUserByBusinessEmailAsync(string email);

        Task<BusinessUsers> GetUserByBusinessPhoneNumberAsync(string phoneNumber);

        Task<BusinessUsers> GetUserByBusinessNameAsync(string businessName);




        Task<bool> UpdateBusinessUserAsync(int userId, BusinessUsers user);


        Task<bool> DeleteBusinessUserAsync(int userId);



        Task<bool> IsBusinessIdentifierTakenAsync(string identifier);

        Task<bool> IsBusinessNameTakenAsync(string businessName);

     

        Task<string> HashBusinessPasswordAsync(string password);

        Task<bool> VerifyBusinessPasswordAsync(string enteredPassword, string storedHash);



        Task<bool> ChangeBusinessPasswordAsync(int userId, BusinessUserPasswordUpdateDto userPasswordUpdateDto);

        Task<bool> UpdateBusinessEmailAsync(int userId, string newEmail);

        Task<bool> ResetBusinessPasswordAsync(BusinessUserForgotPasswordDto forgotPasswordDto);


        

    }
}
