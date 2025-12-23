using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Services.Cache;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Amazon_eCommerce_API.Models.DBEntities.Preferences.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Password;

namespace Amazon_eCommerce_API.Services.Users.Customer
{
    public class CustomerUserService : ICustomerUserService

    {
        private readonly StoreContext storeContext;
        private readonly IMapper mapper;
        private readonly ITokenService tokenService;
        private readonly ICacheService cacheService;
        

        public CustomerUserService(StoreContext storeContext, IMapper mapper, ITokenService tokenService, ICacheService cacheService)
        {
            this.storeContext = storeContext;
            this.mapper = mapper;
           this.tokenService = tokenService;
            this.cacheService = cacheService;
        }


        public async Task<CustomerUserTokenResponseDto> CustomerAuthenticateUserAsync(CustomerUserLoginDto userLoginDto)
        {

            CustomerUser user = null;


            if (Regex.IsMatch(userLoginDto.EmailOrPhone, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                user = await storeContext.CustomerUsers.SingleOrDefaultAsync(u => u.EmailAddress == userLoginDto.EmailOrPhone);

            }

            else
            {
                user = await storeContext.CustomerUsers.SingleOrDefaultAsync(u => u.PhoneNumber == userLoginDto.EmailOrPhone);
            }




            if (user == null || !await VerifyCustomerPasswordAsync(userLoginDto.Password, user.PasswordHash)) 
            
            { 
                
                  return null;
            
            }

            var token = tokenService.GenerateToken(user);

            var authResponse = new CustomerUserTokenResponseDto
            {

                UserId = user.Id,
                DisplayName = user.FirstName,
                Token = token,


            };

            return authResponse;
        }

       

   
        public async Task<bool> ChangeCustomerPasswordAsync(int userId, CustomerUserPasswordUpdateDto userPasswordUpdateDto)
        {

            //finds the user to change password
            var existingUser = await storeContext.CustomerUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false;
            }

            //Verify Current Password
            if (!await VerifyCustomerPasswordAsync(userPasswordUpdateDto.CurrentPassword , existingUser.PasswordHash)) 
            {

                throw new UnauthorizedAccessException("Current password is incorrect");       
            
            }
            existingUser.PasswordHash = await HashCustomerPasswordAsync(userPasswordUpdateDto.NewPassword);

            storeContext.CustomerUsers.Update(existingUser);
            var result = await storeContext.SaveChangesAsync();
            return result > 0;

        }






        public async Task<bool> DeleteCustomerUserAsync(int userId)
        {
           var user = await storeContext.CustomerUsers.FindAsync(userId);

            if (user == null) 
            
            { 
              return false;
                
            }

            storeContext.CustomerUsers.Remove(user);

            var result = await storeContext.SaveChangesAsync();

            return result > 0;



        }

        public async Task<IEnumerable<CustomerUser>> GetAllCustomerUsersAsync()
        {

            return await storeContext.CustomerUsers.ToListAsync();
        }

        public async Task<CustomerUser> GetUserByCustomerEmailAsync(string email)
        {
          return await storeContext.CustomerUsers.SingleOrDefaultAsync(x => x.EmailAddress == email);
        }

        public Task<CustomerUser> GetUserByCustomerIdAsync(int userId)
        {
            return storeContext.CustomerUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<CustomerUser> GetUserByCustomerPhoneNumberAsync(string phoneNumber)
        {
            return await storeContext.CustomerUsers.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        
        public async Task<string> HashCustomerPasswordAsync(string password)
        {
            var hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));

            return hashedPassword;
        }

        public async Task<bool> IsCustomerIdentifierTakenAsync(string identifier)
        {
           var existingUser = await storeContext.CustomerUsers.FirstOrDefaultAsync(u => u.EmailAddress == identifier || u.PhoneNumber == identifier);

            return existingUser != null;
        }

      
        

        public async Task<CustomerUser> RegisterCustomerUserAsync(CustomerUserRegistrationDto userRegistrationDto)
        {

            // Hash password 

            var hashedPassword = await HashCustomerPasswordAsync(userRegistrationDto.Password);

            //create new user
            var user = new CustomerUser
            {
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
                DateOfBirth = userRegistrationDto.DateofBirth,
                EmailAddress = userRegistrationDto.Email,
                PhoneNumber = userRegistrationDto.PhoneNumber,
                PasswordHash = hashedPassword, 
                IsEmailVerified = false


            };

            try {


                await storeContext.CustomerUsers.AddAsync(user);
                await storeContext.SaveChangesAsync(); 

                var preferences = new CustomerPreferences
                {
                    CustomerUserId = user.Id,
                    SubscribeToNewsletter = userRegistrationDto.SubscribeToNewsLetter,
                    PreferredLanguage = "en",
                    PreferredCurrency = "USD",
                    ReceivePromotions = false
                };
                
                await storeContext.CustomerPreferences.AddAsync(preferences);
                await storeContext.SaveChangesAsync();
                
            }
            catch (Exception ex)


            {

                throw new Exception("An error occured while registering the user.", ex);
                 
            }

           

            return user;

        }

        

        public async Task<bool> ResetCustomerPasswordAsync(CustomerUserForgotPasswordDto forgotPasswordDto)
        {

            if (forgotPasswordDto.NewPassword != forgotPasswordDto.ReEnterPassword) 
            
            {
                throw new ArgumentException("The password and confirmation password do not match");
            
            }


            var user = await GetUserByCustomerEmailAsync(forgotPasswordDto.Email);

            if (user == null)
            {

                throw new Exception("The user you are looking for does not exist");

            }



            var cachedOtp = await cacheService.ValidateOtpAsync(forgotPasswordDto.Email,forgotPasswordDto.Otp);

            if (cachedOtp == null) {



                return false;
            }

            //Hash new password before updating


            var hashedPassword = await HashCustomerPasswordAsync(forgotPasswordDto.NewPassword);


            user.PasswordHash = hashedPassword;

            var updateResult = await UpdateCustomerUserAsync(user.Id, user);


            if (updateResult) {


                await cacheService.RemoveOtpAsync(forgotPasswordDto.Email);

            }

            return updateResult;




        }



        public async Task<bool> SubscribeToNewsLetterAsync(int userId)
        {
           var existingUser = await storeContext.CustomerPreferences.FindAsync(userId);

            if (existingUser == null)
            {
                return false;
            }
            existingUser.SubscribeToNewsletter = true;   // If you want to allow unsubscribe, pass a parameter


            storeContext.CustomerPreferences.Update(existingUser);
            var result = await storeContext.SaveChangesAsync();
            return result > 0;
        }

        public Task<bool> UpdateCustomerEmailAsync(int userId, string newEmail)
        {
            throw new NotImplementedException();
        }




        public async Task<bool> UpdateCustomerUserAsync(int userId, CustomerUser user)
        {
            // Retrieve the user from the database by ID
            var existingUser = await storeContext.CustomerUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false; // Return false or throw an exception if the user is not found
            }

           
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;  
            existingUser.EmailAddress = user.EmailAddress;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.IsEmailVerified = user.IsEmailVerified;

      
     
            // Update the user entity in the database
            storeContext.CustomerUsers.Update(existingUser);

            var result = await storeContext.SaveChangesAsync();

            return result > 0; // Return true if update was successful
        }

      

        public async Task<bool> VerifyCustomerPasswordAsync(string enteredPassword, string storedHash)
        {

            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash));


        }

    
    }
}
