using System.Text.RegularExpressions;
using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.CacheStates.Authentication;
using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Password;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Services.Authentication.Token;
using Amazon_eCommerce_API.Services.Cache;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Services.Users.Business
{
    public class BusinessUserService : IBusinessUserService

    {
        private readonly StoreContext _storeContext;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ICacheService _cacheService;

        public BusinessUserService(StoreContext storeContext, IMapper mapper, ITokenService tokenService,
            ICacheService cacheService)
        {
            _storeContext = storeContext;
            _mapper = mapper;
            _tokenService = tokenService;
            _cacheService = cacheService;
        }


        
        public async Task<IEnumerable<BusinessUser>> GetAllBusinessUsersAsync()
        {

            return await _storeContext.BusinessUsers.ToListAsync();
        }

        public async Task<BusinessUser> GetUserByBusinessEmailAsync(string email)
        {
            return await _storeContext.BusinessUsers.SingleOrDefaultAsync(x => x.BusinessEmail == email);
        }

        public Task<BusinessUser> GetUserByBusinessIdAsync(int userId)
        {
            return _storeContext.BusinessUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<BusinessUser> GetUserByBusinessPhoneNumberAsync(string phoneNumber)
        {
            return await _storeContext.BusinessUsers.SingleOrDefaultAsync(u => u.BusinessPhoneNumber == phoneNumber);
        }

        public async Task<BusinessStoreInformation> GetUserByBusinessNameAsync(string businessName)
        {
            return await
                _storeContext.BusinessStoreInformation.SingleOrDefaultAsync(u => u.BusinessName == businessName);



        }

   
        

        
        public async Task<bool> StartBusinessRegistrationAsync(BusinessAccountEmailDto emailDto)
        {
            var existingUser = await _storeContext.BusinessUsers
                .FirstOrDefaultAsync(x => x.BusinessEmail == emailDto.Email);


            if (existingUser != null)
            {
                throw new Exception("A business account with this email already exists");
            }

            var state = new BusinessRegistrationState
            {
                Email = emailDto.Email,
                CurrentStep = RegistrationStep.Step1_Identity


            };

            await _cacheService.SetBusinessRegistraionStateAsync(emailDto.Email, state);

            return true;

        }

        public async Task<bool> SetupBusinessAccountAsync(BusinessAccountSetupDto setupDto)
        {
            var state = await _cacheService.GetBusinessRegistrationStateAsync(setupDto.Email);

            if (state == null)
            {
                throw new Exception("The business account registration has not started");
            }

            if (state.CurrentStep != RegistrationStep.Step1_Identity)
                return false;

            if (setupDto.Password != setupDto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            state.FullName = setupDto.FullName;
            state.PasswordHash = await HashBusinessPasswordAsync(setupDto.Password);
            state.CurrentStep = RegistrationStep.Step2_AccountSetup;

            await _cacheService.SetBusinessRegistraionStateAsync(setupDto.Email, state);

            return true;
        }

        public async Task<BusinessUser> CompleteBusinessRegistrationAsync(string email,
            BusinessAccountDetailsDto accountDetailsDto)
        {

            var state = await _cacheService.GetBusinessRegistrationStateAsync(email);

            if (state == null)
                throw new Exception("The business account registration has not started");

            if (state.CurrentStep != RegistrationStep.Step2_AccountSetup)
                throw new Exception("Invalid step order");

            state.BusinessDetails = new BusinessDetailsState
            {



            };


            state.CurrentStep = RegistrationStep.Completed;


            var nameParts = state.FullName.Split(' ', 2);


            var newUser = new BusinessUser
            {
                
                BusinessEmail = state.Email,
                BusinessPhoneNumber = state.BusinessDetails.BusinessPhoneNumber,
                PasswordHash = state.PasswordHash,
                IsBusinessEmailVerified = state.IsEmailVerified,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,

                
                BusinessProfile = new BusinessProfile
                {
                    FirstName = nameParts[0],
                    LastName = nameParts.Length > 1 ? nameParts[1] : "",
                    ReceiveUpdates = state.BusinessDetails.ReceiveUpdates,
                },


                BusinessStoreInformation = new BusinessStoreInformation
                {

                    BusinessName = state.BusinessDetails.BusinessName,
                    BusinessType = state.BusinessDetails.BusinessType,
                    StreetAddress = state.BusinessDetails.StreetAddress,
                    SuiteUnitFloor = state.BusinessDetails.SuiteUnitFloor,
                    City = state.BusinessDetails.City,
                    State = state.BusinessDetails.State,
                    ZipCode = state.BusinessDetails.ZipCode
                    
                },
            };
            
            
            _storeContext.BusinessUsers.Add(newUser);
            await _storeContext.SaveChangesAsync();
            
            await _cacheService.RemoveBusinessRegistrationStateAsync(state.Email);
            
            return newUser;
        }
    
        
        public async Task<BusinessUserTokenResponseDto> BusinessAuthenticateUserAsync(BusinessUserLoginDto userLoginDto)

        {

            BusinessUser businessUser = null;



            if (Regex.IsMatch(userLoginDto.EmailOrPhone, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {


                businessUser = await _storeContext.BusinessUsers.SingleOrDefaultAsync(u =>
                    u.BusinessEmail == userLoginDto.EmailOrPhone);



            }

            else
            {

                businessUser = await _storeContext.BusinessUsers.SingleOrDefaultAsync(u =>
                    u.BusinessPhoneNumber == userLoginDto.EmailOrPhone);

            }




            if (businessUser == null || !await VerifyBusinessPasswordAsync(userLoginDto.Password, businessUser.PasswordHash))

            {

                return null;

            }
            
            
            
            var business = await _storeContext.BusinessStoreInformation
                .FirstOrDefaultAsync(s=> s.BusinessUserId == businessUser.Id);

            
            var businessContact = await _storeContext.BusinessProfiles.SingleOrDefaultAsync(p => p.BusinessUserId == businessUser.Id);
              

            var fullName = businessContact == null
                ? "Business User" :
                string.Join (" ",businessContact.FirstName, businessContact.LastName);

            
            
            var tokenRequest = new TokenRequestDto
            {
                UserId = businessUser.Id,
                Email = businessUser.BusinessEmail,
                Role = UserRole.Business,
                DisplayName = fullName,
                


            };

            var token = _tokenService.GenerateToken(tokenRequest);

            var authResponse = new BusinessUserTokenResponseDto
            {

                UserId = businessUser.Id,
                BusinessName = business?.BusinessName,
                Token = token,


            };

            return authResponse;
        }



        
        
        public async Task<bool> UpdateBusinessStoreInformationAsync(int userId,
            UpdateBusinessStoreInformationDto updateBusinessStoreInformationDto)
        {
            var businessUser = await _storeContext.BusinessUsers.Include(u => u.BusinessStoreInformation)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            if (businessUser == null)
                throw new Exception("Business user not found");

            if (businessUser.BusinessStoreInformation == null)
            {

                businessUser.BusinessStoreInformation = new BusinessStoreInformation   //Was Here
                {

                    BusinessUserId = businessUser.Id

                };

            }
            
            
            _mapper.Map(updateBusinessStoreInformationDto, businessUser.BusinessStoreInformation);
            
            businessUser.UpdatedAt = DateTime.UtcNow;
            
            var result =  await _storeContext.SaveChangesAsync();
            
            return result > 0;
        }

        
        
        public async Task<bool> UpdateBusinessUserAsync(int userId, UpdateBusinessUserDto updateBusinessUserDto)
        {
            // Retrieve the user from the database by ID
            var existingBusinessUser = await _storeContext.BusinessUsers.FindAsync(userId);

            if (existingBusinessUser == null)
            {

                throw new Exception("The business user you are trying to update does not exist");

                // return false;
            }


            existingBusinessUser.BusinessEmail = updateBusinessUserDto.BusinessEmail;
            existingBusinessUser.BusinessPhoneNumber = updateBusinessUserDto.BusinessPhoneNumber;
            existingBusinessUser.UpdatedAt = DateTime.UtcNow;

            // Update the user entity in the database
            _storeContext.BusinessUsers.Update(existingBusinessUser);



            var result = await _storeContext.SaveChangesAsync();

            return result > 0; // Return true if update was successful
        }

        
        public Task<bool> UpdateBusinessEmailAsync(int userId, string newEmail)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateBusinessPasswordAsync(int userId, UpdateBusinessUserPasswordDto userPasswordDto)
        {
           var businessUser = await _storeContext.BusinessUsers.FirstOrDefaultAsync(x => x.Id == userId);
           
           if (businessUser == null)
               throw new Exception("Business user not found");
           
           var isValid = BCrypt.Net.BCrypt.Verify(userPasswordDto.CurrentPassword, businessUser.PasswordHash);
           
           if (!isValid)
               return false;
           
           businessUser.UpdatedAt = DateTime.UtcNow;
           
           var result =  await _storeContext.SaveChangesAsync();
           
           return result > 0;
               
        }


        public Task<bool> UpdateBusinessProfileAsync(int userId, UpdateBusinessProfileDto updateBusinessProfileDto)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> VerifyBusinessPasswordAsync(string enteredPassword, string storedHash)
        {

            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash));


        }

        public Task<bool> IsBusinessEmailAvailableAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsBusinessIdentifierTakenAsync(string identifier)
        {
            var existingUser = await _storeContext.BusinessUsers.FirstOrDefaultAsync(u =>
                u.BusinessEmail == identifier || u.BusinessPhoneNumber == identifier);

            return existingUser != null;
        }



        public async Task<bool> IsBusinessNameTakenAsync(string businessName)
        {
            var existingUsername =
                await _storeContext.BusinessStoreInformation.FirstOrDefaultAsync(x => x.BusinessName == businessName);
            return existingUsername != null;
        }



      
        public Task<BusinessUser> AddBusinessDetailsAsync(int userId, BusinessAccountDetailsDto detailsDto)
        {
            throw new NotImplementedException();
        }

        
        public async Task<string> HashBusinessPasswordAsync(string password)
        {
            var hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));

            return hashedPassword;
        }
        
        public async Task<bool> ResetBusinessPasswordAsync(BusinessUserForgotPasswordDto forgotPasswordDto)
        {

            if (forgotPasswordDto.NewPassword != forgotPasswordDto.ReEnterPassword)

            {
                throw new ArgumentException("The password and confirmation password do not match");

            }
            
            var user = await GetUserByBusinessEmailAsync(forgotPasswordDto.BusinessEmail);

            if (user == null)
            {

                throw new Exception("The user you are looking for does not exist");

            }
            

            var cachedOtp =
                await _cacheService.ValidateOtpAsync(forgotPasswordDto.BusinessEmail, forgotPasswordDto.Otp);

            if (cachedOtp == null)
            {



                return false;
            }

            //Hash new password before updating


            var hashedPassword = await HashBusinessPasswordAsync(forgotPasswordDto.NewPassword);


            user.PasswordHash = hashedPassword;

            user.UpdatedAt = DateTime.UtcNow;


            await _storeContext.SaveChangesAsync();

            await _cacheService.RemoveOtpAsync(forgotPasswordDto.BusinessEmail);



            return true;




        }

        
        

        public async Task<bool> ChangeBusinessPasswordAsync(int userId,
            UpdateBusinessUserPasswordDto userPasswordDto)
        {

            //finds the user to change password
            var existingUser = await _storeContext.BusinessUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false;
            }

            //Verify Current Password
            if (!await VerifyBusinessPasswordAsync(userPasswordDto.CurrentPassword, existingUser.PasswordHash))
            {

                throw new UnauthorizedAccessException("Current password is incorrect");

            }

            existingUser.PasswordHash = await HashBusinessPasswordAsync(userPasswordDto.NewPassword);

            _storeContext.BusinessUsers.Update(existingUser);
            var result = await _storeContext.SaveChangesAsync();
            return result > 0;

        }


        
        
        
        
        
      
        public async Task<bool> DeleteBusinessUserAsync(int userId)
        {
            var user = await _storeContext.BusinessUsers.FindAsync(userId);

            if (user == null)

            {
                return false;

            }

            _storeContext.BusinessUsers.Remove(user);

            var result = await _storeContext.SaveChangesAsync();

            return result > 0;



        }

}
}
