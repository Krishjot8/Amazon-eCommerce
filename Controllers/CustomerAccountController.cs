using Amazon_eCommerce_API.Models.Users;
using Amazon_eCommerce_API.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAccountController : ControllerBase
    {
        
        private readonly IUserService _userService;

        public CustomerAccountController(IUserService userService)
        {
            _userService = userService;
        }




        [HttpPost("register")]



        public async Task<IActionResult> RegisterCustomer(UserRegistrationDto userRegistrationDto) 
        
        { 
        

              if(!ModelState.IsValid)
                return BadRequest(ModelState);

            string roleName = "Customer";


            var user = await _userService.RegisterUserAsync(userRegistrationDto, roleName);



            if (user == null) { 
            
            
                 return BadRequest("Customer Registration Failed");
              
            
            }


            return Ok(new
            {
                Message = "Customer Registration Successful",
                UserId = user.Id,  // Assuming your user model has an Id property
                UserName = user.Username  // Returning the username for confirmation
            });


        }






    }


    



}
