using Amazon_eCommerce_API.Models.Users;
using Amazon_eCommerce_API.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccountController : ControllerBase
    {
        
        private readonly IUserService _userService;

        public AdminAccountController(IUserService userService)
        {
            _userService = userService;
        }




        [HttpPost("register")]

        public async Task<IActionResult> RegisterAdmin(UserRegistrationDto userRegistrationDto) 
        
        { 
        

              if(!ModelState.IsValid)
                return BadRequest(ModelState);

            string roleName = "Admin";


            var user = await _userService.RegisterUserAsync(userRegistrationDto, roleName);



            if (user == null) { 
            
            
                 return BadRequest("Admin Registration Failed");
              
            
            }


            return Ok(new
            {
                Message = "Admin Registration Successful",
                UserId = user.Id,  // Assuming your user model has an Id property
                UserName = user.Username  // Returning the username for confirmation
            });


        }






    }


    



}
