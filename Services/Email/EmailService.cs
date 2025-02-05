using Amazon_eCommerce_API.Extensions;
using Amazon_eCommerce_API.Models.DTO_s;
using Amazon_eCommerce_API.Models.DTO_s.Cache;
using Amazon_eCommerce_API.Services.Cache;
using Amazon_eCommerce_API.Services.Users;
using System.Net;
using System.Net.Mail;

namespace Amazon_eCommerce_API.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IUserService _userService;
        private readonly ICacheService _cacheService;

        public EmailService(IUserService userService, ICacheService cacheService)
        {
            _userService = userService;
            _cacheService = cacheService;
        }

        public Task<bool> ResendEmailVerificationOtpAsync(string email)
        {
            throw new NotImplementedException();
        }



        public async Task<bool> SendEmailVerificationAsync(UserVerifyEmailDto dto)
        {

            var user = await _userService.GetUserByEmailAsync(dto.Email);  //get user email


            //if email is valid or registered
            if (user == null)
            {


                return false; //email not found

            }

            var otp = OneTimePasswordGenerator.GenerateOtp(); //Generates 6 digit numeric password

            //cache service

            var otpCatcheDto = new OtpCacheDto
            {

                Email = dto.Email,
                Otp = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10)



            };

            await _cacheService.SetOtpAsync(dto.Email, otpCatcheDto);
            await _cacheService.SetOtpRequestLimitAsync(dto.Email, new OtpRequestLimitDto {ExpirationMinutes = 10});

            var emailSent = await SendOtpEmailAsync(dto.Email, otp);


            return emailSent;



        }

        public async Task<bool> SendOtpEmailAsync(string email, string otp)
        {

            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)  return false;

            string subject = "Your Amazon OTP Code";

            string body = GetEmailTemplate(otp);


            using (var client = new SmtpClient("smtp.your-email-provider.com"))
            {

                client.Port = 587;
                client.Credentials = new NetworkCredential("your-email@example.com", "your-password");
                client.EnableSsl = true;


                var mailMessage = new MailMessage
                {
                    From = new MailAddress("your-email@example.com", "Amazon"),
                    Subject = subject,
                    Body = body,
                   IsBodyHtml = true,


                };


                mailMessage.To.Add(email);

                try
                {

                    await client.SendMailAsync(mailMessage);
                    return true;
                }
                catch {

                    return false;
                
                }

            }

        }


        private string GetEmailTemplate(string otp) 
        
        {

            return $@"

                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #232F3E;
                            color: white;
                            text-align: center;
                            padding: 20px;
                        }}
                        .email-container {{
                            background-color: #131A22;
                            padding: 20px;
                            border-radius: 10px;
                            max-width: 500px;
                            margin: auto;
                        }}
                        .logo img {{
                            max-width: 100px;
                        }}
                        .otp {{
                            font-size: 32px;
                            font-weight: bold;
                            margin: 20px 0;
                        }}
                        .footer {{
                            font-size: 12px;
                            margin-top: 20px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='logo'>
                            <img src='https://upload.wikimedia.org/wikipedia/commons/a/a9/Amazon_logo.svg' alt='Amazon'>
                        </div>
                        <p>Your One-Time Password (OTP) is:</p>
                        <div class='otp'>{otp}</div>
                        <p>Don't share this OTP with anyone. Amazon takes your security seriously.</p>
                        <div class='footer'>
                            &copy; 2025 Amazon.com, Inc. or its affiliates. All rights reserved.
                        </div>
                    </div>
                </body>
                </html> ";
        
        
        
        
        }






        public Task<bool> VerifyEmailOtpAsync(UserVerifyEmailDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
