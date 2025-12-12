using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Users;
using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.DTO_s.SellerAccount;
using Amazon_eCommerce_API.Models.Users;
using Amazon_eCommerce_API.Services.Cache;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount;

namespace Amazon_eCommerce_API.Services.Users
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
            
            SellerUsers user = null;


            if (Regex.IsMatch(sellerUserLoginDto.EmailOrPhone, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {


                user = await _storeContext.SellerUsers.SingleOrDefaultAsync(u => u.Email == sellerUserLoginDto.EmailOrPhone);
                
            }
            else
            {

                user = await _storeContext.SellerUsers.SingleOrDefaultAsync(u => u.PhoneNumber == sellerUserLoginDto.EmailOrPhone);
            
            }




            if (user == null || !await VerifySellerPasswordAsync(sellerUserLoginDto.Password, user.PasswordHash)) 
            
            { 
            
                  return null;
            
            }

            var token = _tokenService.GenerateToken(user);

            var authResponse = new SellerUserTokenResponseDto
            {

                UserId = user.Id,
                Username = user.Username,
                Token = token,


            };

            return authResponse;
        }

       

   
        public async Task<bool> ChangeCustomerPasswordAsync(int userId, BusinessUserPasswordUpdateDto userPasswordUpdateDto)
        {

            //finds the user to change password
            var existingUser = await _storeContext.CustomerUsers.FindAsync(userId);

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

            _storeContext.CustomerUsers.Update(existingUser);
            var result = await _storeContext.SaveChangesAsync();
            return result > 0;

        }






        public async Task<bool> DeleteCustomerUserAsync(int userId)
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

        public async Task<IEnumerable<CustomerUsers>> GetAllCustomerUsersAsync()
        {

            return await _storeContext.CustomerUsers.ToListAsync();
        }

        public async Task<CustomerUsers> GetUserByCustomerEmailAsync(string email)
        {
          return await _storeContext.CustomerUsers.SingleOrDefaultAsync(x => x.Email == email);
        }

        public Task<CustomerUsers> GetUserByCustomerIdAsync(int userId)
        {
            return _storeContext.CustomerUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<CustomerUsers> GetUserByCustomerPhoneNumberAsync(string phoneNumber)
        {
            return await _storeContext.CustomerUsers.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<CustomerUsers> GetUserByCustomerUsernameAsync(string username)
        {
           return await _storeContext.CustomerUsers.SingleOrDefaultAsync(u => u.Username == username);



        }

        public async Task<string> HashCustomerPasswordAsync(string password)
        {
            var hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));

            return hashedPassword;
        }

        public async Task<bool> IsCustomerIdentifierTakenAsync(string identifier)
        {
           var existingUser = await _storeContext.CustomerUsers.FirstOrDefaultAsync(u => u.Email == identifier || u.PhoneNumber == identifier);

            return existingUser != null;
        }

      

        public async Task<bool> IsCustomerUsernameTakenAsync(string username)
        {
           var existingUsername = await _storeContext.CustomerUsers.FirstOrDefaultAsync(x => x.Username == username);
            return existingUsername != null;
        }


        public async Task<CustomerUsers> RegisterCustomerUserAsync(BusinessUserRegistrationDto userRegistrationDto , string roleName)
        {

            // Hash password 

            var hashedPassword = await HashCustomerPasswordAsync(userRegistrationDto.Password);

          



            //create new user
            var user = new CustomerUsers
            {
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
                Username = userRegistrationDto.UserName,
                Email = userRegistrationDto.Email,
                PhoneNumber = userRegistrationDto.PhoneNumber,
                PasswordHash = hashedPassword, 
                SubscribeToNewsLetter = userRegistrationDto.SubscribeToNewsLetter,
                IsEmailVerified = false


            };

            try {


                await _storeContext.CustomerUsers.AddAsync(user);
                await _storeContext.SaveChangesAsync();

            }
            catch (Exception ex)


            {

                throw new Exception("An error occured while registering the user.", ex);
                 
            }

           

            return user;

        }



      

        public async Task<bool> ResetCustomerPasswordAsync(BusinessUserForgotPasswordDto forgotPasswordDto)
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



            var cachedOtp = await _cacheService.ValidateOtpAsync(forgotPasswordDto.Email,forgotPasswordDto.Otp);

            if (cachedOtp == null) {



                return false;
            }

            //Hash new password before updating


            var hashedPassword = await HashCustomerPasswordAsync(forgotPasswordDto.NewPassword);


            user.PasswordHash = hashedPassword;

            var updateResult = await UpdateCustomerUserAsync(user.Id, user);


            if (updateResult) {


                await _cacheService.RemoveOtpAsync(forgotPasswordDto.Email);

            }

            return updateResult;




        }

      





        public async Task<bool> SubscribeToNewsLetterAsync(int userId)
        {
           var existingUser = await _storeContext.CustomerUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false;
            }
            existingUser.SubscribeToNewsLetter = true;   // If you want to allow unsubscribe, pass a parameter


            _storeContext.CustomerUsers.Update(existingUser);
            var result = await _storeContext.SaveChangesAsync();
            return result > 0;
        }

        public Task<bool> UpdateCustomerEmailAsync(int userId, string newEmail)
        {
            throw new NotImplementedException();
        }




        public async Task<bool> UpdateCustomerUserAsync(int userId, CustomerUsers user)
        {
            // Retrieve the user from the database by ID
            var existingUser = await _storeContext.CustomerUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false; // Return false or throw an exception if the user is not found
            }

           
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;  
            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.IsEmailVerified = user.IsEmailVerified;

      
     
            // Update the user entity in the database
            _storeContext.CustomerUsers.Update(existingUser);

            var result = await _storeContext.SaveChangesAsync();

            return result > 0; // Return true if update was successful
        }

      

        public async Task<bool> VerifyCustomerPasswordAsync(string enteredPassword, string storedHash)
        {

            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash));


        }

    
    }
}
