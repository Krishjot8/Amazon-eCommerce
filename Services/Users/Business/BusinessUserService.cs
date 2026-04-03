using System.Text.RegularExpressions;
using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Password;
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
        public BusinessUserService(StoreContext storeContext, IMapper mapper, ITokenService tokenService, ICacheService cacheService)
        {
            _storeContext = storeContext;
            _mapper = mapper;
            _tokenService = tokenService;
            _cacheService = cacheService;
        }



        public async Task<BusinessUserTokenResponseDto> BusinessAuthenticateUserAsync(BusinessUserLoginDto userLoginDto)
        
        {
            
            CustomerUser user = null;


            if (Regex.IsMatch(userLoginDto.EmailOrPhone, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {


                user = await _storeContext.CustomerUsers.SingleOrDefaultAsync(u => u.EmailAddress == userLoginDto.EmailOrPhone);



            }

            else
            {

                user = await _storeContext.CustomerUsers.SingleOrDefaultAsync(u => u.PhoneNumber == userLoginDto.EmailOrPhone);
            
            }




            if (user == null || !await VerifyBusinessPasswordAsync(userLoginDto.Password, user.PasswordHash)) 
            
            { 
            
                  return null;
            
            }

            var token = _tokenService.GenerateToken(user);

            var authResponse = new BusinessUserTokenResponseDto
            {

                UserId = user.Id,
                DisplayName = user.FirstName,
                Token = token,


            };

            return authResponse;
        }

       

   
        public async Task<bool> ChangeBusinessPasswordAsync(int userId, BusinessUserPasswordUpdateDto userPasswordUpdateDto)
        {

            //finds the user to change password
            var existingUser = await _storeContext.BusinessUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false;
            }

            //Verify Current Password
            if (!await VerifyBusinessPasswordAsync(userPasswordUpdateDto.CurrentPassword , existingUser.PasswordHash)) 
            {

                throw new UnauthorizedAccessException("Current password is incorrect");       
            
            }
            existingUser.PasswordHash = await HashBusinessPasswordAsync(userPasswordUpdateDto.NewPassword);

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
           return await _storeContext.BusinessStoreInformation.SingleOrDefaultAsync(u => u.BusinessName == businessName);



        }

        public async Task<string> HashBusinessPasswordAsync(string password)
        {
            var hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));

            return hashedPassword;
        }

        public async Task<bool> IsBusinessIdentifierTakenAsync(string identifier)
        {
           var existingUser = await _storeContext.BusinessUsers.FirstOrDefaultAsync(u => u.BusinessEmail == identifier || u.BusinessPhoneNumber == identifier);

            return existingUser != null;
        }

      

        public async Task<bool> IsBusinessNameTakenAsync(string businessName)
        {
           var existingUsername = await _storeContext.BusinessStoreInformation.FirstOrDefaultAsync(x => x.BusinessName == businessName);
            return existingUsername != null;
        }


        public async Task<BusinessUser> RegisterBusinessAccountAsync(BusinessAccountSetupDto setupDto, 
            BusinessAccountDetailsDto accountDetailsDto)
        {

            var existingUser = await _storeContext.BusinessUsers
                .FirstOrDefaultAsync(u => u.BusinessEmail.ToLower() == setupDto.Email.ToLower());
            

            if (existingUser != null) {

                throw new Exception("A business account with this email already exists");
            
            }
            
            if (setupDto.Password != setupDto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            
            //split full name into first and last name
          //  var nameParts = setupDto.FullName.Trim().Split(' ',2);
            
            
            var fullName = setupDto.FullName?.Trim() ?? "";
            var nameParts = fullName.Split(' ',2);
            
            string firstName = nameParts.Length > 0 ? nameParts[0] : "";
            string lastName = nameParts.Length > 1 ? nameParts[1] : "";


            // Hash password 

            var hashedPassword = await HashBusinessPasswordAsync(setupDto.Password);



            var newBusinessUser = new BusinessUser
            {

               
                BusinessEmail = setupDto.Email,
                BusinessPhoneNumber = accountDetailsDto.BusinessPhoneNumber,
                PasswordHash = hashedPassword,
                IsBusinessEmailVerified = false,
                IsBusinessPhoneVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,


            };

            newBusinessUser.BusinessProfile = new BusinessProfile
            {
                FirstName = firstName,
                LastName = lastName,
                ReceiveUpdates = accountDetailsDto.ReceiveUpdates
            };


            newBusinessUser.BusinessStoreInformation = new BusinessStoreInformation
            {

                BusinessName = accountDetailsDto.BusinessName,
                BusinessType = accountDetailsDto.BusinessType,
                StreetAddress = accountDetailsDto.StreetAddress,
                SuiteUnitFloor = accountDetailsDto.SuiteUnitFloor,
                City = accountDetailsDto.City,
                State = accountDetailsDto.State,
                ZipCode = accountDetailsDto.ZipCode
                
                
                
            };
            
            

         
         _storeContext.BusinessUsers.Add(newBusinessUser);
            await _storeContext.SaveChangesAsync();

            return newBusinessUser;

        }



      

        public async Task<bool> ResetBusinessPasswordAsync(BusinessUserForgotPasswordDto forgotPasswordDto)
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


            var hashedPassword = await HashBusinessPasswordAsync(forgotPasswordDto.NewPassword);


            user.PasswordHash = hashedPassword;

            user.UpdatedAt = DateTime.UtcNow;
          

           await _storeContext.SaveChangesAsync();

                await _cacheService.RemoveOtpAsync(forgotPasswordDto.Email);



            return true;




        }

      






        public Task<bool> UpdateBusinessEmailAsync(int userId, string newEmail)
        {
            throw new NotImplementedException();
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

      

        public async Task<bool> VerifyBusinessPasswordAsync(string enteredPassword, string storedHash)
        {

            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash));


        }

        public Task<bool> IsBusinessEmailAvailableAsync(string email)
        {
            throw new NotImplementedException();
        }

   
        public Task<BusinessUser> AddBusinessDetailsAsync(int userId, BusinessAccountDetailsDto detailsDto)
        {
            throw new NotImplementedException();
        }
    }
}
