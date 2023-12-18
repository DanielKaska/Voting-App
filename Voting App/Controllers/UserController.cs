using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Claims;
using Voting_App.Entities;
using Voting_App.Models;
using Voting_App.Services;

namespace Voting_App.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        readonly IPasswordHasher<User> hasher;
        readonly VotingDbContext context;
        readonly UserService userService;

        public UserController(IPasswordHasher<User> _hasher, VotingDbContext _context, UserService _userService)
        {
            hasher = _hasher;
            context = _context;
            userService = _userService;
        }

        [HttpPost("register")]
        public ActionResult CreateUser([FromBody] UserDto userDto)
        {
            if (userDto.Email.IsNullOrEmpty() || userDto.Password.IsNullOrEmpty() || userDto.Nickname.IsNullOrEmpty())
            {
                return BadRequest();
            }

            if(context.users.FirstOrDefault(r => r.Email == userDto.Email) != null)
            {
                return BadRequest("user exists");
            }
            
            var user = new User()
            {
                Email = userDto.Email,
                Nickname = userDto.Nickname,
            };

            user.Password = hasher.HashPassword(user, userDto.Password);
            user.Role = "user";
            context.Add(user);
            context.SaveChanges();

            return Ok();

        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto loginDto)
        {
            var user = context.users.FirstOrDefault(u => u.Email == loginDto.Password); //get user from database

            if(user == null)
            {
                return BadRequest("User not found");
            }

            var result = hasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
            
            if(result == PasswordVerificationResult.Success) 
            {
                var token = userService.GenerateToken(user);

                return Ok(token);
            }

            return BadRequest();
        }


    }
}
