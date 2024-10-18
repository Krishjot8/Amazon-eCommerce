using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.Users;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Amazon_eCommerce_API.Services.Users
{
    public class UserService : IUserService

    {
        private readonly StoreContext _storeContext;

        public UserService(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }



        public async Task<User> AuthenticateUserAsync(UserLoginDto userLoginDto)
        {
            var user = await _storeContext.Users.SingleOrDefaultAsync(u => u.Email == userLoginDto.Email);
            if (user == null || !await VerifyPasswordAsync(userLoginDto.Password, user.PasswordHash)) 
            
            { 
            
                  return null;
            
            }

            return user;
        }

       

        public Task<bool> ChangePasswordAsync(int userId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<string> HashPasswordAsync(string password)
        {
            var hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));

            return hashedPassword;
        }

        public Task<bool> IsEmailTakenAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUsernameTakenAsync(string username)
        {
            throw new NotImplementedException();
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



   
        public Task<bool> ResetPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SubscribeToNewsLetterAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateEmailAsync(int userId, string newEmail)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> VerifyPasswordAsync(string enteredPassword, string storedHash)
        {

            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash));


        }

    
    }
}
