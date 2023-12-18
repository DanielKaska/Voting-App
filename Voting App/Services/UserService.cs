using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Voting_App.Entities;

namespace Voting_App.Services
{
    public class UserService
    {

        readonly JwtSettings jwtSettings;
        public UserService(JwtSettings _jwtSettings)
        {
            jwtSettings = _jwtSettings;
        }


        public string GenerateToken(User user)
        {

            List<Claim> claims = new()
            {
                new Claim("id", user.Id.ToString()),
                new Claim("nick", user.Nickname),
                new Claim("email", user.Email),
                new Claim("role", user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddDays(14), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
