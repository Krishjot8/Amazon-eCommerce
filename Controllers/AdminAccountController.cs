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




        [HttpGet]

        public async Task<IActionResult> GetAllAdminAccounts()
        {


            var users = await _userService.GetAllUsersAsync();


            var adminUsers = users.Where(u => u.RoleId == 2).ToList();

            if (adminUsers == null || !adminUsers.Any())
            {


                return NotFound("No admins were found.");

            }






            return Ok(adminUsers);



        }




        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAdminAccountById(int Id)
        {
            var users = await _userService.GetAllUsersAsync();

            // Find user by Id and ensure their RoleId is 2 (Admin)
            var adminUser = users.FirstOrDefault(u => u.Id == Id && u.RoleId == 2);

            if (adminUser == null)
            {
                return NotFound($"No admin found with user ID {Id}.");
            }

            return Ok(adminUser); // Return the admin user details
        }












        [HttpPost("register")]

        public async Task<IActionResult>AdminRegister(UserRegistrationDto userRegistrationDto) 
        
        { 
        

              if(!ModelState.IsValid)
                return BadRequest(ModelState);


              var emailTaken = await _userService.IsEmailTakenAsync(userRegistrationDto.Email);
            var usernameTaken = await _userService.IsUsernameTakenAsync(userRegistrationDto.UserName);

            if (emailTaken) {


                return BadRequest($"The admin email address {userRegistrationDto.Email} is already taken.");
            }

            if (usernameTaken) { 
            
            return BadRequest($"The admin username {userRegistrationDto.UserName} is already taken.");  
            
            }



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
