using System.Text.RegularExpressions;
using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Preferences.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Services.Authentication.Token;
using Amazon_eCommerce_API.Services.Cache;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Services.Users.Customer
{
    public class CustomerUserService : ICustomerUserService

    {
        private readonly ICacheService cacheService;
        private readonly IMapper mapper;
        private readonly StoreContext storeContext;
        private readonly ITokenService tokenService;


        public CustomerUserService(StoreContext storeContext, IMapper mapper, ITokenService tokenService,
            ICacheService cacheService)
        {
            this.storeContext = storeContext;
            this.mapper = mapper;
            this.tokenService = tokenService;
            this.cacheService = cacheService;
        }


        public async Task<CustomerUserTokenResponseDto> CustomerAuthenticateUserAsync(CustomerUserLoginDto userLoginDto)
        {
           
            CustomerUser? user = null;


            if (Regex.IsMatch(userLoginDto.EmailOrPhone, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                user = await storeContext.CustomerUsers.SingleOrDefaultAsync(u =>
                    u.EmailAddress == userLoginDto.EmailOrPhone);
            }

            else
            {
                user = await storeContext.CustomerUsers.SingleOrDefaultAsync(u =>
                    u.MobileNumber == userLoginDto.EmailOrPhone);
            }


            if (user == null || !await VerifyCustomerPasswordAsync(userLoginDto.Password, user.PasswordHash))

            {
                return null;
            }


            var tokenRequest = new TokenRequestDto
            {
                UserId = user.Id,
                Email = user.EmailAddress,
                DisplayName = user.FirstName
            };


            var token = tokenService.GenerateToken(tokenRequest);

            var authResponse = new CustomerUserTokenResponseDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            };

            return authResponse;
        }


        public async Task<bool> ChangeCustomerPasswordAsync(int userId,
            UpdateCustomerUserPasswordDto userPasswordUpdateDto)
        {
            //finds the user to change password
            var existingUser = await storeContext.CustomerUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false;
            }

            //Verify Current Password
            if (!await VerifyCustomerPasswordAsync(userPasswordUpdateDto.CurrentPassword, existingUser.PasswordHash))
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
            return await storeContext.CustomerUsers.SingleOrDefaultAsync(u => u.MobileNumber == phoneNumber);
        }


        public async Task<string> HashCustomerPasswordAsync(string password)
        {
            var hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));

            return hashedPassword;
        }

        public async Task<bool> IsCustomerIdentifierTakenAsync(string identifier)
        {
            var existingUser = await storeContext.CustomerUsers.FirstOrDefaultAsync(u =>
                u.EmailAddress == identifier || u.MobileNumber == identifier);

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
                EmailAddress = userRegistrationDto.Email,
                MobileNumber = userRegistrationDto.MobileNumber,
                PasswordHash = hashedPassword,
                IsEmailVerified = false,
           

                CustomerProfile = new CustomerProfile
                     {
                
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
                PhoneNumber = userRegistrationDto.MobileNumber

            },
            
            

            CustomerPreferences = new CustomerPreferences
            {
                
                SubscribeToNewsletter = userRegistrationDto.SubscribeToNewsLetter,
                PreferredLanguage = "en",
                PreferredCurrency = Currency.USD,
                ReceivePromotions = false
            }
            
            };

            try
            {
               await storeContext.CustomerUsers.AddAsync(user);
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
            if (forgotPasswordDto.NewPassword != forgotPasswordDto.ConfirmPassword)

            {
                throw new ArgumentException("The password and confirmation password do not match");
            }


            var user = await GetUserByCustomerEmailAsync(forgotPasswordDto.Email);

            if (user == null)
            {
                throw new Exception("The user you are looking for does not exist");
            }


            var cachedOtp = await cacheService.ValidateOtpAsync(forgotPasswordDto.Email, forgotPasswordDto.Otp);

            if (cachedOtp == null)
            {
                return false;
            }

            //Hash new password before updating


            var hashedPassword = await HashCustomerPasswordAsync(forgotPasswordDto.NewPassword);


            user.PasswordHash = hashedPassword;

            var updateResult = await UpdateCustomerUserAsync(user.Id, user);


            if (updateResult)
            {
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

            existingUser.SubscribeToNewsletter = true; // If you want to allow unsubscribe, pass a parameter


            storeContext.CustomerPreferences.Update(existingUser);
            var result = await storeContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateCustomerEmailAsync(int userId, string newEmail)
        {
            var existingUser = await storeContext.CustomerUsers.FindAsync(userId);

            if (existingUser == null)
            {

                return false;

            }
            
            var isTaken = await storeContext.CustomerUsers.AnyAsync(u => 
                u.EmailAddress == newEmail && u.Id != userId);
            
            if (isTaken)
            {
                throw new Exception("Email is already taken");
            }

            existingUser.EmailAddress = newEmail;
            
            existingUser.IsEmailVerified = false;
            
            storeContext.CustomerUsers.Update(existingUser);
            
            var result = await storeContext.SaveChangesAsync();
            
            return result > 0;
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
            existingUser.MobileNumber = user.MobileNumber;
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