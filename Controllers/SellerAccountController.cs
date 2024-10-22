﻿using Amazon_eCommerce_API.Models.Users;
using Amazon_eCommerce_API.Services.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerAccountController : ControllerBase
    {
        
        private readonly IUserService _userService;

        public SellerAccountController(IUserService userService)
        {
            _userService = userService;
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


    }


    



}
