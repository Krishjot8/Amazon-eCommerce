using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.Users;
using Amazon_eCommerce_API.Services;
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
    public class CustomerAccountController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly StoreContext _storeContext;

        public CustomerAccountController(IUserService userService, IMapper mapper, ITokenService tokenService, StoreContext storeContext)
        {
            _userService = userService;
            _mapper = mapper;
            _tokenService = tokenService;
            _storeContext = storeContext;
        }



      



        [HttpPost("register")]



         public async Task<IActionResult> CustomerRegister(UserRegistrationDto userRegistrationDto)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emailTaken = await _userService.IsEmailTakenAsync(userRegistrationDto.Email);
            var usernameTaken = await _userService.IsUsernameTakenAsync(userRegistrationDto.UserName);


            if (emailTaken) {


                return BadRequest($"The customer email address {userRegistrationDto.Email} is already taken.");

            }

            if (usernameTaken) {

                return BadRequest($"The customer username {userRegistrationDto.UserName} is already taken.");

            }





            string roleName = "Customer";

            var user = await _userService.RegisterUserAsync(userRegistrationDto, roleName);



            if (user == null)
            {

                return BadRequest("Customer Registration Failed");

            }




            return Ok(new
            {
                Message = "Customer Registration Successful",
                UserId = user.Id,  // Assuming your user model has an Id property
                UserName = user.Username  // Returning the username for confirmation
            });


        }



        [HttpPost("login")]
        [Authorize]

        public async Task<IActionResult> CustomerLogin([FromBody] UserLoginDto userLoginDto)
        {

            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }

            var customerUser = await _storeContext.Users
                .Include(u => u.Role)
                .SingleOrDefaultAsync(u => u.Email == userLoginDto.Email);

            if (customerUser == null || !await _userService.VerifyPasswordAsync(userLoginDto.Password, customerUser.PasswordHash))
            {

                return Unauthorized(new { message = "Invalid email or password" });


            }

            if (customerUser.RoleId != 1)
            {

                return Forbid("Only customers are allowed to log in");

            }



            var token = _tokenService.GenerateToken(customerUser);

            var userTokenResponse = new UserTokenResponseDto
            {

                UserId = customerUser.Id,
                Username = customerUser.Username,
                Token = token

            };

            return Ok(userTokenResponse);




        }







        [HttpGet]



        public async Task<IActionResult> GetAllCustomerAccounts()
        {


            var user = await _userService.GetAllUsersAsync();


            var customerusers = user.Where(u => u.RoleId == 1);

            if (customerusers == null || !customerusers.Any())
            {


                return NotFound("the customer account you are looking for does not exist.");

            }


            return Ok(customerusers);



        }


        [HttpGet("{id}")]


        public async Task<IActionResult> GetCustomerAccount(int id)
        {


            var user = await _userService.GetAllUsersAsync();


            var customeruser = user.FirstOrDefault(u => u.Id == id && u.RoleId == 1);

            if (customeruser == null)
            {


                return NotFound($"No Customer found with user ID {id}");

            }

            return Ok(customeruser);




        }


        [HttpGet("username")]

        public async Task<IActionResult> GetCustomerAccountByUsername(string username)
        
        {


            var customerUser = await _userService.GetUserByUsernameAsync(username);

            if (customerUser == null || customerUser.RoleId != 1)
            {

                return NotFound("The customer user you are trying to find doesn't exist or is not a customer"); 
            
            
            }

            return Ok(customerUser);
        
        
        }



        [HttpGet("email")]

        public async Task<IActionResult> GetCustomerAccountByEmail(string email)
        { 
        
              var customerUser = await _userService.GetUserByEmailAsync(email);


            if (customerUser == null || customerUser.RoleId != 1) {



                return NotFound("The customer user you are trying to find does not exist or is not a customer.");
            
            }



            return Ok(customerUser);
        
        }



        [HttpGet("phoneNumber")]


        public async Task<IActionResult> GetCustomerAccountbyPhoneNumber(string phoneNumber)
        { 
        
            var customerUser = await _userService.GetUserByPhoneNumberAsync(phoneNumber);


            if (customerUser == null || customerUser.RoleId != 1) { 
            
            
            
            return NotFound("The customer account you are trying to find does not exist or it is not a customer account.");
            
            }


            return Ok(customerUser);
        
        
        }

































        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAccountDetails(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fetch the user by ID from the user service
            var customerUser = await _userService.GetUserByIdAsync(id);

            // Check if the user exists and is a customer
            if (customerUser == null || customerUser.RoleId != 1)
            {
                return NotFound("The customer user you are trying to find does not exist or is not a customer.");
            }

            // Map the properties from UserUpdateDto to the customerUser entity
            _mapper.Map(userUpdateDto, customerUser);

            // Save changes to the database by calling UpdateUserAsync with the User entity
            var isUpdated = await _userService.UpdateUserAsync(customerUser.Id, customerUser);

            if (!isUpdated)
            {
                return StatusCode(500, "Error updating the user.");
            }

            return Ok(new
            {
                Message = "Customer user account updated successfully."
            });
        }





        [HttpPut("update-password")]



        public async Task<IActionResult> UpdateCustomerAccountPassword(int userId, UserPasswordUpdateDto userPasswordUpdateDto)
        {

            if (!ModelState.IsValid) {
            
            return BadRequest(ModelState);
            
            }

            var customerUser = await _userService.GetUserByIdAsync(userId);


            if (customerUser == null || customerUser.RoleId != 1)
            {

                return NotFound("the customer password you are trying to update does not exist or the user is not a customer");
            
            
            }


            _mapper.Map(userPasswordUpdateDto, customerUser);

            customerUser.PasswordHash = await _userService.HashPasswordAsync(userPasswordUpdateDto.NewPassword);


            var isUpdated = await _userService.UpdateUserAsync(userId, customerUser);



            if (!isUpdated)
            {

                return StatusCode(500,"Error updating the password");
            
            }

            return Ok(new
            {

                Message = "Customer Password Updated successfully."

            });
        }



        [HttpDelete("{id}")]


        public async Task<IActionResult> DeleteCustomerUser(int id) 
        {

            var customerUser = await _userService.GetUserByIdAsync(id);


            if (customerUser == null || customerUser.RoleId != 1)
            {

                return NotFound("The customer user you are trying to delete does not exist or is not a customer.");
            
            }
       
            var isDeleted = await _userService.DeleteUserAsync(id);

            if (!isDeleted)
            {

                return StatusCode(500, "An error occured while attempting to delete the user.");

            }

            return Ok( new


                {
            
                   Message = "Customer account deleted successfully."
                 
                }
                
                );
        
        }

    }

}












