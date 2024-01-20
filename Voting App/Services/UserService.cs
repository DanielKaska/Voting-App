using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Voting_App.Entities;
using Voting_App.Exceptions;

namespace Voting_App.Services
{
    public class UserService
    {

        private readonly JwtSettings jwtSettings;
        private readonly VotingDbContext context;

        public UserService(JwtSettings _jwtSettings, VotingDbContext _context)
        {
            jwtSettings = _jwtSettings;
            context = _context;
        }

        public User GetById(int id)
        {
            var user = context.Users.FirstOrDefault(user => user.Id == id);

            if (user is null)
                throw new NotFoundException("user not found");

            return user;
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
            var token = new JwtSecurityToken(issuer: jwtSettings.Issuer, audience: jwtSettings.Audience, claims: claims, expires: DateTime.UtcNow.AddDays(14), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
