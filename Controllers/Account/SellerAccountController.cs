using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Models.DBEntities.Users.Seller;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountRegistration.SellerOnboarding;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountVerification.Address;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountVerification.Documents;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.AccountVerification.Meeting;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.SellerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Amazon_eCommerce_API.Services.Authentication.PasswordChallenge;
using Amazon_eCommerce_API.Services.Authentication.Token;
using Amazon_eCommerce_API.Services.Users.Seller;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Amazon_eCommerce_API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerAccountController : ControllerBase
    {
        
        private readonly ISellerUserService _sellerUserService;
        private readonly IPasswordChallengeService _passwordChallengeService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly StoreContext _storeContext;

        public SellerAccountController(ISellerUserService sellerUserService, IMapper mapper, StoreContext storeContext, ITokenService tokenService, IPasswordChallengeService passwordChallengeService)
        {
         
            _sellerUserService = sellerUserService;
            _mapper = mapper;
            _storeContext = storeContext;
            _tokenService = tokenService;
            _passwordChallengeService = passwordChallengeService;
        }




        [HttpPost("register/account")]

        public async Task<IActionResult> CreateSellerAccount(SellerUserAccountCreationDto accountCreationDto) 
        
        { 
              if(!ModelState.IsValid)
                return BadRequest(ModelState);
              
              var sellerId = await _sellerUserService.CreateSellerAccountAsync(accountCreationDto);

            

            return Ok(new
            {
                SellerId = sellerId,
                Step = "Account Created"
               
            });
            
        }

        [HttpPost("register/business-information")]

        public async Task<IActionResult> CompleteBusinessInformation(int sellerUserId, 
            SellerUserBusinessInformationDto businessInformationDto)
        {
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var result = await _sellerUserService.CompleteBusinessInformationAsync(sellerUserId, businessInformationDto);
            
            if(!result)
                return NotFound("SellerUser Not found");

            return Ok(new
            {
                
                Message = "Business information saved successfully",
                Step = " BusinessInformation"
            }

            );
        }

        [HttpPost("register/business-profile")]

        public async Task<IActionResult> CompleteBusinessProfile(int sellerUserId,
            [FromBody] SellerUserBusinessProfileDto businessProfileDto)
        {
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _sellerUserService.CompleteBusinessProfileAsync(sellerUserId, businessProfileDto);

                if (!result)
                    return NotFound("SellerUser Not found");

                return Ok(new
                    {
                        Message = "Business Profile saved successfully",
                        Step = SellerOnboardingStep.BusinessProfile
                    }
                );



            }
            catch (Exception ex)
            {
                
                return BadRequest(new { message = ex.Message });
                
                
            }
            
            
            
        }


        [HttpPost("register/{sellerUserId}/primary-contact")]


        public async Task<IActionResult> CompletePrimaryContact(int sellerUserId,
            [FromBody] SellerUserPrimaryContactInformationDto contactInformationDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _sellerUserService.CompletePrimaryContactAsync(sellerUserId, contactInformationDto);
                
                if(!result)
                    return NotFound("SellerUser Not found");


                return Ok(new
                {
                    Message = "Primary contact information saved successfully",
                    Step = SellerOnboardingStep.PrimaryContact
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }

        [HttpPost("register/{sellerUserId}/payment")]

        public async Task<IActionResult> AddPaymentInformation(int sellerUserId,
            [FromBody] SellerUserPaymentInformationDto paymentInformationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            try
            {
                
                var result = await _sellerUserService.AddPaymentInformationAsync(sellerUserId, paymentInformationDto);
                
                if(!result)
                    return NotFound("SellerUser Not found");

                return Ok(new
                {
                    Message = "Payment information saved successfully",
                    Step = SellerOnboardingStep.PaymentInformation
                });


            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }


        [HttpPost("register/{sellerUserId}/store-information")]

        public async Task<IActionResult> CompleteStoreInformation(int sellerUserId,
            [FromBody] SellerUserStoreProductInformationDto storeInformationDto)
        {
             if(!ModelState.IsValid)
                 return BadRequest(ModelState);

             try
             {
                 var result = await _sellerUserService.CompleteStoreInformationAsync(sellerUserId, storeInformationDto);
                 
                 if(!result)
                     return NotFound("SellerUser Not found");

                 return Ok(new
                 {
                     Message = "Store information saved successfully",
                     Step = SellerOnboardingStep.StoreInformation

                 });
             }
             catch (Exception ex)
             {
                 return BadRequest(new { message = ex.Message });
             }


        }


        [HttpPost("register/{sellerUserId}/verification")]

        public async Task<IActionResult> SubmitVerification(int sellerUserId,
            [FromBody] SellerUserVerificationStatusDto verificationStatusDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _sellerUserService.SubmitVerificationAsync(sellerUserId, verificationStatusDto);
                
                if(!result)
                    return NotFound("SellerUser Not found");

                return Ok(new
                {
                    Message = "Verification saved successfully",
                    Step = SellerOnboardingStep.VerificationSubmitted,
                    Status = SellerAccountStatus.UnderReview
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }


        }

        [HttpPost("register/{sellerUserId}/schedule-meeting")]



        public async Task<IActionResult> ScheduleMeeting(int sellerUserId,
            [FromBody] ScheduleVerificationMeetingDto meetingDto)
        {
         
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _sellerUserService.ScheduleVerificationMeetingAsync(sellerUserId, meetingDto);
                
                if(!result)
                    return NotFound("SellerUser Not found");

                return Ok(new
                    {
                        
                        Message = "Schedule meeting saved successfully",
                        Step = SellerOnboardingStep.MeetingScheduled
                    });


            }catch(Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            
        }
        
        
        
        
        
        
        
        [HttpPost("login")]
        
        public async Task<IActionResult> SellerLogin([FromBody] SellerUserLoginDto sellerUserLoginDto) {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            };
            
            var otpChallenge = await _passwordChallengeService.GenerateOtpChallengeAsync(sellerUserLoginDto.EmailOrPhone ,sellerUserLoginDto.Password,  UserRole.Seller);


            if (otpChallenge == null)
                return Unauthorized(new { message = "Invalid email or Password" });


            
          
            return Ok(new

            {
                message = $"OTP sent to your {otpChallenge.OtpChannel}",
                pendingAuthId = otpChallenge.PendingAuthId,
                destination = otpChallenge.MaskedDestination


            });
            
         
            
               
  
            
        }




        [HttpGet]

        public async Task<IActionResult> GetAllSellerAccounts() {

            var sellerUser = await _sellerUserService.GetAllSellerUsersAsync();

         

            if (sellerUser == null || !sellerUser.Any()) {


                return NotFound("The seller account you are looking for does not exist.");
            }

            return Ok(sellerUser);
        
        }


        [HttpGet("{id}")]

        public async Task<IActionResult> GetSellerAccount(int id)
        {

            var sellerUsers = await _sellerUserService.GetAllSellerUsersAsync();


            var sellerUser = sellerUsers.FirstOrDefault(e => e.Id == id);

            if(sellerUser == null)
            {

                return NotFound($"There are no seller account with ID {id}");
            }


            return Ok(sellerUser);


        }


      







        [HttpGet("email")]

        public async Task<IActionResult>GetSellerAccountByEmail(string email)
        {

            var sellerUser = await _sellerUserService.GetUserByBusinessEmailAsync(email);

            if(sellerUser == null )
            {


                return NotFound("The seller account you are trying to retrieve does not exist ");


            }

            return Ok(sellerUser);

        }







        [HttpGet("phoneNumber")]


        public async Task<IActionResult>GetSellerAccountByPhoneNumber(string phoneNumber)
        {

            var sellerUser = await _sellerUserService.GetUserByBusinessPhoneNumberAsync(phoneNumber);

            if(sellerUser == null)
            {


                return NotFound("The seller account you are trying to retrieve does not exist ");

            }

            return Ok(sellerUser);
        }


        

        [HttpPut("{id}")]


        public async Task<IActionResult> UpdateSellerAccountDetails(int id, UpdateSellerUserDto userDto) 
        {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            
            }
        

            var selleruser = await _sellerUserService.GetUserBySellerIdAsync(id);

            if (selleruser == null ) 
            
            {

                return NotFound("The seller account for the account details you are trying to update does not exist");
            
            }

            _mapper.Map(userDto, selleruser);


            var isUpdated = await _sellerUserService.UpdateSellerUserAsync(selleruser.Id, selleruser);

            if (!isUpdated) {

                return StatusCode(500,"Error updating the seller account details");     
            }

            return Ok(
                new
                {
                    Message = "Seller account details updated successfully."
                }
                );
        
        
        
        
        }



        [HttpPut("update-password")]



        public async Task<IActionResult> UpdateSellerAccountPassword(int userId, [FromBody] UpdateBusinessUserPasswordDto userPasswordDto) 
        {

            if (!ModelState.IsValid) { 
            
            
                return BadRequest(ModelState);
            }

            var sellerUser = await _sellerUserService.GetUserBySellerIdAsync(userId);

            if (sellerUser == null )
            {

                return NotFound("The seller account you are looking for does not exist.");
            
            
            }

            _mapper.Map(userPasswordDto,sellerUser);

            sellerUser.PasswordHash = await _sellerUserService.HashSellerPasswordAsync(userPasswordDto.NewPassword);

            var isUpdated = await _sellerUserService.UpdateSellerUserAsync(userId, sellerUser);

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


            var sellerUser = await _sellerUserService.GetUserBySellerIdAsync(id);


            if (sellerUser == null )
            { 
            
            return NotFound("The seller user you are trying to delete does not exist or is not a seller.");
            
            }


            var isDeleted = await _sellerUserService.DeleteSellerUserAsync(id);


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
