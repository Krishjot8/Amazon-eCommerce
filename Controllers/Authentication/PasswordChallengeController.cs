using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Password;
using Amazon_eCommerce_API.Services;
using Amazon_eCommerce_API.Services.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Services.Users.Customer;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordChallengeController : ControllerBase
    {


        private readonly IPasswordChallengeService _passwordChallengeService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ITokenService _tokenService;

        public PasswordChallengeController(IPasswordChallengeService passwordChallengeService, ICustomerUserService customerUserService, ITokenService tokenService)
        {
            _passwordChallengeService = passwordChallengeService;
            _customerUserService = customerUserService;
            _tokenService = tokenService;
        }
        
        //


        [HttpPost("generate")]

        /// Generates an OTP challenge for login (email or phone)
        public async Task<IActionResult> GenerateOtp([FromBody] CustomerUserPasswordChallengeRequestDto requestDto)

        {


            if (requestDto == null || string.IsNullOrEmpty(requestDto.Identifier) || string.IsNullOrEmpty(requestDto.Password))
                return BadRequest("Identifier and password are required.");

            var response = await _passwordChallengeService.GenerateOtpChallengeAsync(requestDto.Identifier, requestDto.Password);

            if (response == null)
                return Unauthorized("Invalid identifier or password");



            return Ok(response);





        }


        [HttpPost("verify")]

        public async Task<IActionResult> VerifyOtp([FromBody] CustomerUserPasswordChallengeVerifyDto requestDto)
        {

            if (requestDto == null || string.IsNullOrEmpty(requestDto.PendingAuthId) || string.IsNullOrEmpty(requestDto.Otp))

                return BadRequest("PendingAuthId and OTP are required");



            var isValid = await _passwordChallengeService.VerifyOtpAsync(requestDto);


            if (!isValid)
                return Unauthorized("Invalid or expired OTP");


            var user = await _customerUserService.GetCustomerUserByEmailAsync(requestDto.PendingAuthId)
                ?? await _customerUserService.GetCustomerByPhoneNumberAsync(requestDto.PendingAuthId);

            if (user == null)
                return NotFound("User not found");


            var token = _tokenService.GenerateToken(user);


            var userTokenResponse = new CustomerUserTokenResponseDto
            {

                UserId = user.Id,
                Username = user.Username,
                Token = token

            };



            return Ok( new 
            {
                
                message = "OTP verified successfully." ,
                token = userTokenResponse.Token,
                userId = userTokenResponse.UserId,
                username = userTokenResponse.Username,



            });

        }


    }

}
