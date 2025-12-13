using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Password;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Authentication;
using Amazon_eCommerce_API.Services;
using Amazon_eCommerce_API.Services.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Services.Users.Customer;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAccountController : ControllerBase
    {

        private readonly ICustomerUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IPasswordChallengeService _passwordChallengeService;
        private readonly StoreContext _storeContext;

        public CustomerAccountController(ICustomerUserService userService, IMapper mapper, ITokenService tokenService, StoreContext storeContext, IPasswordChallengeService passwordChallengeService)
        {
            _userService = userService;
            _mapper = mapper;
            _tokenService = tokenService;
            _storeContext = storeContext;
            _passwordChallengeService = passwordChallengeService;
        }







        [HttpPost("register")]



         public async Task<IActionResult> CustomerRegister(CustomerUserRegistrationDto CustomerUserRegistrationDto)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emailTaken = await _userService.IsCustomerIdentifierTakenAsync(CustomerUserRegistrationDto.Email);
            var usernameTaken = await _userService.IsCustomerUsernameTakenAsync(CustomerUserRegistrationDto.UserName);


            if (emailTaken) {


                return BadRequest($"The customer email address {CustomerUserRegistrationDto.Email} is already taken.");

            }

            if (usernameTaken) {

                return BadRequest($"The customer username {CustomerUserRegistrationDto.UserName} is already taken.");

            }





            string roleName = "Customer";

            var customerUser = await _userService.RegisterCustomerUserAsync(CustomerUserRegistrationDto, roleName);



            if (customerUser == null)
            {

                return BadRequest("Customer Registration Failed");

            }


            //New User is already in database now.


            var otpChallenge = await _passwordChallengeService.GenerateOtpChallengeAsync(customerUser.Email ?? customerUser.PhoneNumber , CustomerUserRegistrationDto.Password);

            // Todo: Send OTP to user's email or phone number for verification



            // add verify otp endpoint to log in and verify identity after registration



            return Ok(new

            {
                message = $"OTP sent to your {otpChallenge.OtpChannel}",
                pendingAuthID = otpChallenge.PendingAuthId,
                destination = otpChallenge.MaskedDestination


            });






        }



        [HttpPost("login")]
       
        public async Task<IActionResult> CustomerLogin([FromBody] BusinessUserLoginDto userLoginDto)
        {

            if (!ModelState.IsValid)
            {


                return BadRequest(ModelState);
            }



       
              var  customerUser = await _storeContext.CustomerUsers
     
          .SingleOrDefaultAsync(u => u.Email == userLoginDto.EmailOrPhone);


            if (customerUser == null) {

                customerUser = await _storeContext.CustomerUsers
.SingleOrDefaultAsync(u => u.PhoneNumber == userLoginDto.EmailOrPhone);


            }





            if (customerUser == null || !await _userService.VerifyCustomerPasswordAsync(userLoginDto.Password, customerUser.PasswordHash))
            {


                return Unauthorized(new { message = "Invalid email or password" });




            }

        


            var otpChallenge = await _passwordChallengeService.GenerateOtpChallengeAsync(customerUser.Email ?? customerUser.PhoneNumber, userLoginDto.Password);


            if (otpChallenge == null)
                return StatusCode(500, new { message = "Invalid Password"});




            return Ok(new

            {
                message = $"OTP sent to your {otpChallenge.OtpChannel}",
                pendingAuthID = otpChallenge.PendingAuthId,
                destination = otpChallenge.MaskedDestination


            });


        }







        [HttpGet]



        public async Task<IActionResult> GetAllCustomerAccounts()
        {


            var customerUsers = await _userService.GetAllCustomerUsersAsync();


       

            if (customerUsers == null || !customerUsers.Any())
            {


                return NotFound("No customer accounts found");

            }


            return Ok(customerUsers);



        }


        [HttpGet("{id}")]


        public async Task<IActionResult> GetCustomerAccount(int id)
        {


            var customerUser = await _userService.GetAllCustomerUsersAsync();

          


            if (customerUser == null || !customerUser.Any())
            {


                return NotFound($"No Customer found with user ID {id}");

            }

            return Ok(customerUser);




        }


        [HttpGet("username")]

        public async Task<IActionResult> GetCustomerAccountByUsername(string username)
        
        {


            var customerUser = await _userService.GetUserByCustomerUsernameAsync(username);


            if (customerUser == null)
            {


                return NotFound($"No Customer found with username {username}");

            }

            return Ok(customerUser);
        
        
        }



        [HttpGet("email")]

        public async Task<IActionResult> GetCustomerAccountByEmail(string email)
        { 
        
              var customerUser = await _userService.GetUserByCustomerEmailAsync(email);



            if (customerUser == null)
            {


                return NotFound($"No Customer found with email {email}");

            }

            return Ok(customerUser);

        }



        [HttpGet("phoneNumber")]


        public async Task<IActionResult> GetCustomerAccountbyPhoneNumber(string phoneNumber)
        { 
        
            var customerUser = await _userService.GetUserByCustomerPhoneNumberAsync(phoneNumber);


            if (customerUser == null) { 
            
            
            
            return NotFound($"No Customer found with phone number {phoneNumber}");
            
            }


            return Ok(customerUser);
        
        
        }















        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAccountDetails(int id, [FromBody] SellerUserUpdateDto userUpdateDto)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fetch the user by ID from the user service
            var customerUser = await _userService.GetUserByCustomerIdAsync(id);

            // Check if the user exists and is a customer
            if (customerUser == null)
            {
                return NotFound("The customer user you are trying to find does not exist ");
            }

            // Map the properties from UserUpdateDto to the customerUser entity
            _mapper.Map(userUpdateDto, customerUser);

            // Save changes to the database by calling UpdateUserAsync with the User entity
            var isUpdated = await _userService.UpdateCustomerUserAsync(customerUser.Id, customerUser);

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



        public async Task<IActionResult> UpdateCustomerAccountPassword(int userId, BusinessUserPasswordUpdateDto userPasswordUpdateDto)
        {

            if (!ModelState.IsValid) {
            
            return BadRequest(ModelState);
            
            }

            var customerUser = await _userService.GetUserByCustomerIdAsync(userId);


            if (customerUser == null)
            {

                return NotFound("the customer password you are trying to update does not exist ");
            
            
            }


            _mapper.Map(userPasswordUpdateDto, customerUser);

            customerUser.PasswordHash = await _userService.HashCustomerPasswordAsync(userPasswordUpdateDto.NewPassword);


            var isUpdated = await _userService.UpdateCustomerUserAsync(userId, customerUser);



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

            var customerUser = await _userService.GetUserByCustomerIdAsync(id);


            if (customerUser == null)
            {

                return NotFound("The customer user you are trying to delete does not exist.");
            
            }
       
            var isDeleted = await _userService.DeleteCustomerUserAsync(id);

            if (!isDeleted)
            {

                return StatusCode(500, "An error occured while attempting to delete the customer user.");

            }

            return Ok( new


                {
            
                   Message = "Customer account deleted successfully."
                 
                }
                
                );
        
        }

    }

}












