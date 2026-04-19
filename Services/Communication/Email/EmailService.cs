using System.Net;
using System.Net.Mail;
using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Extensions;
using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.BusinessUserAccount.AccountUpdate;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Verification;
using Amazon_eCommerce_API.Models.DTO_s.Cache;
using Amazon_eCommerce_API.Models.EmailEntities;
using Amazon_eCommerce_API.Services.Cache;
using Amazon_eCommerce_API.Services.Email;
using Amazon_eCommerce_API.Services.Users.Business;
using Amazon_eCommerce_API.Services.Users.Customer;
using Amazon_eCommerce_API.Services.Users.Seller;

namespace Amazon_eCommerce_API.Services.Communication.Email
{
    public class EmailService : IEmailService
    {
        private readonly ICustomerUserService _customerUserService;
        private readonly IBusinessUserService _businessUserService;
        private readonly ISellerUserService _sellerUserService;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;
        private readonly EmailSettings _emailSettings;
        private readonly StoreContext _storeContext;

        public EmailService(ICustomerUserService customerUserService, IBusinessUserService businessUserService,
            ISellerUserService sellerUserService, ICacheService cacheService, IConfiguration configuration, StoreContext storeContext)
        {
            _customerUserService = customerUserService;
            _businessUserService = businessUserService;
            _sellerUserService = sellerUserService;
            _cacheService = cacheService;
            _configuration = configuration;
            _emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();
            _storeContext = storeContext;
        }

        private EmailProviderSettings GetProvider(string providerName = null)
        {
            providerName ??= _emailSettings.DefaultProvider;

            if (!_emailSettings.Providers.ContainsKey(providerName))
                throw new Exception($"Email provider '{providerName}' is not configured.");

            return _emailSettings.Providers[providerName];
        }
    

     private async Task<object> GetUserByEmailAndType(string email, AccountType type)
        {

                   
        return type switch
        {
            AccountType.Customer => await _customerUserService.GetUserByCustomerEmailAsync(email),
            AccountType.Business => await _businessUserService.GetUserByBusinessEmailAsync(email),
            _ => null
        };
     }




        public async Task<bool> SendOtpEmailAsync(string email, string otp, string providerName = null)
        {


            var provider = GetProvider(providerName);

            string subject = "amazon.com: Sign-in attempt";

            string body = GetEmailTemplate(otp);


            using var client = new SmtpClient(provider.SmtpHost, provider.SmtpPort)
            {

                Credentials = new NetworkCredential(provider.SenderEmail, provider.SenderPassword),
                EnableSsl = true


            };


            var mailMessage = new MailMessage
            {

                From = new MailAddress(provider.SenderEmail, provider.SenderName),
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

            }

        }





        public async Task<bool> SendEmailVerificationAsync(VerifyEmailDto dto)
        {

            var user = await GetUserByEmailAndType(dto.Email, (AccountType)dto.AccountType);  //get user email


            //if email is valid or registered
            if (user == null)
            {


                return false; //email not found

            }

            var otp = OneTimePasswordGenerator.GenerateOtp(); //Generates 6 digit numeric password

            //cache service


            await _cacheService.SetOtpAsync(dto.Email, new OtpCacheDto
            {
                Identifier = dto.Email,
                Otp = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10)

            });


            await _cacheService.SetOtpRequestLimitAsync(dto.Email, new OtpRequestLimitDto 
            { 
                Identifier = dto.Email,
                LastRequestTime = DateTime.UtcNow,
                ExpirationMinutes = 10
             
            });

           return await SendOtpEmailAsync(dto.Email, otp); //send email with otp



        }






        public async Task<bool> VerifyEmailOtpAsync(VerifyEmailDto dto)
        {
            // -------------------------------
            // 🔁 RESEND FLOW
            // -------------------------------
            if (dto.IsResendRequest)
            {
                var user = await GetUserByEmailAndType(dto.Email, (AccountType)dto.AccountType);

                if (user == null) return false;

                var canRequestOtp = await _cacheService.CanRequestOtpAsync(dto.Email);
                if (!canRequestOtp) return false;

                var newOtp = OneTimePasswordGenerator.GenerateOtp();

                await _cacheService.SetOtpAsync(dto.Email, new OtpCacheDto
                {
                    Identifier = dto.Email,
                    Otp = newOtp,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(10)
                });

                await _cacheService.SetOtpRequestLimitAsync(dto.Email, new OtpRequestLimitDto
                {
                    Identifier = dto.Email,
                    LastRequestTime = DateTime.UtcNow,
                    ExpirationMinutes = 1
                });

                await SendOtpEmailAsync(dto.Email, newOtp);

                return true;
            }

            // -------------------------------
            // ✅ VERIFY OTP
            // -------------------------------
            var cachedOtpDto = await _cacheService.GetOtpAsync(dto.Email);

            if (cachedOtpDto == null || cachedOtpDto.ExpirationTime <= DateTime.UtcNow)
            {
                await _cacheService.RemoveOtpAsync(dto.Email);
                return false;
            }

            if (cachedOtpDto.Otp != dto.EmailOtp)
                return false;

            var existingUser = await GetUserByEmailAndType(dto.Email, (AccountType)dto.AccountType);

            if (existingUser == null)
                return false;

            // -------------------------------
            // 🔥 FIX: CAST BEFORE USING PROPERTY
            // -------------------------------
            if (existingUser is CustomerUser customer)   //verfies email for customer or business user and updates the database

            {
            
               customer.IsEmailVerified = true;


            }
            else if (existingUser is BusinessUser business)
            {
                business.IsBusinessEmailVerified = true;
            }
            else
            {
                return false; // Unsupported user type
            }



            await _cacheService.RemoveOtpAsync(dto.Email);


            await _storeContext.SaveChangesAsync();

            return true;
        }


        public async Task<bool> ResendEmailVerificationOtpAsync(string email, AccountType accountType)
        {
            var user = await GetUserByEmailAndType(email, accountType);

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



            //generate new request limit

            await _cacheService.SetOtpRequestLimitAsync(email, new OtpRequestLimitDto

            {

                Identifier = email,
                LastRequestTime = DateTime.UtcNow,
                ExpirationMinutes = 10


            });

            return await SendOtpEmailAsync(email, otp);
        }



     


        public string GetEmailTemplate(string verificationCode)
{
    return $@"
<html>
<head>
    <meta name='color-scheme' content='light'>
    <meta name='supported-color-schemes' content='light'>
    <style>
        body {{
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            font-family: Arial, sans-serif;
            background-color: #FFFFFF !important;
            color: #000000 !important;
            text-align: center;
            padding: 20px;
            border-radius: 10px;
            max-width: 500px;
            margin: 20px auto;
            border: 1px solid #e0e0e0;
        }}
        .otp {{
            font-size: 32px;
            font-weight: bold;
            margin: 20px 0;
            color: #000000 !important;
        }}
        p {{
            color: #000000 !important;
            line-height: 1.5;
        }}
        .footer {{
            margin-top: 20px;
            font-size: 12px;
            color: #555555 !important;
        }}
    </style>
</head>
<body style='background-color:#FFFFFF !important; color:#000000 !important;'>
    <div class='email-container' style='background-color:#FFFFFF !important; color:#000000 !important;'>
        <div class='logo' style='text-align:center;'>
            <img src='https://upload.wikimedia.org/wikipedia/commons/a/a9/Amazon_logo.svg' alt='Amazon' style='width:120px; height:auto; display:block; margin:0 auto 20px auto;'>
        </div>
        <p style='color:#000000;'>Your One-Time Password (OTP) is:</p>
        <div class='otp' style='color:#000000;'>{verificationCode}</div>
        <p style='color:#000000;'>
            Don't share this OTP with anyone. Amazon takes your account security very seriously.
            Amazon Customer Service will never ask you to disclose or verify your Amazon password, OTP,
            credit card, or banking account number. If you receive a suspicious email with a link to
            update your account information, do not click on the link — instead, report the email to Amazon
            for investigation.
        </p>
        <p style='color:#000000;'>Thank you,</p>
        <div class='footer' style='color:#555555;'>&copy; 2025 Amazon.com, Inc. or its affiliates. All rights reserved.</div>
    </div>
</body>
</html>";
}

    
    }
}
