using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using Voting_App.Entities;
using Voting_App.Models;

namespace Voting_App.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        IPasswordHasher<User> hasher;
        VotingDbContext context;
        public UserController(IPasswordHasher<User> _hasher, VotingDbContext _context)
        {
            hasher = _hasher;
            context = _context;
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
            context.Add(user);
            context.SaveChanges();


            return Ok();

        }


    }
}
