using System.Text.RegularExpressions;
using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Password;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.SellerRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Services.Authentication.Token;
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

            var store = await _storeContext.SellerStoreInformation
                .FirstOrDefaultAsync(s=> s.SellerUserId == sellerUser.Id);

            
            var primaryContact = await _storeContext.SellerPrimaryContacts
                .FirstOrDefaultAsync(x => x.SellerUserId == sellerUser.Id);

            var fullName = primaryContact == null
                ? "Seller User" :
             string.Join (" ",primaryContact.FirstName, primaryContact.LastName);
            

            var tokenRequest = new TokenRequestDto
            {
                UserId = sellerUser.Id,
                Email = sellerUser.BusinessEmail,
                Role = UserRole.Seller,
                DisplayName = fullName,
                StoreName = store?.StoreName


            };
            
            var token = _tokenService.GenerateToken(tokenRequest);

           return new SellerUserTokenResponseDto
            {

                UserId = sellerUser.Id,
                FullName = fullName,
                StoreName = store?.StoreName,
                Token = token,
            };
           
        }

       

   
        public async Task<bool> ChangeSellerPasswordAsync(int userId, UpdateSellerUserPasswordDto userPasswordDto)
        {

            //finds the user to change password
            var existingUser = await _storeContext.SellerUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false;
            }

            //Verify Current Password
            if (!await VerifySellerPasswordAsync(userPasswordDto.CurrentPassword , existingUser.PasswordHash)) 
            {

                throw new UnauthorizedAccessException("Current password is incorrect");       
            
            }
            existingUser.PasswordHash = await HashSellerPasswordAsync(userPasswordDto.NewPassword);

            _storeContext.SellerUsers.Update(existingUser);
            var result = await _storeContext.SaveChangesAsync();
            return result > 0;

        }


        public Task<bool> UpdateSellerUserAsync(int sellerUserId, SellerUser sellerUser)
        {
            throw new NotImplementedException();
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

        public async Task<SellerUser> GetUserByBusinessEmailAsync(string businessEmail)
        {
          return await _storeContext.SellerUsers.SingleOrDefaultAsync(x => x.BusinessEmail == businessEmail);
        }

        public Task<SellerUser> GetUserBySellerIdAsync(int userId)
        {
            return _storeContext.SellerUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<SellerUser> GetUserByBusinessPhoneNumberAsync(string businessPhoneNumber)
        {
            return await _storeContext.SellerUsers.SingleOrDefaultAsync(u => u.BusinessPhoneNumber == businessPhoneNumber);
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
            user.UpdatedAt = DateTime.UtcNow;
            
            await _storeContext.SaveChangesAsync();

           await _cacheService.RemoveOtpAsync(forgotPasswordDto.Email);
           
           return true;
           

        }

        

        public Task<bool> UpdateSellerEmailAsync(int userId, string newEmail)
        {
            throw new NotImplementedException();
        }

        
        public async Task<int> CreateSellerAccountAsync(SellerUserAccountCreationDto accountCreationDto)
        {
            // Retrieve the user from the database by ID
            var existingUser = await _storeContext.SellerUsers
                .FirstOrDefaultAsync(x => x.BusinessEmail == accountCreationDto.Email);

            if (existingUser != null)
            {
                throw new Exception("The seller account with this email already exists");
            }

            if (accountCreationDto.Password != accountCreationDto.ConfirmPassword)
                throw new Exception("The password and confirmation password do not match");
      
     
            var hashedPassword = await HashSellerPasswordAsync(accountCreationDto.Password);


            var sellerUser = new SellerUser
            {

                BusinessEmail = accountCreationDto.Email,
                PasswordHash = hashedPassword,
                OnboardingStep = SellerOnboardingStep.AccountCreated,
                AccountStatus = SellerAccountStatus.Pending,
                IsEmailVerified = false,
                IsPhoneNumberVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                
            };
            
            // Update the user entity in the database
           await _storeContext.SellerUsers.AddAsync(sellerUser);
           await _storeContext.SaveChangesAsync();



           return sellerUser.Id; // Return true if update was successful
        }

        
        
        public async Task<bool> CompleteBusinessInformationAsync(int sellerUserId, SellerUserBusinessInformationDto businessInformationDto)
        {
            var sellerUser = await _storeContext.SellerUsers
                .FirstOrDefaultAsync(u => u.Id == sellerUserId);

            if (sellerUser == null)
                return false;
            if(sellerUser.OnboardingStep != SellerOnboardingStep.AccountCreated)
                throw new Exception("You must complete Account Creation step first");

            var businessInfo = new SellerBusinessInformation
            {

                SellerUserId = sellerUser.Id,
                BusinessName = businessInformationDto.BusinessName,
                BusinessPhoneNumber = businessInformationDto.VerificationPhoneNumber,
                CompanyRegistrationNumber = businessInformationDto.CompanyRegistrationNumber,
                CountryCode = businessInformationDto.Country,
                AddressLine1 = businessInformationDto.AddressLine1,
                AddressLine2 = businessInformationDto.AddressLine2,
                City = businessInformationDto.City,
                State = businessInformationDto.State,
                ZipCode = businessInformationDto.ZipCode

            };
            
            await _storeContext.SellerBusinessInformation.AddAsync(businessInfo);
            
            sellerUser.OnboardingStep = SellerOnboardingStep.BusinessInformation;
            sellerUser.UpdatedAt = DateTime.UtcNow;
            
            await _storeContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> CompleteBusinessProfileAsync(int sellerUserId, SellerUserBusinessProfileDto businessProfileDto)
        {
            var seller = await _storeContext.SellerUsers
                .FirstOrDefaultAsync(u => u.Id == sellerUserId);
            
            if (seller == null)
                return false;
            
            if(seller.OnboardingStep != SellerOnboardingStep.BusinessInformation)
                throw new Exception("You must complete Business Information first");

            var businessProfile = new SellerBusinessProfile
            {

                SellerUserId = sellerUserId,
                BusinessType = businessProfileDto.BusinessType,
                AgreedToTerms = businessProfileDto.AgreeToTerms,
                TermsAcceptedAt = DateTime.UtcNow,

            };
            
            await _storeContext.SellerBusinessProfiles.AddAsync(businessProfile);
            seller.OnboardingStep = SellerOnboardingStep.BusinessProfile;
            seller.UpdatedAt = DateTime.UtcNow;
            
            await _storeContext.SaveChangesAsync();
            
            return true;
        }
        
        
        public async Task<bool> CompletePrimaryContactAsync(int sellerUserId, SellerUserPrimaryContactInformationDto contactInformationDto)
        {
            var sellerUser = await _storeContext.SellerUsers
                .FirstOrDefaultAsync(u => u.Id == sellerUserId);
            
            if (sellerUser == null)
                return false;
            
            if (sellerUser.OnboardingStep != SellerOnboardingStep.BusinessProfile)
                throw new Exception("You must complete the Business Profile step first");

            var dateOfBirth = new DateOnly(

                contactInformationDto.BirthYear,
                contactInformationDto.BirthMonth,
                contactInformationDto.BirthDay
            );


            var today = DateTime.UtcNow.Date;
            var dob = dateOfBirth.ToDateTime(TimeOnly.MinValue);
            
            var age =today.Year - dob.Year;

            if (dob > today.AddYears(-age))
                age--;
            
            if(age < 18)
                throw new Exception("You must be at least 18 years old");
            
            var primaryContact = new SellerPrimaryContact
            {
                SellerUserId = sellerUserId,
                FirstName = contactInformationDto.FirstName,
                MiddleName = contactInformationDto.MiddleName,
                LastName = contactInformationDto.LastName,
                CountryCode = contactInformationDto.Country,
                DateOfBirth = dateOfBirth,
                IdentityDocumentType = contactInformationDto.IdentityDocumentType,
                IdentityProofNumber = contactInformationDto.IdentityProofNumber,
                IdentityProofExpirationDate = contactInformationDto.IdentityProofExpirationDate,
                CountryOfIssue = contactInformationDto.CountryOfIssue,


            };
            
            await _storeContext.SellerPrimaryContacts.AddAsync(primaryContact);
            
            sellerUser.OnboardingStep = SellerOnboardingStep.PrimaryContact;
            sellerUser.UpdatedAt = DateTime.UtcNow;
            
            await _storeContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> AddPaymentInformationAsync(int sellerUserId, SellerUserPaymentInformationDto paymentInformationDto)
        {
           
            var sellerUser = await _storeContext.SellerUsers.FirstOrDefaultAsync(x => x.Id == sellerUserId);
            
            if (sellerUser == null)
                return false;
            
            if(sellerUser.OnboardingStep != SellerOnboardingStep.PrimaryContact)
                throw new Exception("You must complete the PrimaryContact step first");

            var last4 = paymentInformationDto.CardNumber.Length > 4 
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
                    _ => "Unknown"


                };


            }


            var paymentToken = "tok_" + Guid.NewGuid();

            var paymentProfile = new SellerPaymentProfile
            {

                SellerUserId = sellerUserId,
                PaymentProviderToken = paymentToken,
                CardBrand = DetectCardBrand(paymentInformationDto.CardNumber),
                CardHolderName = paymentInformationDto.CardHolderName,
                Last4Digits = last4,
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
                UpdatedAt = DateTime.UtcNow,
            };
            
            await _storeContext.SellerPaymentProfiles.AddAsync(paymentProfile);

            sellerUser.OnboardingStep = SellerOnboardingStep.PaymentInformation;
            sellerUser.UpdatedAt = DateTime.UtcNow;
            
            await _storeContext.SaveChangesAsync();
            
            return true;
        }

        
        
        
        public async Task<bool> CompleteStoreInformationAsync(int sellerUserId, SellerUserStoreInformationDto storeInformationDto)
        {
            var sellerUser = await _storeContext.SellerUsers.FirstOrDefaultAsync(x => x.Id == sellerUserId);

            if (sellerUser == null)
                return false;
            
            if (sellerUser.OnboardingStep != SellerOnboardingStep.PaymentInformation)
                throw new Exception("You must complete the PaymentInformation step first");

            var storeInfo = new SellerStoreInformation
            {
               SellerUserId = sellerUserId,
               StoreName = storeInformationDto.StoreName,
               HasUPCsForAllProducts = storeInformationDto.HasUPCsForAllProducts,
               HasDiversityCertifications = storeInformationDto.HasDiversityCertifications,
               BrandOwnership = storeInformationDto.BrandOwnership,
               TrademarkStatus = storeInformationDto.TrademarkStatus,
               
            };
            
            await _storeContext.SellerStoreInformation.AddAsync(storeInfo);
            
            sellerUser.OnboardingStep = SellerOnboardingStep.StoreInformation;
            sellerUser.UpdatedAt = DateTime.UtcNow;
            
            await _storeContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> SubmitVerificationAsync(int sellerUserId, SellerUserVerificationStatusDto verificationStatus)
        {
            var sellerUser = await _storeContext.SellerUsers.FirstOrDefaultAsync(x => x.Id == sellerUserId);
            
            if (sellerUser == null)
                return false;
            
            if (sellerUser.OnboardingStep != SellerOnboardingStep.StoreInformation)
                throw new Exception("You must complete the StoreInformation step first");

            var verification = new SellerVerification
            {

                SellerUserId = sellerUserId,
                DocumentType =  verificationStatus.DocumentType,
                DocumentFrontUrl = verificationStatus.DocumentFrontUrl,
                DocumentBackUrl = verificationStatus.DocumentBackUrl,
                ProofOfAddress = verificationStatus.ProofOfAddress,
                ProofOfAddressDocumentUrl = verificationStatus.ProofOfAddressDocumentUrl,
                
                VerificationStatus = VerificationState.Pending

            };
            
            await _storeContext.SellerVerificationStatus.AddAsync(verification);

            sellerUser.OnboardingStep = SellerOnboardingStep.VerificationSubmitted;

            sellerUser.AccountStatus = SellerAccountStatus.UnderReview;
            
            
            sellerUser.UpdatedAt = DateTime.UtcNow;
            
            await _storeContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> ScheduleVerificationMeetingAsync(int sellerUserId, ScheduleVerificationMeetingDto verificationMeetingDto)
        {
           var sellerUser = await _storeContext.SellerUsers.FirstOrDefaultAsync(x => x.Id == sellerUserId);
           
           if (sellerUser == null)
               return false;
           
           if (sellerUser.OnboardingStep != SellerOnboardingStep.VerificationSubmitted)
               throw new Exception("You must submit the verification first");
           
           var verification = await _storeContext.SellerVerificationStatus
               .FirstOrDefaultAsync(x => x.SellerUserId == sellerUserId);
           
           if (verification == null)
               throw new Exception("verification record not found");

           verification.VerificationMethod = verificationMeetingDto.MeetingType;
           
          verification.ScheduledDateTime = verificationMeetingDto.ScheduledDateTime;
           verification.Notes = verificationMeetingDto.Notes;

           sellerUser.OnboardingStep = SellerOnboardingStep.MeetingScheduled;
           
           sellerUser.UpdatedAt = DateTime.UtcNow;
           
           await _storeContext.SaveChangesAsync();
           
           return true;

        }


        public async Task<bool> VerifySellerPasswordAsync(string enteredPassword, string storedHash)
        {

            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash));


        }

    
    }
}
