using System.Text.RegularExpressions;
using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Password;
using Amazon_eCommerce_API.Services.Cache;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Services.Users.Seller
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
            
            SellerUser sellerUser = null;


            if (Regex.IsMatch(sellerUserLoginDto.EmailOrPhone, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {


                sellerUser = await _storeContext.SellerUsers.SingleOrDefaultAsync(u => u.Email == sellerUserLoginDto.EmailOrPhone);
                
            }
            else
            {

                sellerUser = await _storeContext.SellerUsers.SingleOrDefaultAsync(u => u.PhoneNumber == sellerUserLoginDto.EmailOrPhone);
            
            }




            if (sellerUser == null || !await VerifySellerPasswordAsync(sellerUserLoginDto.Password, sellerUser.PasswordHash)) 
            
            { 
            
                  return null;
            
            }

            var token = _tokenService.GenerateToken(sellerUser);

            var authResponse = new SellerUserTokenResponseDto
            {

                UserId = sellerUser.Id,
                Username = sellerUser.Username,
                Token = token,


            };

            return authResponse;
        }

       

   
        public async Task<bool> ChangeSellerPasswordAsync(int userId, SellerUserPasswordUpdateDto userPasswordUpdateDto)
        {

            //finds the user to change password
            var existingUser = await _storeContext.SellerUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false;
            }

            //Verify Current Password
            if (!await VerifySellerPasswordAsync(userPasswordUpdateDto.CurrentPassword , existingUser.PasswordHash)) 
            {

                throw new UnauthorizedAccessException("Current password is incorrect");       
            
            }
            existingUser.PasswordHash = await HashSellerPasswordAsync(userPasswordUpdateDto.NewPassword);

            _storeContext.SellerUsers.Update(existingUser);
            var result = await _storeContext.SaveChangesAsync();
            return result > 0;

        }






        public async Task<bool> DeleteSellerUserAsync(int userId)
        {
           var user = await _storeContext.SellerUsers.FindAsync(userId);

            if (user == null) 
            
            { 
              return false;
                
            }

            _storeContext.SellerUsers.Remove(user);

            var result = await _storeContext.SaveChangesAsync();

            return result > 0;



        }

        public async Task<IEnumerable<SellerUser>> GetAllSellerUsersAsync()
        {

            return await _storeContext.SellerUsers.ToListAsync();
        }

        public async Task<SellerUser> GetUserBySellerEmailAsync(string email)
        {
          return await _storeContext.SellerUsers.SingleOrDefaultAsync(x => x.Email == email);
        }

        public Task<SellerUser> GetUserBySellerIdAsync(int userId)
        {
            return _storeContext.SellerUsers.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<SellerUser> GetUserBySellerPhoneNumberAsync(string phoneNumber)
        {
            return await _storeContext.SellerUsers.SingleOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<SellerUser> GetUserBySellerUsernameAsync(string username)
        {
           return await _storeContext.SellerUsers.SingleOrDefaultAsync(u => u.Username == username);
           
        }

        public async Task<string> HashSellerPasswordAsync(string password)
        {
            var hashedPassword = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));

            return hashedPassword;
        }

        public async Task<bool> IsSellerIdentifierTakenAsync(string identifier)
        {
           var existingUser = await _storeContext.SellerUsers.FirstOrDefaultAsync(u => u.Email == identifier || u.PhoneNumber == identifier);

            return existingUser != null;
        }

        
        public async Task<SellerUser> RegisterSellerUserAsync(SellerUserRegistrationDto userRegistrationDto)
        {

            // Hash password 

            var hashedPassword = await HashSellerPasswordAsync(userRegistrationDto.Password);

          



            //create new user
            var user = new SellerUser
            {
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
                Email = userRegistrationDto.Email,
                PhoneNumber = userRegistrationDto.PhoneNumber,
                PasswordHash = hashedPassword, 
                IsEmailVerified = false


            };

            try {


                await _storeContext.SellerUsers.AddAsync(user);
                await _storeContext.SaveChangesAsync();

            }
            catch (Exception ex)


            {

                throw new Exception("An error occured while registering the user.", ex);
                 
            }

           

            return user;

        }
      

        public async Task<bool> ResetSellerPasswordAsync(SellerUserForgotPasswordDto forgotPasswordDto)
        {

            if (forgotPasswordDto.NewPassword != forgotPasswordDto.ReEnterPassword) 
            
            {
                throw new ArgumentException("The password and confirmation password do not match");
            
            }


            var user = await GetUserBySellerEmailAsync(forgotPasswordDto.Email);

            if (user == null)
            {

                throw new Exception("The user you are looking for does not exist");

            }



            var cachedOtp = await _cacheService.ValidateOtpAsync(forgotPasswordDto.Email,forgotPasswordDto.Otp);

            if (cachedOtp == null) {



                return false;
            }

            //Hash new password before updating


            var hashedPassword = await HashSellerPasswordAsync(forgotPasswordDto.NewPassword);


            user.PasswordHash = hashedPassword;

            var updateResult = await UpdateSellerUserAsync(user.Id, user);


            if (updateResult) {


                await _cacheService.RemoveOtpAsync(forgotPasswordDto.Email);

            }

            return updateResult;




        }

        

        public Task<bool> UpdateSellerEmailAsync(int userId, string newEmail)
        {
            throw new NotImplementedException();
        }




        public async Task<bool> UpdateSellerUserAsync(int userId, SellerUser sellerUser)
        {
            // Retrieve the user from the database by ID
            var existingUser = await _storeContext.SellerUsers.FindAsync(userId);

            if (existingUser == null)
            {
                return false; // Return false or throw an exception if the user is not found
            }

           
            existingUser.FirstName = sellerUser.FirstName;
            existingUser.LastName = sellerUser.LastName;  
            existingUser.Email = sellerUser.Email;
            existingUser.PhoneNumber = sellerUser.PhoneNumber;
            existingUser.IsEmailVerified = sellerUser.IsEmailVerified;

      
     
            // Update the user entity in the database
            _storeContext.SellerUsers.Update(existingUser);

            var result = await _storeContext.SaveChangesAsync();

            return result > 0; // Return true if update was successful
        }

      

        public async Task<bool> VerifySellerPasswordAsync(string enteredPassword, string storedHash)
        {

            return await Task.Run(() => BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash));


        }

    
    }
}
