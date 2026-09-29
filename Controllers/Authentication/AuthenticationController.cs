using Amazon_eCommerce_API.Models.DTO_s.Authentication.CheckIdentifier;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Services.Authentication.UserResolver;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers.Authentication
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserResolverService _userResolverService;
        public AuthenticationController(IUserResolverService userResolverService)
        {
            _userResolverService = userResolverService;         
        }
        
        [HttpPost("check-identifier")]
        public async Task<IActionResult> CheckIdentifier([FromBody] CheckIdentifierDto request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Identifier))
                return BadRequest("Identifier is required.");
            
            
            var identifier = request.Identifier.Trim();
            

            var user = await _userResolverService.ResolveUserAsync(identifier, (UserRole)request.AccountType);
           
            
            return Ok(new { exists = user != null });
        }

       
    }
}