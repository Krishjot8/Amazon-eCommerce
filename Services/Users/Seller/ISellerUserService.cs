using Amazon_eCommerce_API.Models.DBEntities.Users;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Password;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration;

namespace Amazon_eCommerce_API.Services.Users.Seller
{
    public interface ISellerUserService
    {   


        Task<IEnumerable<SellerUser>> GetAllSellerUsersAsync();

        
        Task<SellerUserTokenResponseDto> SellerAuthenticateUserAsync(SellerUserLoginDto sellerUserLoginDto);

        
       
        Task<SellerUser> GetUserBySellerIdAsync(int sellerUserId);

        Task<SellerUser> GetUserByBusinessEmailAsync(string businessEmail);

        Task<SellerUser> GetUserByBusinessPhoneNumberAsync(string businessPhoneNumber);

        //SellerOnboarding
        
        Task<int> CreateSellerAccountAsync(SellerUserAccountCreationDto accountCreationDto);

        Task<bool> CompleteBusinessInformationAsync(int sellerUserId, SellerUserBusinessInformationDto businessInformationDto);

        Task<bool> CompleteBusinessProfileAsync(int sellerUserId, SellerUserBusinessProfileDto businessProfileDto);
        
        Task<bool> CompletePrimaryContactAsync(int sellerUserId, SellerUserPrimaryContactInformationDto contactInformationDto);
        
        Task<bool> AddPaymentInformationAsync(int sellerUserId, SellerUserPaymentInformationDto paymentInformationDto);

        Task<bool> CompleteStoreInformationAsync(int sellerUserId, SellerUserStoreInformationDto storeInformationDto);
        
        Task<bool>SubmitVerificationAsync(int sellerUserId, SellerUserVerificationStatusDto status);
        
        Task<bool> ScheduleVerificationMeetingAsync(int sellerUserId, ScheduleVerificationMeetingDto verificationMeetingDto);
        
        Task<bool> UpdateSellerUserAsync(int sellerUserId, SellerUser sellerUser);
        
        Task<bool> DeleteSellerUserAsync(int sellerUserId);
        
        Task<bool> IsSellerIdentifierTakenAsync(string sellerIdentifier);
        

        Task<string> HashSellerPasswordAsync(string sellerPassword);

        Task<bool> VerifySellerPasswordAsync(string sellerEnteredPassword, string sellerStoredHash);
        

        Task<bool> ChangeSellerPasswordAsync(int sellerUserId, UpdateSellerUserPasswordDto updateSellerUserPasswordDto);

        Task<bool> UpdateSellerEmailAsync(int sellerUserId, string sellerNewEmail);

        Task<bool> ResetSellerPasswordAsync(SellerUserForgotPasswordDto forgotPasswordDto);


        

    }
}
