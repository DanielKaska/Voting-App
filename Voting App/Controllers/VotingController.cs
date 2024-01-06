using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks.Sources;
using Voting_App.Entities;
using Voting_App.Models;
using Voting_App.Services;

namespace Voting_App.Controllers
{
    [Authorize]
    [Route("vote")]
    
    public class VotingController : Controller
    {
        private readonly VotingDbContext context; // db context
        private readonly IMapper mapper; //automapper 
        private readonly VoteService vs; //vote service
        private readonly UserService us;

        public VotingController(VotingDbContext _context, IMapper _mapper, VoteService _voteService, UserService us)
        {
            context = _context;
            mapper = _mapper;
            vs = _voteService;
            this.us = us;
        }


        [HttpGet("get/{voteId}")]
        public ActionResult Get([FromRoute] int voteId)
        {
            var vote = vs.GetVoteDto(voteId);

            return Ok(vote);
        }

        [HttpPost("create")]
        public ActionResult CreateVote([FromBody] VoteDto dto)
        {
            var claims = User.Claims; //get user claims
            var userId = claims.Where(c => c.Type == "id").FirstOrDefault(); //get user id

            if(userId == null)
                return BadRequest("could not idenditify user");

            var vote = vs.Create(dto, int.Parse(userId.Value));

            context.votes.Add(vote);
            context.SaveChanges();
            return Ok(vote.Id);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Vote([FromBody] VoteAnswerDto dto)
        {
            if (dto is null)
                return BadRequest("dto cant be null");

            var voteId = dto.Id;

            //check if user voted
            var voted = Request.Cookies[dto.Id.ToString()];
            if (voted is not null)
                return BadRequest("you've already voted");

            var vote = vs.GetVoteById(voteId);
            var answer = vs.GetAnswerFromVote(vote, dto.AnswerName);

            var cookieOptions = new CookieOptions() {Expires = DateTime.Now.AddDays(30),Path = "/"};
            Response.Cookies.Append(voteId.ToString(), "voted", cookieOptions);

            answer.VoteCounter += 1;
            context.SaveChanges();

            return Ok();
        }


        [HttpDelete("delete/{voteId}")]
        public ActionResult Delete([FromRoute] int voteId)
        {
            var vote = vs.GetVoteById(voteId);

            if(vote is null)
            {
                return BadRequest("vote not found");
            }

            var voteCreator = us.GetById(vote.CreatedBy); //user that created the vote

            var userClaims = User.Claims; //claims

            var userId = int.Parse(User.Claims.Where(c => c.Type == "id").FirstOrDefault().Value);
            var role = User.Claims.Where(c => c.Type == "role").FirstOrDefault().Value;

            if (userId == voteCreator.Id || role == "Admin") //check if the vote was created by client sending request
            {
                context.votes.Remove(vote); //if so, remove the vote from database
                context.SaveChanges();
                
                return Ok("vote removed");
            }
            

            return BadRequest("Something went wrong");
        }



    }
}
