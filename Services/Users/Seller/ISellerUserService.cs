using Amazon_eCommerce_API.Models.DBEntities.Users;
using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.DTO_s.SellerAccount;
using Amazon_eCommerce_API.Models.Users;

namespace Amazon_eCommerce_API.Services.Users
{
    public interface ISellerUserService
    {


        Task<IEnumerable<SellerUsers>> GetAllSellerUsersAsync();

        Task<SellerUsers> RegisterSellerUserAsync(SellerUserRegistrationDto sellerUserRegistrationDto);

        Task<SellerUserTokenResponseDto> SellerAuthenticateUserAsync(SellerUserLoginDto sellerUserLoginDto);

        Task<SellerUsers> GetUserBySellerIdAsync(int sellerUserId);

        Task<SellerUsers> GetUserBySellerEmailAsync(string sellerEmail);

        Task<SellerUsers> GetUserBySellerPhoneNumberAsync(string sellerPhoneNumber);


        Task<CustomerUsers> GetUserBySellerUsernameAsync(string sellerUsername);

        Task<bool> UpdateSellerUserAsync(int sellerUserId, SellerUsers sellerUser);


        Task<bool> DeleteSellerUserAsync(int sellerUserId);



        Task<bool> IsSellerIdentifierTakenAsync(string sellerIdentifier);

     

        Task<string> HashSellerPasswordAsync(string sellerPassword);

        Task<bool> VerifySellerPasswordAsync(string sellerEnteredPassword, string sellerStoredHash);



        Task<bool> ChangeSellerPasswordAsync(int sellerUserId, SellerUserPasswordUpdateDto sellerUserPasswordUpdateDto);

        Task<bool> UpdateSellerEmailAsync(int sellerUserId, string sellerNewEmail);

        Task<bool> ResetSellerPasswordAsync(SellerUserForgotPasswordDto forgotPasswordDto);


        

    }
}
