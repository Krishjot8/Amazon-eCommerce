﻿using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.Users;

namespace Amazon_eCommerce_API.Services.Users
{
    public interface IUserService
    {


        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User> RegisterUserAsync(UserRegistrationDto userRegistrationDto);

        Task<User> AuthenticateUserAsync(UserLoginDto userLoginDto);

        Task<User> GetUserByIdAsync(int userId);

        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByPhoneNumberAsync(string phoneNumber);

        Task<User> GetUserByUsernameAsync(string username);

        Task<User> UpdateUserAsync(int userId, UserUpdateDto userUpdateDto);

        Task<bool> DeleteUserAsync(int userId);

        Task<bool> SubscribeToNewsLetterAsync(int userId);

        Task<bool> IsEmailTakenAsync(string email);

        Task<bool> IsUsernameTakenAsync(string username);

        Task<bool> VerifyPasswordAsync(string enteredPassword, byte[] storedHash, byte[] storedSalt);

        Task<bool> ChangePasswordAsync(int userId, string newPassword);

        Task<bool> UpdateEmailAsync(int userId, string newEmail);

        Task<bool> ResetPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    }
}