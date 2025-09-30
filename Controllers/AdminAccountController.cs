using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.Users;
using Amazon_eCommerce_API.Services;
using Amazon_eCommerce_API.Services.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Services.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccountController : ControllerBase
    {
//hello
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IPasswordChallengeService _passwordChallengeService;
        private readonly  StoreContext _storeContext;


        public AdminAccountController(IUserService userService, IMapper mapper, StoreContext storeContext, ITokenService tokenService, IPasswordChallengeService passwordChallengeService)
        {
            _userService = userService;
            _mapper = mapper;
            _storeContext = storeContext;
            _tokenService = tokenService;
            _passwordChallengeService = passwordChallengeService;
        }







        [HttpPost("register")]

        public async Task<IActionResult> AdminRegister(UserRegistrationDto userRegistrationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var emailTaken = await _userService.IsEmailTakenAsync(userRegistrationDto.Email);
            var usernameTaken = await _userService.IsUsernameTakenAsync(userRegistrationDto.UserName);

            if (emailTaken)
            {

                return BadRequest($"The admin email address {userRegistrationDto.Email} is already taken.");
            }

            if (usernameTaken)
            {

                return BadRequest($"The admin username {userRegistrationDto.UserName} is already taken.");

            }

            string roleName = "Admin";


            var user = await _userService.RegisterUserAsync(userRegistrationDto, roleName);

            if (user == null)
            {

                return BadRequest("Admin Registration Failed");

            }

            return Ok(new
            {
                Message = "Admin Registration Successful",
                UserId = user.Id,  // Assuming your user model has an IdD property
                UserName = user.Username  // Returning the username for confirmation
            });


        }



        [HttpPost("login")]
       

        public async Task<IActionResult> AdminLogin([FromBody] UserLoginDto userLoginDto) 
        
        
        {

            if (!ModelState.IsValid) { 
            
                return BadRequest(ModelState);
            
            }


            var adminUser = await _storeContext.Users.Include
                (u => u.Role).SingleOrDefaultAsync(u => u.Email == userLoginDto.EmailOrPhone);

        
            if(adminUser == null || !await _userService.VerifyPasswordAsync(userLoginDto.Password, adminUser.PasswordHash))
            {


                return Unauthorized(new { message = "Invalid email or password" });




            }

            if(adminUser.RoleId != 2)
            {

                return Forbid("Only Admins are allowed to Log in");


            }



            var otpChallenge = await _passwordChallengeService.GenerateOtpChallengeAsync(adminUser.Email ?? adminUser.PhoneNumber, userLoginDto.Password);


            if (otpChallenge == null)
                return StatusCode(500, new { message = "Invalid Password" });




            return Ok(new

            {
                message = $"OTP sent to your {otpChallenge.OtpChannel}",
                pendingAuthID = otpChallenge.PendingAuthId,
                destination = otpChallenge.MaskedDestination


            });


        }






        [HttpGet]

        public async Task<IActionResult> GetAllAdminAccounts()
        {

            var users = await _userService.GetAllUsersAsync();

            var adminUsers = users.Where(u => u.RoleId == 2).ToList();

            if (adminUsers == null || !adminUsers.Any())
            {
                return NotFound("No admins were found.");
            }

            return Ok(adminUsers);

        }




        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAdminAccount(int Id)
        {
            var users = await _userService.GetAllUsersAsync();

            // Find user by ID and ensure their RoleId is 2 (Admin)
            var adminUser = users.FirstOrDefault(u => u.Id == Id && u.RoleId == 2);

            if (adminUser == null)
            {
                return NotFound($"No admin found with user ID {Id}.");
            }

            return Ok(adminUser); // Return the admin user details
        }


        [HttpGet("username")]



        public async Task<IActionResult> GetAdminAccountByUsername(string username)
        { 
        

            var adminUser = await _userService.GetUserByUsernameAsync(username);
        
        
            if(adminUser == null || adminUser.RoleId !=2)
            {

                return NotFound("The admin account you are trying to get does not exist or is not an admin");


            }

            return Ok(adminUser);
        }

       
        
        
        
        
        
        [HttpGet("email")]

        public async Task<IActionResult>GetAdminAccountByEmail(string email)
        {

            var adminUser = await _userService.GetUserByEmailAsync(email);


            if(adminUser == null || adminUser.RoleId !=2)
            {


                return NotFound("The admin account you are trying to get does not exist or is not an admin");
                
                
                
                }


            return Ok(adminUser);

        }






        
        [HttpGet("phoneNumber")]

        public async Task<IActionResult>GetAdminAccountByPhoneNumber(string phoneNumber)
        {

            var adminUser = await _userService.GetUserByPhoneNumberAsync(phoneNumber);


            if(adminUser == null || adminUser.RoleId!=2)
            {


                return NotFound("The admin account you are trying to get does not exist or is not an admin");


            }



            return Ok(adminUser);
        }








        [HttpPut("{id}")]


        public async Task<IActionResult> UpdateAdminAccountDetails(int id, UserUpdateDto userUpdateDto)
        {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);

            }


            var adminuser = await _userService.GetUserByIdAsync(id);

            if (adminuser == null || adminuser.RoleId != 2) {

                return NotFound("The admin user you are trying to update does not exist or is not an Admin");

            }



            _mapper.Map(userUpdateDto, adminuser);

            var isUpdated = await _userService.UpdateUserAsync(adminuser.Id, adminuser);

            if (!isUpdated) {



                return StatusCode(500, "Error updating the admin user");
            }

            return Ok(new
            {

                Message = "Admin user account updated successfully."

            }
         
            ); 
        
        
        }




        [HttpPut("update-password")]



        public async Task<IActionResult> UpdateAdminAccountPassword(int userId, UserPasswordUpdateDto userPasswordUpdateDto)
        {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            
            }
        
          var adminUser = await _userService.GetUserByIdAsync(userId);

            if (adminUser == null || adminUser.RoleId != 2) {


                return NotFound("The admin Password you are trying to update does not exist or is not an admin.");
            
            }

            _mapper.Map(userPasswordUpdateDto,adminUser);


            adminUser.PasswordHash = await _userService.HashPasswordAsync(userPasswordUpdateDto.NewPassword);


            var isUpdated = await _userService.UpdateUserAsync(userId, adminUser);

            if (!isUpdated) {

                return StatusCode(500, "Error updating the admin password.");
            
            }

            return Ok(new
            { 
            Message = "Admin password updated successfully."
               
            }
                
                );

        }




        [HttpDelete("{id}")]


        public async Task<IActionResult> DeleteAdminAccount(int id)
        { 
        
        var adminUser = await _userService.GetUserByIdAsync(id);


            if (adminUser == null || adminUser.RoleId != 2) {
            
                return NotFound("The admin account you are trying to delete does not exist or it is not a admin account.");         
            
            }


            var isDeleted = await _userService.DeleteUserAsync(id);

            if (!isDeleted) 
            
            {

                return StatusCode(500, "Error deleting Admin User");
            
            }


            return Ok(new
            {
               Message = "Admin user account deleted successfully."
            }
            );
        
        
        }




    }


    



}
