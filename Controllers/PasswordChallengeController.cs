using Amazon_eCommerce_API.Models.DTO_s.UserAccount;
using Amazon_eCommerce_API.Services.Authentication.PasswordChallenge;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordChallengeController : ControllerBase
    {


        private readonly IPasswordChallengeService _passwordChallengeService;

        public PasswordChallengeController(IPasswordChallengeService passwordChallengeService)
        {
            _passwordChallengeService = passwordChallengeService;
        }



        [HttpPost("generate")]

        /// Generates an OTP challenge for login (email or phone)
        public async Task<IActionResult> GenerateOtp([FromBody] UserPasswordChallengeRequestDto requestDto)

        {


            if (requestDto == null || string.IsNullOrEmpty(requestDto.Identifier) || string.IsNullOrEmpty(requestDto.Password))
                return BadRequest("Identifier and password are required.");

            var response = await _passwordChallengeService.GenerateOtpChallengeAsync(requestDto.Identifier, requestDto.Password);

            if (response == null)
                return Unauthorized("Invalid identifier or password");



            return Ok(response);





        }


        [HttpPost("verify")]

        public async Task<IActionResult> VerifyOtp([FromBody] UserPasswordChallengeVerifyDto requestDto)
        {

            if (requestDto == null || string.IsNullOrEmpty(requestDto.PendingAuthId) || string.IsNullOrEmpty(requestDto.Otp))

                return BadRequest("PendingAuthId and OTP are required");



            var isValid = await _passwordChallengeService.VerifyOtpAsync(requestDto.PendingAuthId,requestDto.Otp);


            if (!isValid)
                return Unauthorized("Invalid or expired OTP");


            return Ok( new {message = "OTP verified successfully." });

        }


    }

}
