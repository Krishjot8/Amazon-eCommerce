using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.Users;
using Amazon_eCommerce_API.Services.Cache;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Amazon_eCommerce_API.Services.Users
{
    public class UserService : IUserService

    {
        private readonly StoreContext _storeContext;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ICacheService _cacheService;
        public UserService(StoreContext storeContext, IMapper mapper, ITokenService tokenService, ICacheService cacheService)
        {
            _storeContext = storeContext;
            _mapper = mapper;
            _tokenService = tokenService;
            _cacheService = cacheService;
        }



        public async Task<UserTokenResponseDto> AuthenticateUserAsync(UserLoginDto userLoginDto)





        {



            User user = null;


            if (Regex.IsMatch(userLoginDto.EmailOrPhone, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {


                user = await _storeContext.Users.SingleOrDefaultAsync(u => u.Email == userLoginDto.EmailOrPhone);



            }

            else
            {

                user = await _storeContext.Users.SingleOrDefaultAsync(u => u.PhoneNumber == userLoginDto.EmailOrPhone);
            
            }




            if (user == null || !await VerifyPasswordAsync(userLoginDto.Password, user.PasswordHash)) 
            
            { 
            
                  return null;
            
            }

            var token = _tokenService.GenerateToken(user);

            var authResponse = new UserTokenResponseDto
            {

                UserId = user.Id,
                Username = user.Username,
                Token = token,


            };

            return authResponse;
        }

       

   
        public async Task<bool> ChangePasswordAsync(int userId, UserPasswordUpdateDto userPasswordUpdateDto)
        {

            //finds the user to change password
            var existingUser = await _storeContext.Users.FindAsync(userId);

            if (existingUser == null)
            {
                return false;
            }

            //Verify Current Password
            if (!await VerifyPasswordAsync(userPasswordUpdateDto.CurrentPassword , existingUser.PasswordHash)) 
            {

                throw new UnauthorizedAccessException("Current password is incorrect");       
            
            }
            existingUser.PasswordHash = await HashPasswordAsync(userPasswordUpdateDto.NewPassword);

            _storeContext.Users.Update(existingUser);
            var result = await _storeContext.SaveChangesAsync();
            return result > 0;

        }






        public async Task<bool> DeleteUserAsync(int userId)
        {
           var user = await _storeContext.Users.FindAsync(userId);

            if (user == null) 
            
            { 
              return false;
                
            }

            _storeContext.Users.Remove(user);

            var result = await _storeContext.SaveChangesAsync();

            return result > 0;



        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {

            return await _storeContext.Users.ToListAsync();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
          return await _storeContext.Users.SingleOrDefaultAsync(x => x.Email == email);
        }

        public Task<User> GetUserByIdAsync(int userId)
        {
            return _storeContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _storeContext.Users.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
           return await _storeContext.Users.SingleOrDefaultAsync(u => u.Username == username);



        }

        public async Task<string> HashPasswordAsync(string password)
        {
            var hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));

            return hashedPassword;
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
           var existingEmail = await _storeContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            return existingEmail != null;
        }

      

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
           var existingUsername = await _storeContext.Users.FirstOrDefaultAsync(x => x.Username == username);
            return existingUsername != null;
        }

        public async Task<User> RegisterUserAsync(UserRegistrationDto userRegistrationDto , string roleName)
        {

            // Hash password 

            var hashedPassword = await HashPasswordAsync(userRegistrationDto.Password);

            var role = await _storeContext.UserRoles.FirstOrDefaultAsync(r => r.RoleName == roleName);


            if (role == null) {


                throw new Exception("Role not found");
            }


            //create new user
            var user = new User
            {
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
                Username = userRegistrationDto.UserName,
                Email = userRegistrationDto.Email,
                PhoneNumber = userRegistrationDto.PhoneNumber,
                PasswordHash = hashedPassword,
                RoleId = role.Id,
                Role = role


            };

            try {


                await _storeContext.Users.AddAsync(user);
                await _storeContext.SaveChangesAsync();

            }
            catch (Exception ex)


            {

                throw new Exception("An error occured while registering the user.", ex);
                 
            }

           

            return user;

        }



      

        public async Task<bool> ResetPasswordAsync(UserForgotPasswordDto forgotPasswordDto)
        {

            if (forgotPasswordDto.NewPassword != forgotPasswordDto.ReEnterPassword) 
            
            {
                throw new ArgumentException("The password and confirmation password do not match");
            
            }


            var user = await GetUserByEmailAsync(forgotPasswordDto.Email);

            if (user == null)
            {

                throw new Exception("The user you are looking for does not exist");

            }



            var cachedOtp = await _cacheService.ValidateOtpAsync(forgotPasswordDto.Email,forgotPasswordDto.Otp);

            if (cachedOtp == null) {



                return false;
            }

            //Hash new password before updating


            var hashedPassword = await HashPasswordAsync(forgotPasswordDto.NewPassword);


            user.PasswordHash = hashedPassword;

            var updateResult = await UpdateUserAsync(user.Id, user);


            if (updateResult) {


                await _cacheService.RemoveOtpAsync(forgotPasswordDto.Email);

            }

            return updateResult;




        }

      





        public Task<bool> SubscribeToNewsLetterAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEmailAsync(int userId, string newEmail)
        {
            throw new NotImplementedException();
        }




        public async Task<bool> UpdateUserAsync(int userId, User user)
        {
            // Retrieve the user from the database by ID
            var existingUser = await _storeContext.Users.FindAsync(userId);

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
            _storeContext.Users.Update(existingUser);

            var result = await _storeContext.SaveChangesAsync();

            return result > 0; // Return true if update was successful
        }

      

        public async Task<bool> VerifyPasswordAsync(string enteredPassword, string storedHash)
        {

            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash));


        }

    
    }
}
