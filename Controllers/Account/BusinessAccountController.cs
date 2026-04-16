using System.Data.Common;
using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Password;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Services;
using Amazon_eCommerce_API.Services.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Services.Authentication.Token;
using Amazon_eCommerce_API.Services.Users.Business;
using Amazon_eCommerce_API.Services.Users.Customer;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amazon_eCommerce_API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessAccountController : ControllerBase
    {
        //hello
        private readonly IBusinessUserService _businessUserService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IPasswordChallengeService _passwordChallengeService;
        private readonly  StoreContext _storeContext;


        public BusinessAccountController(IBusinessUserService businessUserService,IMapper mapper, StoreContext storeContext, ITokenService tokenService, IPasswordChallengeService passwordChallengeService)
        {
            _businessUserService = businessUserService;
            _mapper = mapper;
            _storeContext = storeContext;
            _tokenService = tokenService;
            _passwordChallengeService = passwordChallengeService;
           
        }
        


        [HttpPost("register-email")]

        public async Task<IActionResult> RegisterEmail(BusinessAccountEmailDto dto)
        {
            
            
            var result = await _businessUserService.StartBusinessRegistrationAsync(dto);

            return result
                ? Ok ("Email registered")
                    : BadRequest ("Failed to register email");
            
        }

        [HttpPost("register-setup")]

        public async Task<IActionResult> RegisterSetup([FromBody] BusinessAccountSetupDto dto)
        {
            
            var result = await _businessUserService.SetupBusinessAccountAsync(dto);
            
            return result
                ? Ok ("Setup complete")
                : BadRequest ("Setup failed");
            
        }

        [HttpPost("register-details/{email}")]

        public async Task<IActionResult> RegisterDetails( string email, [FromBody] BusinessAccountDetailsDto dto)
        {

            var user = await _businessUserService.CompleteBusinessRegistrationAsync(email, dto);
            
            return Ok(new{ message = "Registration complete", userId = user.Id });
        }








        [HttpPost("login")]
       

        public async Task<IActionResult> BusinessLogin([FromBody] BusinessUserLoginDto userLoginDto) 
        
        
        {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var otpChallenge = await _passwordChallengeService.GenerateOtpChallengeAsync(userLoginDto.EmailOrPhone ,userLoginDto.Password,  UserRole.Business);
            
      

            if (otpChallenge == null)
                return Unauthorized(new { message = "Invalid email or Password" });

            

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

            var businessUsers = await _businessUserService.GetAllBusinessUsersAsync();

   

            if (businessUsers == null)
            {
                return NotFound("No business accounts were found.");
            }

            return Ok(businessUsers);

        }




        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBusinessAccount(int Id)
        {
            var users = await _businessUserService.GetAllBusinessUsersAsync();

            // Find user by ID and ensure their RoleId is 2 (Admin)
            var businessUser = users.FirstOrDefault(u => u.Id == Id);

            if (businessUser == null)
            {
                return NotFound($"No business user found with user ID {Id}.");
            }

            return Ok(businessUser); // Return the admin user details
        }


        
        
        
        
        [HttpGet("email")]

        public async Task<IActionResult>GetBusinessAccountByEmail(string email)
        {

            var businessUser = await _businessUserService.GetUserByBusinessEmailAsync(email);


            if(businessUser == null)
            {


                return NotFound("The business account you are trying to get does not exist or is not a business user");
                
        }


            return Ok(businessUser);

        }
        


        
        [HttpGet("phoneNumber")]

        public async Task<IActionResult>GetBusinessAccountByPhoneNumber(string phoneNumber)
        {

            var businessUser = await _businessUserService.GetUserByBusinessPhoneNumberAsync(phoneNumber);


            if(businessUser == null )
            {


                return NotFound("The business account you are trying to get does not exist or is not a business user");


            }



            return Ok(businessUser);
        }


        



        [HttpPut("{id}/store")]
        
        public async Task<IActionResult> UpdateBusinessStoreInformation(int id,[FromBody] UpdateBusinessStoreInformationDto storeInformationDto)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var result = await _businessUserService.UpdateBusinessStoreInformationAsync(id, storeInformationDto);
            
        

            if (!result) {



                return StatusCode(500, "Error updating the business store information");
            }

            return Ok(new
            {

                Message = "Business Store Information updated successfully."

            }
         
            ); 
        
        
        }




        [HttpPut("{id}/update-password")]
        public async Task<IActionResult> UpdateBusinessAccountPassword(int userId, [FromBody] UpdateBusinessUserPasswordDto userPasswordDto)
        {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            var result = await _businessUserService.UpdateBusinessPasswordAsync(userId, userPasswordDto);

            if (!result) {


                return BadRequest("Password update Failed.");
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
        
        var businessUser = await _businessUserService.GetUserByBusinessIdAsync(id);


            if (businessUser == null) {
            
                return NotFound("The business account you are trying to delete does not exist.");         
            
            }


            var isDeleted = await _businessUserService.DeleteBusinessUserAsync(id);

            if (!isDeleted) 
            
            {

                return StatusCode(500, "Error deleting business account");
            
            }


            return Ok(new
            {
               Message = "Business user account deleted successfully."
            }
            );
        
        
        }




    }


    



}
