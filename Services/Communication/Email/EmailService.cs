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

        public async Task<bool> SendRegistrationOtpEmailAsync(string email, string otp, string providerName = null)
        {
            string subject = "amazon.com: Verify your new Amazon account";
            string body = GetRegistrationEmailTemplate(otp);

            return await SendEmailInternalAsync(email, subject, body, providerName);
        }

        public async Task<bool> SendSignInAttemptOtpEmailAsync(string email, string otp, string timestamp, string userName = "Customer",
            string deviceDetails = "generic web browser", string locationDetails = "Texas, United States", string denyLink = "https://www.amazon.com", string providerName = null)
        {
            var subject = "amazon.com: Sign-in attempt";

            var htmlContent = GetSignInAttemptEmailTemplate(
                verificationCode: otp,
                timestamp: timestamp,
                userName: userName,
                deviceDetails: deviceDetails,
                locationDetails: locationDetails,
                denyLink: denyLink);

            return await SendEmailInternalAsync(email, subject, htmlContent, providerName);
        }


        public async Task<bool> SendPasswordResetOtpEmailAsync(
            string email,
            string otp,
            string timestamp,
            string userName = "Customer",
            string deviceDetails = "generic web browser",
            string locationDetails = "United States",
            string denyLink = "https://www.amazon.com",
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

            return await SendRegistrationOtpEmailAsync(dto.Email, otp);
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

                await SendRegistrationOtpEmailAsync(dto.Email, newOtp);
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

            return await SendRegistrationOtpEmailAsync(email, otp);
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

        public string GetRegistrationEmailTemplate(string verificationCode)
        {
         return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <meta name='color-scheme' content='light' />
    <meta name='supported-color-schemes' content='light' />
    <style>
        body, table, td, div {{
            font-family: Arial, sans-serif !important;
        }}
    </style>
</head>
<body bgcolor='#ffffff' style='margin: 0; padding: 0; background-color: #ffffff !important; color: #111111 !important;'>
    <!-- Outer Container -->
    <table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#ffffff' style='background-color: #ffffff !important; width: 100%;'>
        <tr>
            <td align='center' bgcolor='#ffffff' style='padding: 24px; background-color: #ffffff !important;'>
                
                <!-- Main Email Card -->
                <table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0' style='max-width: 540px; background-color: #ffffff !important;'>
                    
                    <!-- Top Black Amazon Logo -->
                    <tr>
                        <td align='left' bgcolor='#ffffff' style='background-color: #ffffff !important; padding-bottom: 24px;'>
                            <img src='https://www.thriftysigns.com/wp-content/uploads/2020/12/Amazon-Logo.jpg.webp' 
                                 alt='Amazon' 
                                 width='115' 
                                 style='width: 115px; height: auto; display: block; border: 0;'>
                        </td>
                    </tr>

                    <!-- Title -->
                    <tr>
                        <td align='left' bgcolor='#ffffff' style='background-color: #ffffff !important; font-size: 17px; font-weight: bold; color: #111111 !important; padding-bottom: 6px;'>
                            Verify your new Amazon account
                        </td>
                    </tr>

                    <!-- Subtext -->
                    <tr>
                        <td align='left' bgcolor='#ffffff' style='background-color: #ffffff !important; font-size: 14px; color: #222222 !important; line-height: 1.4; padding-bottom: 18px;'>
                            To verify your email address, please use the following One Time Password (OTP):
                        </td>
                    </tr>

                    <!-- OTP Code -->
                    <tr>
                        <td align='left' bgcolor='#ffffff' style='background-color: #ffffff !important; font-size: 32px; font-weight: bold; letter-spacing: 0.5px; color: #111111 !important; padding-bottom: 24px;'>
                            {verificationCode}
                        </td>
                    </tr>

                    <!-- Disclaimer -->
                    <tr>
                        <td align='left' bgcolor='#ffffff' style='background-color: #ffffff !important; font-size: 13.5px; color: #111111 !important; line-height: 1.45; padding-bottom: 24px;'>
                            Don't share this OTP with anyone. Amazon takes your account security very seriously. Amazon Customer Service will never ask you to disclose or verify your Amazon password, OTP, credit card, or banking account number. If you receive a suspicious email with a link to update your account information, do not click on the link—instead, report the email to Amazon for investigation.
                        </td>
                    </tr>

                    <!-- Closing -->
                    <tr>
                        <td align='left' bgcolor='#ffffff' style='background-color: #ffffff !important; font-size: 14px; color: #111111 !important; padding-bottom: 28px;'>
                            Thank you
                        </td>
                    </tr>

                    <!-- Footer Container -->
                    <tr>
                        <td align='left' bgcolor='#f0f2f2' style='background-color: #f0f2f2 !important; padding: 20px; border-radius: 2px;'>
                            <table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0'>
                                <tr>
                                    <td style='font-size: 11.5px; color: #555555 !important; line-height: 1.5;'>
                                        &copy;{DateTime.UtcNow.Year} Amazon.com, Inc. or its affiliates. Amazon and all related marks are trademarks of Amazon.com, Inc. or its affiliates, Amazon.com, Inc. 410 Terry Avenue N., Seattle, WA 98109.
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding-top: 16px;'>
                                        <!-- Bottom Black Amazon Logo (Arrow/Smile) -->
                                        <img src='https://vectorseek.com/wp-content/uploads/2023/09/Amazon-shopping-smile-Logo-Vector.svg-.png' 
                                             alt='Amazon' 
                                             width='60' 
                                             style='width: 60px; height: auto; display: block; border: 0;'>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                </table>

            </td>
        </tr>
    </table>
</body>
</html>";
        }
        

        public string GetSignInAttemptEmailTemplate(string verificationCode, string timestamp, string userName, string deviceDetails,
            string locationDetails , string denyLink = "https://www.amazon.com")
        {
          return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <meta name='color-scheme' content='dark' />
    <meta name='supported-color-schemes' content='dark' />
    <style>
        body, table, td, div {{
            font-family: Arial, sans-serif !important;
        }}
    </style>
</head>
<body bgcolor='#0f1111' style='margin: 0; padding: 0; background-color: #0f1111 !important;'>
    <!-- Outer Table Container -->
    <table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#0f1111' style='background-color: #0f1111 !important; width: 100%;'>
        <tr>
            <td align='center' bgcolor='#0f1111' style='padding: 24px; background-color: #0f1111 !important;'>
                
                <!-- Main Email Card -->
                <table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0' style='max-width: 580px; background-color: #0f1111 !important;'>
                    
                    <!-- White Header Logo PNG -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; padding-bottom: 28px;'>
                            <img src='https://cdn.freebiesupply.com/images/large/2x/amazon-logo-white.png' 
                                 alt='Amazon' 
                                 width='115' 
                                 style='width: 115px; height: auto; display: block; border: 0;'>
                        </td>
                    </tr>

                    <!-- Greeting -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 18px; font-weight: 600; color: #ffffff !important; padding-bottom: 20px; letter-spacing: -0.2px;'>
                            {userName ?? "Customer"},
                        </td>
                    </tr>

                    <!-- Intro -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 15px; color: #ffffff !important; line-height: 1.4; padding-bottom: 20px;'>
                            Someone who knows your password is attempting to sign-in to your account.
                        </td>
                    </tr>

                    <!-- Details -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 15px; color: #ffffff !important; line-height: 1.6; padding-bottom: 28px;'>
                            <strong>When:</strong> {timestamp}<br>
                            <strong>Device:</strong> {deviceDetails}<br>
                            <strong>Near:</strong> {locationDetails}
                        </td>
                    </tr>

                    <!-- Code Label -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 15px; font-weight: bold; color: #ffffff !important; padding-bottom: 10px;'>
                            If this was you, your verification code is:
                        </td>
                    </tr>

                    <!-- OTP Code -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 26px; font-weight: bold; letter-spacing: 0.5px; color: #ffffff !important; padding-bottom: 28px;'>
                            {verificationCode}
                        </td>
                    </tr>

                    <!-- Security Notice -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 15px; color: #ffffff !important; line-height: 1.5; padding-bottom: 28px;'>
                            If you didn't request it: <a href='{denyLink}' style='color: #55b5d9; text-decoration: none;'>click here to deny</a>.<br>
                            Don't share it with others.
                        </td>
                    </tr>

                    <!-- Dark Footer Container -->
                    <tr>
                        <td align='left' bgcolor='#1e293b' style='background-color: #1e293b !important; padding: 20px; border-radius: 4px;'>
                            <table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0'>
                                <tr>
                                    <td style='font-size: 12px; color: #cccccc !important; line-height: 1.5;'>
                                        &copy;{DateTime.UtcNow.Year} <a href='https://www.amazon.com' style='color: #55b5d9; text-decoration: none;'>Amazon.com</a>, Inc. or its affiliates. Amazon and all related marks are trademarks of <a href='https://www.amazon.com' style='color: #55b5d9; text-decoration: none;'>Amazon.com</a>, Inc. or its affiliates, <a href='https://www.amazon.com' style='color: #55b5d9; text-decoration: none;'>Amazon.com</a>, Inc. 410 Terry Avenue N., Seattle, WA 98109.<br><br>
                                        Is it safe to follow this link?<br>
                                        The link provided in this email starts with <strong>'https://www.amazon.com'</strong>. If you prefer, copy the following link and paste it into a browser to view.<br><br>
                                        <a href='{denyLink}' style='color: #55b5d9; text-decoration: none; word-break: break-all;'>{denyLink}</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding-top: 15px;'>
                                        <!-- Footer White Amazon Logo PNG -->
                                        <img src='https://upload.wikimedia.org/wikipedia/commons/e/eb/Amazon_shopping_smile_logo-app.svg?utm_source=commons.wikimedia.org&utm_campaign=index&utm_content=original' 
                                             alt='Amazon' 
                                             width='50' 
                                             style='width: 50px; height: auto; display: block; border: 0;'>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                </table>

            </td>
        </tr>
    </table>
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
<html lang='en'>
<head>
    <meta charset='utf-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <meta name='color-scheme' content='dark' />
    <meta name='supported-color-schemes' content='dark' />
    <style>
        body, table, td, div {{
            font-family: Arial, sans-serif !important;
        }}
    </style>
</head>
<body bgcolor='#0f1111' style='margin: 0; padding: 0; background-color: #0f1111 !important;'>
    <!-- Outer Table Container -->
    <table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0' bgcolor='#0f1111' style='background-color: #0f1111 !important; width: 100%;'>
        <tr>
            <td align='center' bgcolor='#0f1111' style='padding: 24px; background-color: #0f1111 !important;'>
                
                <!-- Main Email Card -->
                <table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0' style='max-width: 580px; background-color: #0f1111 !important;'>
                    
                    <!-- White Header Logo PNG -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; padding-bottom: 28px;'>
                            <img src='https://cdn.freebiesupply.com/images/large/2x/amazon-logo-white.png' 
                                 alt='Amazon' 
                                 width='115' 
                                 style='width: 115px; height: auto; display: block; border: 0;'>
                        </td>
                    </tr>

                    <!-- Greeting -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 18px; font-weight: 600; color: #ffffff !important; padding-bottom: 20px; letter-spacing: -0.2px;'>
                            {userName ?? "Customer"},
                        </td>
                    </tr>

                    <!-- Intro -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 15px; color: #ffffff !important; line-height: 1.4; padding-bottom: 20px;'>
                            Someone is attempting to reset the password of your account.
                        </td>
                    </tr>

                    <!-- Details -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 15px; color: #ffffff !important; line-height: 1.6; padding-bottom: 28px;'>
                            <strong>When:</strong> {timestamp}<br>
                            <strong>Device:</strong> {deviceDetails}<br>
                            <strong>Near:</strong> {locationDetails}
                        </td>
                    </tr>

                    <!-- Code Label -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 15px; font-weight: bold; color: #ffffff !important; padding-bottom: 10px;'>
                            If this was you, your verification code is:
                        </td>
                    </tr>

                    <!-- OTP Code -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 26px; font-weight: bold; letter-spacing: 0.5px; color: #ffffff !important; padding-bottom: 28px;'>
                            {verificationCode}
                        </td>
                    </tr>

                    <!-- Security Notice -->
                    <tr>
                        <td align='left' bgcolor='#0f1111' style='background-color: #0f1111 !important; font-size: 15px; color: #ffffff !important; line-height: 1.5; padding-bottom: 28px;'>
                            If you didn't request it: <a href='{denyLink}' style='color: #55b5d9; text-decoration: none;'>click here to deny</a>.<br>
                            Don't share it with others.
                        </td>
                    </tr>

                    <!-- Dark Footer Container -->
                    <tr>
                        <td align='left' bgcolor='#1e293b' style='background-color: #1e293b !important; padding: 20px; border-radius: 4px;'>
                            <table role='presentation' width='100%' border='0' cellspacing='0' cellpadding='0'>
                                <tr>
                                    <td style='font-size: 12px; color: #cccccc !important; line-height: 1.5;'>
                                        &copy;{DateTime.UtcNow.Year} <a href='https://www.amazon.com' style='color: #55b5d9; text-decoration: none;'>Amazon.com</a>, Inc. or its affiliates. Amazon and all related marks are trademarks of <a href='https://www.amazon.com' style='color: #55b5d9; text-decoration: none;'>Amazon.com</a>, Inc. or its affiliates, <a href='https://www.amazon.com' style='color: #55b5d9; text-decoration: none;'>Amazon.com</a>, Inc. 410 Terry Avenue N., Seattle, WA 98109.<br><br>
                                        Is it safe to follow this link?<br>
                                        The link provided in this email starts with <strong>'https://www.amazon.com'</strong>. If you prefer, copy the following link and paste it into a browser to view.<br><br>
                                        <a href='{denyLink}' style='color: #55b5d9; text-decoration: none; word-break: break-all;'>{denyLink}</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding-top: 15px;'>
                                        <!-- Footer White Amazon Logo PNG -->
                                        <img src='https://upload.wikimedia.org/wikipedia/commons/e/eb/Amazon_shopping_smile_logo-app.svg?utm_source=commons.wikimedia.org&utm_campaign=index&utm_content=original' 
                                             alt='Amazon' 
                                             width='50' 
                                             style='width: 50px; height: auto; display: block; border: 0;'>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                </table>

            </td>
        </tr>
    </table>
</body>
</html>";

        }
    }
}   