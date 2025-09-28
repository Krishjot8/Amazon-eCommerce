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
        private readonly IConfiguration _configuration;

        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly string _senderName;

        public EmailService(IUserService userService, ICacheService cacheService, IConfiguration configuration)
        {
            _userService = userService;
            _cacheService = cacheService;
            _configuration = configuration;

            _smtpHost = _configuration["EmailSettings:SmtpHost"];
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            _senderEmail = _configuration["EmailSettings:SenderEmail"];
            _senderPassword = _configuration["EmailSettings:SenderPassword"];
            _senderName = _configuration["EmailSettings:SenderName"];


        }

        public async Task<bool> ResendEmailVerificationOtpAsync(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null) return false;


            var otpLimit = await _cacheService.GetOtpRequestLimitAsync(email);

            if (otpLimit != null && (DateTime.UtcNow - otpLimit.LastRequestTime).TotalMinutes < otpLimit.ExpirationMinutes)
            {


                return false;

            }

            //remove the old limit

            await _cacheService.RemoveOtpRequestLimitAsync(email);

            var otp = OneTimePasswordGenerator.GenerateOtp();

            //generate new otp

            await _cacheService.SetOtpAsync(email, new OtpCacheDto

            {
                Identifier = email,
                Otp = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10)
            });



            //generate new requestlimit

            await _cacheService.SetOtpRequestLimitAsync(email, new OtpRequestLimitDto

            {

                Identifier = email,
                LastRequestTime = DateTime.UtcNow,
                ExpirationMinutes = 10


            });

            return await SendOtpEmailAsync(email, otp);
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

                Identifier = dto.Email,
                Otp = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10)



            };

            await _cacheService.SetOtpAsync(dto.Email, otpCatcheDto);
            await _cacheService.SetOtpRequestLimitAsync(dto.Email, new OtpRequestLimitDto { ExpirationMinutes = 10 });

            var emailSent = await SendOtpEmailAsync(dto.Email, otp);


            return emailSent;



        }

        public async Task<bool> SendOtpEmailAsync(string email, string otp)
        {

            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null) return false;

            string subject = "Your Amazon OTP Code";

            string body = GetEmailTemplate(otp);


            using var client = new SmtpClient(_smtpHost,_smtpPort)
            {

                Credentials = new NetworkCredential(_senderEmail, _senderPassword),
                EnableSsl = true


            };


            var mailMessage = new MailMessage
            {

                From = new MailAddress(_senderEmail,_senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true

            };

            mailMessage.To.Add(email);


            try 
            {

                await client.SendMailAsync(mailMessage);
                return true;
            
            
            }
            catch
            {

                return false;

            };

        }



        public string GetEmailTemplate(string verificationCode)
        {
            return $@"
    <html>
    <head>
        <meta name='color-scheme' content='light dark'>
        <meta name='supported-color-schemes' content='light dark'>
        <style>
            body {{
                font-family: Arial, sans-serif;
                background-color: #FFFFFF !important;
                color: #000000 !important;
                text-align: center;
                padding: 20px;
            }}
            .email-container {{
                background-color: #FFFFFF !important;
                color: #000000 !important;
                padding: 20px;
                border-radius: 10px;
                max-width: 500px;
                margin: auto;
            }}
                .logo img {{
                width: 120px !important;
                height: auto !important;
                display: block;
                margin: 0 auto 20px auto;
            }}
            .otp {{
                font-size: 32px;
                font-weight: bold;
                margin: 20px 0;
                color: #000000 !important;
            }}
            p, .footer {{
                color: #000000 !important;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='logo'>
                <img src='https://upload.wikimedia.org/wikipedia/commons/a/a9/Amazon_logo.svg' alt='Amazon'>
            </div>
            <p>Your One-Time Password (OTP) is:</p>
            <div class='otp'>{verificationCode}</div>
            <p>Don't share this OTP with anyone. Amazon takes your account security very seriously.
            Amazon Customer Service will never ask you to disclose or verify your Amazon password, OTP,
            credit card, or banking account number. If you receive a suspicious email with a link to
            update your account information, do not click on the link — instead, report the email to Amazon
            for investigation.</p>

            <p>Thank you</p>

            <div class='footer'>
                &copy; 2025 Amazon.com, Inc. or its affiliates. All rights reserved.
            </div>
        </div>
    </body>
    </html>";
        }



        public async Task<bool> VerifyEmailOtpAsync(UserVerifyEmailDto dto)
        {

            if (dto.IsResendRequest)
            {

                var user = await _userService.GetUserByEmailAsync(dto.Email);

                if (user == null) return false;


                //check if user can request a new OTP

                var canRequestOtp = await _cacheService.CanRequestOtpAsync(dto.Email);
                if (!canRequestOtp) return false;


                var newOtp = OneTimePasswordGenerator.GenerateOtp();



                //cache new otp with expirationTime

                var otpCacheDto = new OtpCacheDto
                { 
                
                    Identifier = dto.Email,
                    Otp = newOtp,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(10)

                
                
                };

                await _cacheService.SetOtpAsync(dto.Email, otpCacheDto);


                //Apply Request Limit


                await _cacheService.SetOtpRequestLimitAsync(dto.Email, new OtpRequestLimitDto
                {
                    Identifier = dto.Email,
                    LastRequestTime = DateTime.UtcNow,
                    ExpirationMinutes = 1



                });

                await SendOtpEmailAsync(dto.Email, newOtp);


                return true;

            }


            // Handle OTP verification



            var cachedOtpDto = await _cacheService.GetOtpAsync(dto.Email);
            if(cachedOtpDto == null || cachedOtpDto.ExpirationTime <= DateTime.UtcNow)
            {

                await _cacheService.RemoveOtpAsync(dto.Email);
                return false;


            } 


            if(cachedOtpDto.Otp != dto.emailOtp) return false;


            //email verified


            var existingUser = await _userService.GetUserByEmailAsync(dto.Email);


            if(existingUser == null) return false;


            existingUser.IsEmailVerified = true;

            bool updateResult = await _userService.UpdateUserAsync(existingUser.Id, existingUser);

            if (!updateResult)
            {


                return false; //User Update Failed
            };

            await _cacheService.RemoveOtpAsync(dto.Email);  //remove otp after successful verification

            return true;



        }
    }
}
