using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.Users;
using Amazon_eCommerce_API.Services.Users;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAccountController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public CustomerAccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }




        [HttpPost("register")]



        public async Task<IActionResult> CustomerRegister(UserRegistrationDto userRegistrationDto)

        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

          var emailTaken = await _userService.IsEmailTakenAsync(userRegistrationDto.Email);
            var usernameTaken = await _userService.IsUsernameTakenAsync(userRegistrationDto.UserName);


            if (emailTaken) {
            
            
            return BadRequest($"The customer email address {userRegistrationDto.Email} is already taken.");  
            
            }

            if (usernameTaken) {

                return BadRequest($"The customer username {userRegistrationDto.UserName} is already taken.");
            
            }




            string roleName = "Customer";

            var user = await _userService.RegisterUserAsync(userRegistrationDto, roleName);



            if (user == null)
            {

                return BadRequest("Customer Registration Failed");

            }

           


            return Ok(new
            {
                Message = "Customer Registration Successful",
                UserId = user.Id,  // Assuming your user model has an Id property
                UserName = user.Username  // Returning the username for confirmation
            });


        }





        [HttpGet]



        public async Task<IActionResult> GetAllCustomerAccounts()
        {


            var user = await _userService.GetAllUsersAsync();


            var customerusers = user.Where(u => u.RoleId == 1);

            if (customerusers == null || !customerusers.Any())
            {


                return NotFound("the customer account you are looking for does not exist.");

            }


            return Ok(customerusers);



        }


        [HttpGet("{id}")]


        public async Task<IActionResult> GetCustomerAccount(int id)
        {


            var user = await _userService.GetAllUsersAsync();


            var customeruser = user.FirstOrDefault(u => u.Id == id && u.RoleId == 1);

            if (customeruser == null)
            {


                return NotFound($"No Customer found with user ID {id}");

            }

            return Ok(customeruser);




        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomerAccountDetails(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fetch the user by ID from the user service
            var customerUser = await _userService.GetUserByIdAsync(id);

            // Check if the user exists and is a customer
            if (customerUser == null || customerUser.RoleId != 1)
            {
                return NotFound("The customer user you are trying to find does not exist or is not a customer.");
            }

            // Map the properties from UserUpdateDto to the customerUser entity
            _mapper.Map(userUpdateDto, customerUser);

            // Save changes to the database by calling UpdateUserAsync with the User entity
            var isUpdated = await _userService.UpdateUserAsync(customerUser.Id, customerUser);

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



        public async Task<IActionResult> UpdateCustomerAccountPassword(int userId, UserPasswordUpdateDto userPasswordUpdateDto)
        {

            if (!ModelState.IsValid) {
            
            return BadRequest(ModelState);
            
            }

            var customerUser = await _userService.GetUserByIdAsync(userId);


            if (customerUser == null || customerUser.RoleId != 1)
            {

                return NotFound("the customer password you are trying to update does not exist or the user is not a customer");
            
            
            }


            _mapper.Map(userPasswordUpdateDto, customerUser);

            customerUser.PasswordHash = await _userService.HashPasswordAsync(userPasswordUpdateDto.NewPassword);


            var isUpdated = await _userService.UpdateUserAsync(userId, customerUser);



            if (!isUpdated)
            {

                return StatusCode(500,"Error updating the password");
            
            }

            return Ok(new
            {

                Message = "Customer Password Updated successfully."

            });
        }


    }

}












