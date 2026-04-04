using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.Password;
using Amazon_eCommerce_API.Services;
using Amazon_eCommerce_API.Services.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Services.Users.Business;
using Amazon_eCommerce_API.Services.Users.Customer;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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







        [HttpPost("register")]

        public async Task<IActionResult> BusinessRegister(BusinessUserRegistrationDto userRegistrationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var emailTaken = await _businessUserService.IsBusinessIdentifierTakenAsync(userRegistrationDto.Email);
            var  phoneNumberTaken = await _businessUserService.GetUserByBusinessPhoneNumberAsync(userRegistrationDto.UserName);

            if (emailTaken)
            {

                return BadRequest($"The business email address {userRegistrationDto.Email} is already taken.");
            }

            if (phoneNumberTaken)
            {

                return BadRequest($"The business phone number {userRegistrationDto.PhoneNumber
                    } is already taken.");

            }

          


            var user = await _businessUserService.RegisterBusinessAccountAsync(userRegistrationDto);

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
       

        public async Task<IActionResult> BusinessLogin([FromBody] BusinessUserLoginDto userLoginDto) 
        
        
        {

            if (!ModelState.IsValid) { 
            
                return BadRequest(ModelState);
            
            }


            var businessUser = await _storeContext.BusinessUsers.
                SingleOrDefaultAsync(u => u.Email == userLoginDto.EmailOrPhone);

        
            if(businessUser == null || !await _businessUserService.VerifyBusinessPasswordAsync(userLoginDto.Password, businessUser.PasswordHash))
            {


                return Unauthorized(new { message = "Invalid email or password" });




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








        [HttpPut("{id}")]


        public async Task<IActionResult> UpdatBusinessStoreInformation(int id, UpdateBusinessStoreInformationDto storeInformationDto)
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



        public async Task<IActionResult> UpdateBusinessAccountPassword(int userId, BusinessUserPasswordUpdateDto userPasswordUpdateDto)
        {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            
            }
        
          var businessUser = await _userService.GetUserByIdAsync(userId);

            if (businessUser == null ) {


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
