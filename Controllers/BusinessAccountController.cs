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
    public class BusinessAccountController : ControllerBase
    {
//hello
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IPasswordChallengeService _passwordChallengeService;
        private readonly  StoreContext _storeContext;


        public BusinessAccountController(IUserService userService, IMapper mapper, StoreContext storeContext, ITokenService tokenService, IPasswordChallengeService passwordChallengeService)
        {
            _userService = userService;
            _mapper = mapper;
            _storeContext = storeContext;
            _tokenService = tokenService;
            _passwordChallengeService = passwordChallengeService;
        }







        [HttpPost("register")]

        public async Task<IActionResult> BusinessRegister(UserRegistrationDto userRegistrationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var emailTaken = await _userService.IsIdentifierTakenAsync(userRegistrationDto.Email);
            var usernameTaken = await _userService.IsUsernameTakenAsync(userRegistrationDto.UserName);

            if (emailTaken)
            {

                return BadRequest($"The business email address {userRegistrationDto.Email} is already taken.");
            }

            if (usernameTaken)
            {

                return BadRequest($"The business username {userRegistrationDto.UserName} is already taken.");

            }

            string roleName = "Business";


            var user = await _userService.RegisterUserAsync(userRegistrationDto, roleName);

            if (user == null)
            {

                return BadRequest("Business Registration Failed");

            }

            return Ok(new
            {
                Message = "Business Registration Successful",
                UserId = user.Id,  // Assuming your user model has an IdD property
                UserName = user.Username  // Returning the username for confirmation
            });


        }



        [HttpPost("login")]
       

        public async Task<IActionResult> BusinessLogin([FromBody] UserLoginDto userLoginDto) 
        
        
        {

            if (!ModelState.IsValid) { 
            
                return BadRequest(ModelState);
            
            }


            var businessUser = await _storeContext.Users.Include
                (u => u.Role).SingleOrDefaultAsync(u => u.Email == userLoginDto.EmailOrPhone);

        
            if(businessUser == null || !await _userService.VerifyPasswordAsync(userLoginDto.Password, businessUser.PasswordHash))
            {


                return Unauthorized(new { message = "Invalid email or password" });




            }

            if(businessUser.RoleId != 2)
            {

                return Forbid("Only business users are allowed to Log in");


            }



            var otpChallenge = await _passwordChallengeService.GenerateOtpChallengeAsync(businessUser.Email ?? businessUser.PhoneNumber, userLoginDto.Password);


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

        public async Task<IActionResult> GetAllBusinessAccounts()
        {

            var users = await _userService.GetAllUsersAsync();

            var businessUsers = users.Where(u => u.RoleId == 2).ToList();

            if (businessUsers == null || !businessUsers.Any())
            {
                return NotFound("No business accounts were found.");
            }

            return Ok(businessUsers);

        }




        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBusinessAccount(int Id)
        {
            var users = await _userService.GetAllUsersAsync();

            // Find user by ID and ensure their RoleId is 2 (Admin)
            var businessUser = users.FirstOrDefault(u => u.Id == Id && u.RoleId == 2);

            if (businessUser == null)
            {
                return NotFound($"No admin found with user ID {Id}.");
            }

            return Ok(businessUser); // Return the admin user details
        }


        [HttpGet("username")]



        public async Task<IActionResult> GetBusinessAccountByUsername(string username)
        { 
        

            var businessUser = await _userService.GetUserByUsernameAsync(username);


            if(businessUser == null || businessUser.RoleId !=2)
            {

                return NotFound("The business account you are trying to get does not exist or is not a business user");


            }

            return Ok(businessUser);
        }

       
        
        
        
        
        
        [HttpGet("email")]

        public async Task<IActionResult>GetBusinessAccountByEmail(string email)
        {

            var businessUser = await _userService.GetUserByEmailAsync(email);


            if(businessUser == null || businessUser.RoleId !=2)
            {


                return NotFound("The business account you are trying to get does not exist or is not a business user");
                
                
                
                }


            return Ok(businessUser);

        }






        
        [HttpGet("phoneNumber")]

        public async Task<IActionResult>GetBusinessAccountByPhoneNumber(string phoneNumber)
        {

            var businessUser = await _userService.GetUserByPhoneNumberAsync(phoneNumber);


            if(businessUser == null || businessUser.RoleId!=2)
            {


                return NotFound("The business account you are trying to get does not exist or is not a business user");


            }



            return Ok(businessUser);
        }








        [HttpPut("{id}")]


        public async Task<IActionResult> UpdatBusinessAccountDetails(int id, UserUpdateDto userUpdateDto)
        {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);

            }


            var businessUser = await _userService.GetUserByIdAsync(id);

            if (businessUser == null || businessUser.RoleId != 2) {

                return NotFound("The business account you are trying to update does not exist or is not a business user");

            }



            _mapper.Map(userUpdateDto, businessUser);

            var isUpdated = await _userService.UpdateUserAsync(businessUser.Id, businessUser);

            if (!isUpdated) {



                return StatusCode(500, "Error updating the business user");
            }

            return Ok(new
            {

                Message = "Business user account updated successfully."

            }
         
            ); 
        
        
        }




        [HttpPut("update-password")]



        public async Task<IActionResult> UpdateBusinessAccountPassword(int userId, UserPasswordUpdateDto userPasswordUpdateDto)
        {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            
            }
        
          var businessUser = await _userService.GetUserByIdAsync(userId);

            if (businessUser == null || businessUser.RoleId != 2) {


                return NotFound("The business password you are trying to update does not exist or is not an business user.");
            
            }

            _mapper.Map(userPasswordUpdateDto, businessUser);


            businessUser.PasswordHash = await _userService.HashPasswordAsync(userPasswordUpdateDto.NewPassword);


            var isUpdated = await _userService.UpdateUserAsync(userId, businessUser);

            if (!isUpdated) {

                return StatusCode(500, "Error updating the business user password.");
            
            }

            return Ok(new
            { 
            Message = "Business user password updated successfully."
               
            }
                
                );

        }




        [HttpDelete("{id}")]


        public async Task<IActionResult> DeleteBusinessAccount(int id)
        { 
        
        var businessUser = await _userService.GetUserByIdAsync(id);


            if (businessUser == null || businessUser.RoleId != 2) {
            
                return NotFound("The business account you are trying to delete does not exist or it is not a business user.");         
            
            }


            var isDeleted = await _userService.DeleteUserAsync(id);

            if (!isDeleted) 
            
            {

                return StatusCode(500, "Error deleting business user");
            
            }


            return Ok(new
            {
               Message = "Business user account deleted successfully."
            }
            );
        
        
        }




    }


    



}
