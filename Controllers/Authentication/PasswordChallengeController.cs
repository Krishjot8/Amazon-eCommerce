using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Services.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Services.Authentication.Token;
using Amazon_eCommerce_API.Services.Authentication.UserResolver;
using Amazon_eCommerce_API.Services.Users.Customer;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordChallengeController : ControllerBase
    {


        private readonly IPasswordChallengeService _passwordChallengeService;
        private readonly IUserResolverService _userResolverService;
        private readonly ICustomerUserService _customerUserService;
        private readonly ITokenService _tokenService;
        private readonly StoreContext _storeContext;

        public PasswordChallengeController(IPasswordChallengeService passwordChallengeService,
            IUserResolverService userResolverService, ITokenService tokenService, ICustomerUserService customerUserService, StoreContext storeContext)
        {
            _passwordChallengeService = passwordChallengeService;
            _userResolverService = userResolverService;
            _tokenService = tokenService;
            _customerUserService = customerUserService;
            _storeContext = storeContext;
        }

        //


        [HttpPost("generate")]

        // Generates an OTP challenge for login (email or phone)
        
        public async Task<IActionResult> GenerateOtp([FromBody] PasswordChallengeRequestDto requestDto)

        {


            if (requestDto == null || string.IsNullOrEmpty(requestDto.EmailOrPhone) ||
                string.IsNullOrEmpty(requestDto.Password))
                return BadRequest("Identifier and password are required.");

            var response = await _passwordChallengeService.GenerateOtpChallengeAsync(requestDto.EmailOrPhone,
                requestDto.Password, requestDto.Role);

            if (response == null)
                return Unauthorized("Invalid identifier or password");



            return Ok(response);





        }


        [HttpPost("verify")]

        public async Task<IActionResult> VerifyOtp([FromBody] PasswordChallengeVerifyDto requestDto)
        {

            if (requestDto == null || string.IsNullOrEmpty(requestDto.PendingAuthId)
                                   || string.IsNullOrEmpty(requestDto.Otp))

                return BadRequest("PendingAuthId and OTP are required");


            var isValid = await _passwordChallengeService.VerifyOtpAsync(requestDto);

            if (!isValid)
                return Unauthorized("Invalid or expired OTP");

            
           

            var user = await _userResolverService.ResolveUserAsync
                (requestDto.PendingAuthId, 
                    requestDto.Role);

            if (user == null)
                return NotFound("User not found");

            int userId = 0;
            string displayName = "";
            string? storeName = null;


            switch (requestDto.Role)
            {

                case UserRole.Customer:
                {
                    var customer = (CustomerUser)user;
                    
                    customer.IsEmailVerified = true;
                    
                    userId = customer.Id;
                    displayName = customer.FirstName;
                    
               
                    break;
                }
                case UserRole.Business:
                {
                    var business = (BusinessUser)user;
                    userId = business.Id;
                    displayName = "Business User";
                    break;
                }
                case UserRole.Seller:
                {
                    var seller = (SellerUser)user;
                    userId = seller.Id;
                    displayName = "Seller User";
                    break;
                }
            }

            await _storeContext.SaveChangesAsync();

            var token = _tokenService.GenerateToken(new TokenRequestDto
            {
                UserId = userId,
                Email = requestDto.PendingAuthId,
                Role = requestDto.Role,
                DisplayName = displayName,
                StoreName = storeName



            });



            return Ok(new
            {
                message = "OTP verified successfully.",
                token,
                userId,
                username = displayName

            });

        }


    }

}