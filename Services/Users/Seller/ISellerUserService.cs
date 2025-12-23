using Amazon_eCommerce_API.Models.DBEntities.Users;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Password;

namespace Amazon_eCommerce_API.Services.Users.Seller
{
    public interface ISellerUserService
    {


        Task<IEnumerable<SellerUser>> GetAllSellerUsersAsync();

        //Task<SellerUsers> RegisterSellerUserAsync(SellerUserRegistrationDto sellerUserRegistrationDto);

        Task<SellerUserTokenResponseDto> SellerAuthenticateUserAsync(SellerUserLoginDto sellerUserLoginDto);

        Task<SellerUser> GetUserBySellerIdAsync(int sellerUserId);

        Task<SellerUser> GetUserByBusinessEmailAsync(string sellerEmail);

        Task<SellerUser> GetUserBySellerPhoneNumberAsync(string sellerPhoneNumber);

        Task<bool> UpdateSellerUserAsync(int sellerUserId, SellerUser sellerUser);
        
        Task<bool> DeleteSellerUserAsync(int sellerUserId);
        
        Task<bool> IsSellerIdentifierTakenAsync(string sellerIdentifier);
        

        Task<string> HashSellerPasswordAsync(string sellerPassword);

        Task<bool> VerifySellerPasswordAsync(string sellerEnteredPassword, string sellerStoredHash);



        Task<bool> ChangeSellerPasswordAsync(int sellerUserId, SellerUserPasswordUpdateDto sellerUserPasswordUpdateDto);

        Task<bool> UpdateSellerEmailAsync(int sellerUserId, string sellerNewEmail);

        Task<bool> ResetSellerPasswordAsync(SellerUserForgotPasswordDto forgotPasswordDto);


        

    }
}
