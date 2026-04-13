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
        
        public Task <bool> StartBusinessRegistrationAsync(BusinessAccountEmailDto emailDto);

        public Task<bool> SetupBusinessAccountAsync(BusinessAccountSetupDto setupDto);

        public Task<BusinessUser> CompleteBusinessRegistrationAsync(string email , BusinessAccountDetailsDto accountDetailsDto);

        Task<BusinessUser> AddBusinessDetailsAsync(int userId, BusinessAccountDetailsDto detailsDto);
        
        Task<BusinessUserTokenResponseDto> BusinessAuthenticateUserAsync(BusinessUserLoginDto userLoginDto);
        

        Task<BusinessUser> GetUserByBusinessIdAsync(int userId);

        Task<BusinessUser> GetUserByBusinessEmailAsync(string email);

        Task<BusinessUser> GetUserByBusinessPhoneNumberAsync(string phoneNumber);

        Task<BusinessStoreInformation> GetUserByBusinessNameAsync(string businessName);

        
        
        Task<bool> UpdateBusinessUserAsync(int userId, UpdateBusinessUserDto updateBusinessUserDto);


        Task<bool> UpdateBusinessProfileAsync(int userId, UpdateBusinessProfileDto updateBusinessProfileDto);
        
        Task<bool> UpdateBusinessStoreInformationAsync(int userId, UpdateBusinessStoreInformationDto updateBusinessStoreInformationDto);
        
        Task<bool> DeleteBusinessUserAsync(int userId);

        
        Task<bool> IsBusinessIdentifierTakenAsync(string identifier);

        Task<bool> IsBusinessNameTakenAsync(string businessName);

     

        Task<string> HashBusinessPasswordAsync(string password);

        Task<bool> VerifyBusinessPasswordAsync(string enteredPassword, string storedHash);



        Task<bool> ChangeBusinessPasswordAsync(int userId, UpdateBusinessUserPasswordDto userPasswordDto);

        Task<bool> UpdateBusinessEmailAsync(int userId, string newEmail);

        Task<bool>UpdateBusinessPasswordAsync(int userId, UpdateBusinessUserPasswordDto userPasswordDto);
        
        Task<bool> ResetBusinessPasswordAsync(BusinessUserForgotPasswordDto forgotPasswordDto);


        

    }
}
