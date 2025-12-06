using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Users;
using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount;
using Amazon_eCommerce_API.Models.DTO_s.BusinessAccount;
using Amazon_eCommerce_API.Models.Users;
using Amazon_eCommerce_API.Services.Cache;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Amazon_eCommerce_API.Services.Users
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



            CustomerUsers user = null;


            if (Regex.IsMatch(userLoginDto.EmailOrPhone, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {


                user = await _storeContext.CustomerUsers.SingleOrDefaultAsync(u => u.Email == userLoginDto.EmailOrPhone);



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
                Username = user.Username,
                Token = token,


            };

            return authResponse;
        }

       

   
        public async Task<bool> ChangeBusinessPasswordAsync(int userId, BusinessUserPasswordUpdateDto userPasswordUpdateDto)
        {

            //finds the user to change password
            var existingUser = await _storeContext.CustomerUsers.FindAsync(userId);

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

            _storeContext.CustomerUsers.Update(existingUser);
            var result = await _storeContext.SaveChangesAsync();
            return result > 0;

        }






        public async Task<bool> DeleteBusinessUserAsync(int userId)
        {
           var user = await _storeContext.CustomerUsers.FindAsync(userId);

            if (user == null) 
            
            { 
              return false;
                
            }

            _storeContext.CustomerUsers.Remove(user);

            var result = await _storeContext.SaveChangesAsync();

            return result > 0;



        }

        public async Task<IEnumerable<BusinessUsers>> GetAllBusinessUsersAsync()
        {

            return await _storeContext.BusinessUsers.ToListAsync();
        }

        public async Task<BusinessUsers> GetUserByBusinessEmailAsync(string email)
        {
          return await _storeContext.BusinessUsers.SingleOrDefaultAsync(x => x.Email == email);
        }

        public Task<BusinessUsers> GetUserByBusinessIdAsync(int userId)
        {
            return _storeContext.BusinessUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<BusinessUsers> GetUserByBusinessPhoneNumberAsync(string phoneNumber)
        {
            return await _storeContext.BusinessUsers.SingleOrDefaultAsync(u => u.BusinessPhone == phoneNumber);
        }

        public async Task<BusinessUsers> GetUserByBusinessNameAsync(string businessName)
        {
           return await _storeContext.BusinessUsers.SingleOrDefaultAsync(u => u.BusinessName == businessName);



        }

        public async Task<string> HashBusinessPasswordAsync(string password)
        {
            var hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));

            return hashedPassword;
        }

        public async Task<bool> IsBusinessIdentifierTakenAsync(string identifier)
        {
           var existingUser = await _storeContext.BusinessUsers.FirstOrDefaultAsync(u => u.Email == identifier || u.BusinessPhone == identifier);

            return existingUser != null;
        }

      

        public async Task<bool> IsBusinessNameTakenAsync(string businessName)
        {
           var existingUsername = await _storeContext.BusinessUsers.FirstOrDefaultAsync(x => x.BusinessName == businessName);
            return existingUsername != null;
        }


        public async Task<BusinessUsers> RegisterBusinessAccountAsync(BusinessAccountSetupDto setupDto)
        {



            var existingUser = await _storeContext.BusinessUsers
                .FirstOrDefaultAsync(u => u.Email == setupDto.Email);




            if (existingUser == null) {


                throw new Exception("A business account with this email already exists");
            
            }


            if (setupDto.Password != setupDto.ConfirmPassword)
                throw new Exception("Passwords do not match");




            //split full name into first and last name
            var nameParts = setupDto.FullName.Trim().Split(' ',2);
            string firstName = nameParts.Length > 0 ? nameParts[0] : "";
            string lastName = nameParts.Length > 1 ? nameParts[1] : "";


            // Hash password 

            var hashedPassword = await HashBusinessPasswordAsync(setupDto.Password);



            var newBusinessUser = new BusinessUsers
            {

                FirstName = firstName,
                LastName = lastName,
                Email = setupDto.Email,
                PasswordHash = hashedPassword,




                BusinessPhone = null,
                RecieveTextUpdates = false,
                BusinessName = null,
                BusinessType = null,
                StreetAddress = null,
                SuiteOrUnit = null,
                ZipCode = null,
                City = null,
                State = null,


                IsEmailVerified = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,


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

            var updateResult = await UpdateBusinessUserAsync(user.Id, user);


            if (updateResult) {


                await _cacheService.RemoveOtpAsync(forgotPasswordDto.Email);

            }

            return updateResult;




        }

      






        public Task<bool> UpdateBusinessEmailAsync(int userId, string newEmail)
        {
            throw new NotImplementedException();
        }




        public async Task<bool> UpdateBusinessUserAsync(int userId, BusinessUsers businessUser)
        {
            // Retrieve the user from the database by ID
            var existingBusinessUser = await _storeContext.BusinessUsers.FindAsync(userId);

            if (existingBusinessUser == null)
            {
                return false; // Return false or throw an exception if the business user is not found
            }


            existingBusinessUser.FirstName = businessUser.FirstName;
            existingBusinessUser.LastName = businessUser.LastName;
            existingBusinessUser.Email = businessUser.Email;
            existingBusinessUser.BusinessPhone = businessUser.BusinessPhone;
            existingBusinessUser.RecieveTextUpdates = businessUser.RecieveTextUpdates;
            existingBusinessUser.BusinessName = businessUser.BusinessName;
            existingBusinessUser.BusinessType = businessUser.BusinessType;
            existingBusinessUser.StreetAddress = businessUser.StreetAddress;
            existingBusinessUser.SuiteOrUnit = businessUser.SuiteOrUnit;
            existingBusinessUser.ZipCode = businessUser.ZipCode;
            existingBusinessUser.City = businessUser.City;
            existingBusinessUser.State = businessUser.State;
            existingBusinessUser.IsEmailVerified = businessUser.IsEmailVerified;
            existingBusinessUser.UpdatedAt = businessUser.UpdatedAt;



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

        public Task<BusinessUsers> AddBusinessDetailsAsync(int userId, BusinessAccountDetailsDto detailsDto)
        {
            throw new NotImplementedException();
        }
    }
}
