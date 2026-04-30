using System.Data.Common;
using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Services;
using Amazon_eCommerce_API.Services.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Services.Authentication.Token;
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

        private readonly ICustomerUserService _customerUserService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IPasswordChallengeService _passwordChallengeService;
        private readonly StoreContext _storeContext;

        public CustomerAccountController(ICustomerUserService customerUserService, IMapper mapper, ITokenService tokenService, StoreContext storeContext, IPasswordChallengeService passwordChallengeService)
        {
            _customerUserService = customerUserService;
            _mapper = mapper;
            _tokenService = tokenService;
            _storeContext = storeContext;
            _passwordChallengeService = passwordChallengeService;
        }
        

        [HttpPost("register")]

        
         public async Task<IActionResult> CustomerRegister(CustomerUserRegistrationDto customerUserRegistrationDto)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emailTaken = await _customerUserService.IsCustomerIdentifierTakenAsync(customerUserRegistrationDto.Email);
         

            if (emailTaken) {
                
                return BadRequest($"The customer email address {customerUserRegistrationDto.Email} is already taken.");

            }

            if (!string.IsNullOrEmpty(customerUserRegistrationDto.MobileNumber))
            {
                
                var phoneNumberTaken = await _customerUserService.IsCustomerIdentifierTakenAsync(customerUserRegistrationDto.MobileNumber);
                
                if (phoneNumberTaken) 
                    return BadRequest($"The customer phone number {customerUserRegistrationDto.MobileNumber} is already taken.");
                
            }
           
            
            var customerUser = await _customerUserService.RegisterCustomerUserAsync(customerUserRegistrationDto);
            
            if (customerUser == null)
                return BadRequest("Customer Registration Failed");
            
            //New User is already in database now.
            
            var identifier = customerUserRegistrationDto.Email  ?? customerUserRegistrationDto.MobileNumber;

            

            var otpChallenge = await _passwordChallengeService.GenerateOtpChallengeAsync(
                identifier,
                customerUserRegistrationDto.Password, 
                UserRole.Customer);

        
                if(otpChallenge == null)
                return BadRequest("Failed to send OTP");

            return Ok(new

            {
                message = $"OTP sent to your {otpChallenge.OtpChannel}",
                pendingAuthID = otpChallenge.PendingAuthId,
                destination = otpChallenge.MaskedDestination,
                requiresVerification = true

            });






        }



        [HttpPost("login")]
       
        public async Task<IActionResult> CustomerLogin([FromBody] CustomerUserLoginDto customerUserLoginDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            
            var otpChallenge = await _passwordChallengeService.GenerateOtpChallengeAsync( customerUserLoginDto.EmailOrPhone ,customerUserLoginDto.Password,  UserRole.Customer);


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

            var customerUsers = await _customerUserService.GetAllCustomerUsersAsync();
            

            if (customerUsers == null || !customerUsers.Any())
            {
                return NotFound("No customer accounts found");

            }


            return Ok(customerUsers);
            

        }


        [HttpGet("{id}")]


        public async Task<IActionResult> GetCustomerAccount(int id)
        {


            var customerUser = await _customerUserService.GetAllCustomerUsersAsync();

          


            if (customerUser == null || !customerUser.Any())
            {


                return NotFound($"No Customer found with user ID {id}");

            }

            return Ok(customerUser);




        }



        [HttpGet("email")]

        public async Task<IActionResult> GetCustomerAccountByEmail(string email)
        { 
        
              var customerUser = await _customerUserService.GetUserByCustomerEmailAsync(email);



            if (customerUser == null)
            {


                return NotFound($"No Customer found with email {email}");

            }

            return Ok(customerUser);

        }



        [HttpGet("phoneNumber")]


        public async Task<IActionResult> GetCustomerAccountbyPhoneNumber(string phoneNumber)
        { 
        
            var customerUser = await _customerUserService.GetUserByCustomerPhoneNumberAsync(phoneNumber);


            if (customerUser == null) { 
            
            
            
            return NotFound($"No Customer found with phone number {phoneNumber}");
            
            }


            return Ok(customerUser);
        
        
        }















        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAccountDetails(int id, [FromBody] UpdateSellerUserDto userDto)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fetch the user by ID from the user service
            var customerUser = await _customerUserService.GetUserByCustomerIdAsync(id);

            // Check if the user exists and is a customer
            if (customerUser == null)
            {
                return NotFound("The customer user you are trying to find does not exist ");
            }

            // Map the properties from UserUpdateDto to the customerUser entity
            _mapper.Map(userDto, customerUser);

            // Save changes to the database by calling UpdateUserAsync with the User entity
            var isUpdated = await _customerUserService.UpdateCustomerUserAsync(customerUser.Id, customerUser);

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



        public async Task<IActionResult> UpdateCustomerAccountPassword(int userId, UpdateBusinessUserPasswordDto userPasswordDto)
        {

            if (!ModelState.IsValid) {
            
            return BadRequest(ModelState);
            
            }

            var customerUser = await _customerUserService.GetUserByCustomerIdAsync(userId);


            if (customerUser == null)
            {

                return NotFound("the customer password you are trying to update does not exist ");
            
            
            }


            _mapper.Map(userPasswordDto, customerUser);

            customerUser.PasswordHash = await _customerUserService.HashCustomerPasswordAsync(userPasswordDto.NewPassword);


            var isUpdated = await _customerUserService.UpdateCustomerUserAsync(userId, customerUser);



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

            var customerUser = await _customerUserService.GetUserByCustomerIdAsync(id);


            if (customerUser == null)
            {

                return NotFound("The customer user you are trying to delete does not exist.");
            
            }
       
            var isDeleted = await _customerUserService.DeleteCustomerUserAsync(id);

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












