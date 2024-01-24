using Microsoft.IdentityModel.Tokens;
using RESTAURANT.GATEWAY.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RESTAURANT.GATEWAY.Services
{
    public class JWTService
    {
        private readonly IConfiguration _configuration;

        public JWTService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateSecurityToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value));
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("userId", user.Id)
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public string? ExtractUserIdFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            Console.WriteLine(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenUser = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if(tokenUser != null)
            {
                var userId = tokenUser.Claims.First(claim => claim.Type == "userId");
                if(userId != null)
                {
                    return userId.Value;
                }
                else
                {
                    return null;
                }
            }
            return null;
           
        }
    }
}
