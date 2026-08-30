using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Amazon_eCommerce_API.Data;
using Amazon_eCommerce_API.Extensions;
using Amazon_eCommerce_API.Models.DBEntities.Users.Business;
using Amazon_eCommerce_API.Models.DBEntities.Users.Customer;
using Amazon_eCommerce_API.Models.DTO_s.Accounts.CustomerUserAccount.Authentication;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Verification;
using Amazon_eCommerce_API.Models.DTO_s.Cache;
using Amazon_eCommerce_API.Models.EmailEntities;
using Amazon_eCommerce_API.Services.Cache;
using Amazon_eCommerce_API.Services.Users.Business;
using Amazon_eCommerce_API.Services.Users.Customer;

namespace Amazon_eCommerce_API.Services.Communication.Email
{
    public class EmailService : IEmailService
    {
        private readonly ICustomerUserService _customerUserService;
        private readonly IBusinessUserService _businessUserService;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;
        private readonly EmailSettings _emailSettings;
        private readonly StoreContext _storeContext;
        private readonly ILogger<EmailService> _logger;

        // FIX 1: Removed unused ISellerUserService & added ILogger
        public EmailService(
            ICustomerUserService customerUserService,
            IBusinessUserService businessUserService,
            ICacheService cacheService,
            IConfiguration configuration,
            StoreContext storeContext,
            ILogger<EmailService> logger)
        {
            _customerUserService = customerUserService;
            _businessUserService = businessUserService;
            _cacheService = cacheService;
            _configuration = configuration;
            _emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();
            _storeContext = storeContext;
            _logger = logger;
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
            string subject = "amazon.com: Sign-in attempt";
            string body = GetEmailTemplate(otp);

            return await SendEmailInternalAsync(email, subject, body, providerName);
        }

      
        public async Task<bool> SendPasswordResetOtpEmailAsync(
            string email,
            string otp,
            string timestamp,
            string userName = "Customer",
            string deviceDetails = "generic web browser",
            string locationDetails = "United States",
            string providerName = null)
        {
            var subject = "amazon.com: Password Recovery";
            
            var htmlContent = GetPasswordResetEmailTemplate(
                userName : userName,
                verificationCode : otp,
                timestamp : timestamp,
                deviceDetails : deviceDetails,
                locationDetails : locationDetails);

            return await SendEmailInternalAsync(email, subject, htmlContent, providerName);
        }

        public async Task<bool> SendEmailVerificationAsync(VerifyEmailDto dto)
        {
            var user = await GetUserByEmailAndType(dto.Email, (AccountType)dto.AccountType);

            if (user == null) return false;

            var otp = OneTimePasswordGenerator.GenerateOtp();

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

            return await SendOtpEmailAsync(dto.Email, otp);
        }

        public async Task<bool> VerifyEmailOtpAsync(VerifyEmailDto dto)
        {
            // Resend flow
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

            // Verification flow
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

            // FIX 3: Explicitly attached entity to store context to guarantee EF updates it
            if (existingUser is CustomerUser customer)
            {
                customer.IsEmailVerified = true;
                _storeContext.Update(customer);
            }
            else if (existingUser is BusinessUser business)
            {
                business.IsBusinessEmailVerified = true;
                _storeContext.Update(business);
            }
            else
            {
                return false;
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

            if (otpLimit != null &&
                (DateTime.UtcNow - otpLimit.LastRequestTime).TotalMinutes < otpLimit.ExpirationMinutes)
            {
                return false;
            }

            await _cacheService.RemoveOtpRequestLimitAsync(email);

            var otp = OneTimePasswordGenerator.GenerateOtp();

            await _cacheService.SetOtpAsync(email, new OtpCacheDto
            {
                Identifier = email,
                Otp = otp,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10)
            });

            await _cacheService.SetOtpRequestLimitAsync(email, new OtpRequestLimitDto
            {
                Identifier = email,
                LastRequestTime = DateTime.UtcNow,
                ExpirationMinutes = 10
            });

            return await SendOtpEmailAsync(email, otp);
        }

        private async Task<bool> SendEmailInternalAsync(string email, string subject, string htmlContent,
            string providerName = null)
        {
            var provider = GetProvider(providerName);

            using var client = new SmtpClient(provider.SmtpHost, provider.SmtpPort)
            {
                Credentials = new NetworkCredential(provider.SenderEmail, provider.SenderPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(provider.SenderEmail, provider.SenderName),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            try
            {
                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                // FIX 4: Log errors so you know why email delivery fails
                _logger.LogError(ex, "Failed to send email to {Email}", email);
                return false;
            }
        }

        public string GetEmailTemplate(string verificationCode)
        {
            return $@"
<html>
<head>
    <meta name='color-scheme' content='light'>
    <meta name='supported-color-schemes' content='light'>
    <style>
        body {{ margin: 0; padding: 0; }}
        .email-container {{ font-family: Arial, sans-serif; background-color: #FFFFFF !important; color: #000000 !important; text-align: center; padding: 20px; border-radius: 10px; max-width: 500px; margin: 20px auto; border: 1px solid #e0e0e0; }}
        .otp {{ font-size: 32px; font-weight: bold; margin: 20px 0; color: #000000 !important; }}
        p {{ color: #000000 !important; line-height: 1.5; }}
        .footer {{ margin-top: 20px; font-size: 12px; color: #555555 !important; }}
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
        </p>
        <p style='color:#000000;'>Thank you,</p>
        <div class='footer' style='color:#555555;'>&copy; {DateTime.UtcNow.Year} Amazon.com, Inc. or its affiliates. All rights reserved.</div>
    </div>
</body>
</html>";
        }

        public string GetPasswordResetEmailTemplate(
            string userName,
            string verificationCode,
            string timestamp,
            string deviceDetails,
            string locationDetails,
            string denyLink = "https://www.amazon.com")
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8' />
    <meta name='color-scheme' content='dark'>
    <meta name='supported-color-schemes' content='dark'>
    <style>
        body {{ 
            background-color: #111414; 
            color: #ffffff; 
            font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif; 
            margin: 0; 
            padding: 24px; 
            -webkit-font-smoothing: antialiased;
        }}
        .email-container {{ 
            max-width: 580px; 
            margin: 0 auto; 
            background-color: #111414; 
            text-align: left;
        }}
        .logo {{ margin-bottom: 28px; }}
        /* Forces any logo image to render 100% pure white */
        .white-logo {{ 
            width: 115px; 
            height: auto; 
            display: block; 
            filter: brightness(0) invert(1); 
            -webkit-filter: brightness(0) invert(1);
        }}
        .greeting {{ 
            font-size: 18px; 
            font-weight: 600; 
            margin-bottom: 20px; 
            color: #ffffff; 
            letter-spacing: -0.2px;
        }}
        .intro {{ 
            font-size: 15px; 
            color: #ffffff; 
            margin-bottom: 20px; 
            font-weight: 400;
            line-height: 1.4;
        }}
        .details {{ 
            font-size: 15px; 
            color: #ffffff; 
            line-height: 1.6; 
            margin-bottom: 28px; 
            font-weight: 400;
        }}
        .code-label {{ 
            font-size: 15px; 
            font-weight: 700; 
            color: #ffffff; 
            margin-bottom: 10px; 
        }}
        .otp {{ 
            font-size: 34px; 
            font-weight: 700; 
            letter-spacing: 1px; 
            color: #ffffff; 
            margin-bottom: 28px; 
        }}
        .security-notice {{ 
            font-size: 15px; 
            color: #ffffff; 
            line-height: 1.5; 
            margin-bottom: 28px; 
            font-weight: 400;
        }}
        .security-notice a {{ 
            color: #55b5d9; 
            text-decoration: none; 
        }}
        .footer-box {{ 
            background-color: #232f3e; 
            padding: 20px; 
            font-size: 12px; 
            color: #cccccc; 
            line-height: 1.5; 
            border-radius: 4px; 
        }}
        .footer-box a {{ 
            color: #55b5d9; 
            text-decoration: none; 
            word-break: break-all; 
        }}
        .footer-logo {{ 
            width: 38px; 
            margin-top: 15px; 
            display: block; 
            filter: brightness(0) invert(1);
            -webkit-filter: brightness(0) invert(1);
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='logo'>
            <!-- Pure White Amazon Logo -->
            <img src='https://upload.wikimedia.org/wikipedia/commons/a/a9/Amazon_logo.svg' alt='Amazon' class='white-logo'>
        </div>

        <div class='greeting'>{userName ?? "Customer"},</div>

        <div class='intro'>Someone is attempting to reset the password of your account.</div>

        <div class='details'>
            <strong>When:</strong> {timestamp}<br>
            <strong>Device:</strong> {deviceDetails}<br>
            <strong>Near:</strong> {locationDetails}
        </div>

        <div class='code-label'>If this was you, your verification code is:</div>
        <div class='otp'>{verificationCode}</div>

        <div class='security-notice'>
            If you didn't request it: <a href='{denyLink}'>click here to deny</a>.<br>
            Don't share it with others.
        </div>

        <div class='footer-box'>
            &copy;{DateTime.UtcNow.Year} <a href='https://www.amazon.com'>Amazon.com</a>, Inc. or its affiliates. Amazon and all related marks are trademarks of <a href='https://www.amazon.com'>Amazon.com</a>, Inc. or its affiliates, <a href='https://www.amazon.com'>Amazon.com</a>, Inc. 410 Terry Avenue N., Seattle, WA 98109.<br><br>
            Is it safe to follow this link?<br>
            The link provided in this email starts with <strong>'https://www.amazon.com'</strong>. If you prefer, copy the following link and paste it into a browser to view.<br><br>
            <a href='{denyLink}'>{denyLink}</a>

            <img src='https://upload.wikimedia.org/wikipedia/commons/a/a9/Amazon_logo.svg' alt='Amazon' class='footer-logo'>
        </div>
    </div>
</body>
</html>";

        }
    }
}   