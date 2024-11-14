using Amazon_eCommerce_API.Models.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Amazon_eCommerce_API.Services
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
            _expirationMinutes = int.Parse(configuration["JwtSettings:ExpirationMinutes"]);
        }

      

        public string GenerateToken(User user)
        {

            var claims = new[]

            {  new Claim(JwtRegisteredClaimNames.Sub, user.Username),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim("role", user.RoleId.ToString())// Role information
            
            };


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
