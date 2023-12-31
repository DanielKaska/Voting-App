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
        private readonly VotingDbContext context;
        private readonly IMapper mapper;
        private readonly VoteService vs; //vote service
        public VotingController(VotingDbContext _context, IMapper _mapper, VoteService _voteService)
        {
            context = _context;
            mapper = _mapper;
            vs = _voteService;
        }


        [HttpGet("get/{voteId}")]
        public ActionResult Get([FromRoute] int voteId)
        {
            //var res = context.votes.Where(v => v.Id <= maxIndex).Where(v => v.Id > maxIndex - 10);

            var vote = vs.GetById(voteId);

            return Ok(vote);
        }

        [HttpPost("create")]
        public ActionResult CreateVote([FromBody] VoteDto dto)
        {
            var claims = User.Claims; //get user claims
            var userId = claims.Where(c => c.Type == "id").FirstOrDefault(); //get user id

            if(userId == null)
            {
                return BadRequest();
            }

            var vote = vs.Create(dto, int.Parse(userId.Value));

            context.votes.Add(vote);
            context.SaveChanges();
            return Ok(vote.Id);

        }



        [HttpPost]
        public ActionResult Vote([FromBody] int voteId)
        {
            var clientIp = Request.HttpContext.Connection.RemoteIpAddress;
            var vote = context.votes.FirstOrDefault(v => v.Id == voteId);


            return Ok();
        }


        [HttpDelete("delete/{voteId}")]
        public ActionResult Delete([FromRoute] int voteId)
        {
            var vote = context.votes.FirstOrDefault(v => v.Id == voteId);

            if(vote is null)
            {
                return BadRequest("vote not found");
            }

            var voteCreator = context.users.FirstOrDefault(user => user.Id == vote.CreatedBy); //user that created the vote

            var client = User.Claims.ToList(); //client
            var clientId = int.Parse(client[0].Value);
            var clientRole = client[3].Value;

            if (clientId == voteCreator.Id || clientRole == "Admin") //check if the vote was created by client sending request
            {
                context.votes.Remove(vote); //if so, remove the vote from database
                context.SaveChanges();
                
                return Ok("vote removed");
            }
            

            return BadRequest("Something went wrong");
        }



    }
}
