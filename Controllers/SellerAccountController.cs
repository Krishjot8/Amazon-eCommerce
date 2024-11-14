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
    public class SellerAccountController : ControllerBase
    {
        
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public SellerAccountController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }




        [HttpPost("register")]

        public async Task<IActionResult> SellerRegister(UserRegistrationDto userRegistrationDto) 
        
        { 
        
              if(!ModelState.IsValid)
                return BadRequest(ModelState);

              var emailTaken  = await _userService.IsEmailTakenAsync(userRegistrationDto.Email);
            var usernameTaken = await _userService.IsUsernameTakenAsync(userRegistrationDto.UserName);

            if (emailTaken) {

                return BadRequest($"The seller email address {userRegistrationDto.Email} is already taken.");
           
            }

            if (usernameTaken) {
            
                return BadRequest($"The seller username {userRegistrationDto.UserName} is already taken.");
            
            }

            string roleName = "Seller";

            var user = await _userService.RegisterUserAsync(userRegistrationDto, roleName);

            if (user == null) {      
            
                 return BadRequest("Seller Registration Failed");        
            
            }


            return Ok(new
            {
                Message = "Seller Registration Successful",
                UserId = user.Id,  // Assuming your user model has an Id property
                UserName = user.Username  // Returning the username for confirmation
            });


        }




        [HttpGet]

        public async Task<IActionResult> GetAllSellerAccounts() {

            var user = await _userService.GetAllUsersAsync();

            var selleruser = user.Where(e => e.RoleId == 3);

            if (selleruser == null || !selleruser.Any()) {


                return NotFound("The seller account you are looking for does not exist.");
            }

            return Ok(selleruser);
        
        }


        [HttpGet("{id}")]

        public async Task<IActionResult> GetSellerAccount(int id)
        {

            var users = await _userService.GetAllUsersAsync();


            var selleruser = users.FirstOrDefault(e => e.Id == id && e.RoleId == 3);

            if(selleruser == null)
            {

                return NotFound($"There are no seller account with ID {id}");
            }


            return Ok(selleruser);


        }


        [HttpGet("username")]


        public async Task<IActionResult> GetSellerAccountByUsername(string username)
        {

            var sellerUser = await _userService.GetUserByUsernameAsync(username);

            if(sellerUser == null || sellerUser.RoleId !=3)
            {


                return NotFound("The seller account you are trying to get does not exist or is not a seller account");


            }


            return Ok(sellerUser);
        
        
        }








        [HttpGet("email")]

        public async Task<IActionResult>GetSellerAccountByEmail(string email)
        {

            var sellerUser = await _userService.GetUserByEmailAsync(email);

            if(sellerUser == null || sellerUser.RoleId!=3)
            {


                return NotFound("The seller account you are trying to get does not exist or is not a seller account");


            }

            return Ok(sellerUser);

        }







        [HttpGet("phoneNumber")]


        public async Task<IActionResult>GetSellerAccountByPhoneNumber(string phoneNumber)
        {

            var sellerUser = await _userService.GetUserByPhoneNumberAsync(phoneNumber);

            if(sellerUser == null || sellerUser.RoleId !=3)
            {


                return NotFound("The seller account you are trying to get does not exist or it is not a seller account.");

            }

            return Ok(sellerUser);
        }







        [HttpPut("{id}")]


        public async Task<IActionResult> UpdateSellerAccountDetails(int id, UserUpdateDto userUpdateDto) 
        {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            
            }
        

            var selleruser = await _userService.GetUserByIdAsync(id);

            if (selleruser == null || selleruser.RoleId != 3) 
            
            {

                return NotFound("The seller account you are trying to update does not exist or it is not a seller account.");
            
            }

            _mapper.Map(userUpdateDto, selleruser);


            var isUpdated = await _userService.UpdateUserAsync(selleruser.Id, selleruser);

            if (!isUpdated) {

                return StatusCode(500,"Error updating the seller account");     
            }

            return Ok(
                new
                {
                    Message = "Seller account updated successfully."
                }
                );
        
        
        
        
        }



        [HttpPut("update-password")]



        public async Task<IActionResult> UpdateSellerAccountPassword(int userId, [FromBody] UserPasswordUpdateDto userPasswordUpdateDto) 
        {

            if (!ModelState.IsValid) { 
            
            
                return BadRequest(ModelState);
            }

            var sellerUser = await _userService.GetUserByIdAsync(userId);

            if (sellerUser == null || sellerUser.RoleId != 3)
            {

                return NotFound("The seller account you are looking for does not exist or it is not a seller account.");
            
            
            }

            _mapper.Map(userPasswordUpdateDto,sellerUser);

            sellerUser.PasswordHash = await _userService.HashPasswordAsync(userPasswordUpdateDto.NewPassword);

            var isUpdated = await _userService.UpdateUserAsync(userId, sellerUser);

            if (!isUpdated)
            {


                return StatusCode(500, "Error updating the password");


            }

            return Ok(new

            { 
               Message = "The seller account password has been updated successfully."
            }

                );

        
        
        }



        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteSellerAccount(int id) {


            var sellerUser = await _userService.GetUserByIdAsync(id);


            if (sellerUser == null || sellerUser.RoleId != 3)
            { 
            
            return NotFound("The seller user you are trying to delete does not exist or is not a seller.");
            
            }


            var isDeleted = await _userService.DeleteUserAsync(id);


            if (!isDeleted)
            {

                return StatusCode(500,"Error deleting seller account.");
            
            }


            return Ok(new

            { 
               Message = "Seller account deleted successfully."
            
            }
                
                );


        }





    }


    



}
