using System.Text.RegularExpressions;
using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Password;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration;
using Amazon_eCommerce_API.Services.Cache;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Services.Users.Seller
{
    public class SellerUserService : ISellerUserService

    {
        private readonly StoreContext _storeContext;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ICacheService _cacheService;
        public SellerUserService(StoreContext storeContext, IMapper mapper, ITokenService tokenService, ICacheService cacheService)
        {
            _storeContext = storeContext;
            _mapper = mapper;
            _tokenService = tokenService;
            _cacheService = cacheService;
        }



        public async Task<SellerUserTokenResponseDto> SellerAuthenticateUserAsync(SellerUserLoginDto sellerUserLoginDto)
        {
            
            SellerUser sellerUser = null;
         


            if (Regex.IsMatch(sellerUserLoginDto.EmailOrPhone, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {


                sellerUser = await _storeContext.SellerUsers.SingleOrDefaultAsync(u => u.BusinessEmail == sellerUserLoginDto.EmailOrPhone);
                
            }
            else
            {

                sellerUser = await _storeContext.SellerUsers.SingleOrDefaultAsync(u => u.BusinessPhoneNumber == sellerUserLoginDto.EmailOrPhone);
            
            }




            if (sellerUser == null || !await VerifySellerPasswordAsync(sellerUserLoginDto.Password, sellerUser.PasswordHash)) 
            
            { 
            
                  return null;
            
            }

            var token = _tokenService.GenerateToken(sellerUser);

            var authResponse = new SellerUserTokenResponseDto
            {

                UserId = sellerUser.Id,
                DisplayName = sellerUser.DisplayName,
                Token = token,


            };

            return authResponse;
        }

       

   
        public async Task<bool> ChangeSellerPasswordAsync(int userId, SellerUserPasswordUpdateDto userPasswordUpdateDto)
        {

            //finds the user to change password
            var existingUser = await _storeContext.SellerUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false;
            }

            //Verify Current Password
            if (!await VerifySellerPasswordAsync(userPasswordUpdateDto.CurrentPassword , existingUser.PasswordHash)) 
            {

                throw new UnauthorizedAccessException("Current password is incorrect");       
            
            }
            existingUser.PasswordHash = await HashSellerPasswordAsync(userPasswordUpdateDto.NewPassword);

            _storeContext.SellerUsers.Update(existingUser);
            var result = await _storeContext.SaveChangesAsync();
            return result > 0;

        }






        public async Task<bool> DeleteSellerUserAsync(int userId)
        {
           var user = await _storeContext.SellerUsers.FindAsync(userId);

            if (user == null) 
            
            { 
              return false;
                
            }

            _storeContext.SellerUsers.Remove(user);

            var result = await _storeContext.SaveChangesAsync();

            return result > 0;



        }

        public async Task<IEnumerable<SellerUser>> GetAllSellerUsersAsync()
        {

            return await _storeContext.SellerUsers.ToListAsync();
        }

        public async Task<SellerUser> GetUserByBusinessEmailAsync(string email)
        {
          return await _storeContext.SellerUsers.SingleOrDefaultAsync(x => x.BusinessEmail == email);
        }

        public Task<SellerUser> GetUserBySellerIdAsync(int userId)
        {
            return _storeContext.SellerUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<SellerUser> GetUserBySellerPhoneNumberAsync(string phoneNumber)
        {
            return await _storeContext.SellerUsers.SingleOrDefaultAsync(u => u.BusinessPhoneNumber == phoneNumber);
        }



        public async Task<string> HashSellerPasswordAsync(string password)
        {
            var hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));

            return hashedPassword;
        }

        public async Task<bool> IsSellerIdentifierTakenAsync(string identifier)
        {
           var existingUser = await _storeContext.SellerUsers.FirstOrDefaultAsync(u => u.BusinessEmail == identifier || u.BusinessPhoneNumber == identifier);

            return existingUser != null;
        }

        
        public async Task<SellerUser> RegisterSellerUserAsync(
            SellerUserAccountCreationDto  accountCreationDto,
            SellerUserBusinessInformationDto  businessInformationDto,
            SellerUserBusinessProfileDto  businessProfileDto,
            SellerUserPrimaryContactInformationDto  primaryContactInformationDto,
            SellerUserPaymentInformationDto  paymentInformationDto,
            SellerUserStoreInformationDto  storeInformationDto,
            SellerUserVerificationStatusDto  verificationStatusDto)
        {

            // Hash password 

            var hashedPassword = await HashSellerPasswordAsync(accountCreationDto.Password);
            var sellerUser = new SellerUser
            {
                BusinessEmail = accountCreationDto.Email,
                PasswordHash = hashedPassword,
                BusinessPhoneNumber = businessInformationDto.BusinessPhoneNumber,
                OnboardingStatus = SellerOnboardingStatus.AccountCreated,
                IsEmailVerified = false,
                IsPhoneNumberVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                
            };

          await _storeContext.SellerUsers.AddAsync(sellerUser);
          await _storeContext.SaveChangesAsync();

    //Business Information
   
    await _storeContext.SellerBusinessInformation.AddAsync(new SellerBusinessInformation

    {
        SellerUserId = sellerUser.Id,
        BusinessName = businessInformationDto.BusinessName,
        BusinessPhoneNumber = businessInformationDto.BusinessPhoneNumber,
        CompanyRegistrationNumber = businessInformationDto.CompanyRegistrationNumber,
        Country = businessInformationDto.Country,
        AddressLine1 = businessInformationDto.AddressLine1,
        AddressLine2 = businessInformationDto.AddressLine2,
        City = businessInformationDto.City,
        State = businessInformationDto.State,
        ZipCode = businessInformationDto.ZipCode
    });

    
    //Business Profile
    await _storeContext.SellerBusinessProfiles.AddAsync(new SellerBusinessProfile
    {
        SellerUserId = sellerUser.Id,
        BusinessType = businessProfileDto.BusinessType,
        AgreedToTerms = businessProfileDto.AgreeToTerms,
        TermsAcceptedAt = DateTime.UtcNow

    });
    
    //Primary Contact

    var DateOfBirth = new DateOnly(
        
        primaryContactInformationDto.BirthYear,
        primaryContactInformationDto.BirthMonth,
        primaryContactInformationDto.BirthDay
        );
    
    await _storeContext.SellerPrimaryContacts.AddAsync(new SellerPrimaryContact
    {
   SellerUserId = sellerUser.Id,
   FirstName = primaryContactInformationDto.FirstName,
   MiddleName = primaryContactInformationDto.MiddleName,
   LastName = primaryContactInformationDto.LastName,
   CountryOfBirth = primaryContactInformationDto.CountryOfBirth,
   DateOfBirth = DateOfBirth,
   IdentityProof = primaryContactInformationDto.IdentityProof,
   IdentityProofNumber = primaryContactInformationDto.IdentityProofNumber,
   IdentityProofExpirationDate = primaryContactInformationDto.IdentityProofExpirationDate,
   CountryOfIssue =  primaryContactInformationDto.CountryOfIssue,
    });


    var Last4Digits = paymentInformationDto.CardNumber.Length >= 4
        ? paymentInformationDto.CardNumber[^4..]
        : paymentInformationDto.CardNumber;

    
    string DetectCardBrand(string cardNumber)
    {
        return cardNumber switch
        {
            var n when n.StartsWith("34") || n.StartsWith("37") => "American Express",
            var n when n.StartsWith("4") => "Visa",
            var n when n.StartsWith("5") => "MasterCard",
            var n when n.StartsWith("6") => "Discover",
            var n when n.StartsWith("35") => "JCB",
            _ => "Unknown"
        };
    }
    
    var PaymentProviderToken = "tok_mok" + Guid.NewGuid();  

    await _storeContext.SellerPaymentProfiles.AddAsync(new SellerPaymentProfile

        {
            SellerUserId = sellerUser.Id,
            PaymentProviderToken = PaymentProviderToken,
            CardBrand = DetectCardBrand(paymentInformationDto.CardNumber),
            CardHolderName = paymentInformationDto.CardHolderName,
            Last4Digits = Last4Digits,
            IsDefaultPaymentMethod = true,
            ExpirationMonth = paymentInformationDto.ExpirationMonth,
            ExpirationYear = paymentInformationDto.ExpirationYear,
            BillingAddressLine1 = paymentInformationDto.BillingAddressLine1,
            BillingAddressLine2 = paymentInformationDto.BillingAddressLine2,
            BillingCity = paymentInformationDto.BillingCity,
            BillingState = paymentInformationDto.BillingState,
            BillingZipCode = paymentInformationDto.BillingZipCode,
            BillingCountry = paymentInformationDto.BillingCountry,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
            
        }
    );

    await _storeContext.SellerStoreInformation.AddAsync(new SellerStoreInformation
    {

        SellerUserId = sellerUser.Id,
        StoreName = storeInformationDto.StoreName,
        HasUPCsForAllProducts = storeInformationDto.HasUPCsForAllProducts,
        HasDiversityCertifications = storeInformationDto.HasDiversityCertifications,
        BrandOwnerShip = storeInformationDto.BrandOwnerShip,
        TrademarkStatus = storeInformationDto.TrademarkStatus
        
    });

    await _storeContext.SellerVerificationStatus.AddAsync(new SellerVerificationStatus
    {
        SellerUserId = sellerUser.Id,
        DocumentType = verificationStatusDto.DocumentType,
        DocumentFrontUrl = verificationStatusDto.DocumentFrontUrl,
        DocumentBackUrl = verificationStatusDto.DocumentBackUrl,
        ProofOfAddress = verificationStatusDto.ProofOfAddress,
        ProofOfAddressDocumentUrl = verificationStatusDto.ProofOfAddressDocumentUrl,
        ScheduledVerificationType = verificationStatusDto.ScheduledVerificationType,
        VerificationStatus = verificationStatusDto.VerificationStatus,
        
        
    });
            return sellerUser;

        }
      

        public async Task<bool> ResetSellerPasswordAsync(SellerUserForgotPasswordDto forgotPasswordDto)
        {

            if (forgotPasswordDto.NewPassword != forgotPasswordDto.ReEnterPassword) 
            
            {
                throw new ArgumentException("The password and confirmation password do not match");
            
            }


            var user = await GetUserByBusinessEmailAsync(forgotPasswordDto.Email);

            if (user == null)
            {

                throw new Exception("The user you are looking for does not exist");

            }



            var cachedOtp = await _cacheService.ValidateOtpAsync(forgotPasswordDto.Email,forgotPasswordDto.Otp);

            if (cachedOtp == null) {



                return false;
            }

            //Hash new password before updating


            var hashedPassword = await HashSellerPasswordAsync(forgotPasswordDto.NewPassword);


            user.PasswordHash = hashedPassword;

            var updateResult = await UpdateSellerUserAsync(user.Id, user);


            if (updateResult) {


                await _cacheService.RemoveOtpAsync(forgotPasswordDto.Email);

            }

            return updateResult;




        }

        

        public Task<bool> UpdateSellerEmailAsync(int userId, string newEmail)
        {
            throw new NotImplementedException();
        }




        public async Task<bool> UpdateSellerUserAsync(int userId, SellerUser sellerUser)
        {
            // Retrieve the user from the database by ID
            var existingUser = await _storeContext.SellerUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false; // Return false or throw an exception if the user is not found
            }

           
            existingUser.FirstName = sellerUser.FirstName;
            existingUser.LastName = sellerUser.LastName;  
            existingUser.SellerEmail = sellerUser.SellerEmail;
            existingUser.PersonalPhoneNumber = sellerUser.PersonalPhoneNumber;
            existingUser.IsEmailVerified = sellerUser.IsEmailVerified;

      
     
            // Update the user entity in the database
            _storeContext.SellerUsers.Update(existingUser);

            var result = await _storeContext.SaveChangesAsync();

            return result > 0; // Return true if update was successful
        }

      

        public async Task<bool> VerifySellerPasswordAsync(string enteredPassword, string storedHash)
        {

            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash));


        }

    
    }
}
