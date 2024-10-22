using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.Users;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Amazon_eCommerce_API.Services.Users
{
    public class UserService : IUserService

    {
        private readonly StoreContext _storeContext;
        private readonly IMapper _mapper;

        public UserService(StoreContext storeContext, IMapper mapper)
        {
            _storeContext = storeContext;
            _mapper = mapper;
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

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {

            return await _storeContext.Users.ToListAsync();
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByIdAsync(int userId)
        {
            return _storeContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
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




        public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto)
        {
            // Retrieve the user from the database by ID
            var user = await _storeContext.Users.FindAsync(userId);

            if (user == null)
            {
                return false; // Return false or throw an exception if the user is not found
            }

            // Map the UserUpdateDto to the User entity (excluding password fields)
            _mapper.Map(userUpdateDto, user);

            // If the user wants to update the password
            if (!string.IsNullOrEmpty(userUpdateDto.NewPassword))
            {
                // Verify the current password using the stored hash
                var passwordValid = await VerifyPasswordAsync(userUpdateDto.CurrentPassword, user.PasswordHash);
                if (!passwordValid)
                {
                    throw new Exception("Current password is incorrect");
                }

                // Hash the new password and update the PasswordHash field
                user.PasswordHash = await HashPasswordAsync(userUpdateDto.NewPassword);
            }

            // Update the user entity in the database
            _storeContext.Users.Update(user);
            var result = await _storeContext.SaveChangesAsync();

            return result > 0; // Return true if update was successful
        }



        public async Task<bool> VerifyPasswordAsync(string enteredPassword, string storedHash)
        {

            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash));


        }

    
    }
}
