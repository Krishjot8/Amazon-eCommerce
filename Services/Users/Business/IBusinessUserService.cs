using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Password;

namespace Amazon_eCommerce_API.Services.Users.Business
{
    public interface IBusinessUserService
    {


        Task<IEnumerable<BusinessUser>> GetAllBusinessUsersAsync();
        
        Task<bool> IsBusinessEmailAvailableAsync(string email);

        Task<BusinessUser> RegisterBusinessAccountAsync(BusinessAccountSetupDto setupDto, BusinessAccountDetailsDto accountDetailsDto);
        
        Task<BusinessUser> AddBusinessDetailsAsync(int userId, BusinessAccountDetailsDto detailsDto);
        
        Task<BusinessUserTokenResponseDto> BusinessAuthenticateUserAsync(BusinessUserLoginDto userLoginDto);
        

        Task<BusinessUser> GetUserByBusinessIdAsync(int userId);

        Task<BusinessUser> GetUserByBusinessEmailAsync(string email);

        Task<BusinessUser> GetUserByBusinessPhoneNumberAsync(string phoneNumber);

        Task<BusinessStoreInformation> GetUserByBusinessNameAsync(string businessName);

        
        Task<bool> UpdateBusinessUserAsync(int userId, UpdateBusinessUserDto updateBusinessUserDto);


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
