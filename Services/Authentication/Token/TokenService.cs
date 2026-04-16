using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Amazon_eCommerce_API.Models.DTO_s.Authentication.Token;
using Microsoft.IdentityModel.Tokens;

namespace Amazon_eCommerce_API.Services.Authentication.Token
{


    public class TokenService : ITokenService
    {

        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;
        public TokenService(IConfiguration configuration)
        {
            _secretKey = configuration["JwtSettings:SecretKey"];
            _issuer = configuration["JwtSettings:Issuer"];
            _audience = configuration["JwtSettings:Audience"];
            _expirationMinutes = int.TryParse(configuration["JwtSettings:ExpirationMinutes"], 
                out var minutes)? minutes : 60;
        }

      

        public string GenerateToken(TokenRequestDto request)
        {

            var claims = new List<Claim>

            {  
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim(ClaimTypes.NameIdentifier, request.UserId.ToString()),
              new Claim(ClaimTypes.Email, request.Email),
              new Claim(ClaimTypes.Role, request.Role.ToString()),
            };

            
            if(!string.IsNullOrEmpty(request.DisplayName))
                claims.Add(new Claim("displayName", request.DisplayName));
            
            if(!string.IsNullOrEmpty(request.StoreName))
                claims.Add(new Claim("storeName", request.StoreName));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_expirationMinutes),
                signingCredentials: creds);



            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
