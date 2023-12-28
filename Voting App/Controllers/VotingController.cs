using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Voting_App.Entities;
using Voting_App.Models;

namespace Voting_App.Controllers
{
    [Authorize]
    [Route("vote")]
    
    public class VotingController : Controller
    {
        VotingDbContext context;
        private IMapper mapper;

        public VotingController(VotingDbContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
        }


        [HttpGet("get/{page}")]
        public ActionResult ReturnVotes([FromRoute] int page)
        {
            if(page <= 0)
            {
                return BadRequest();
            }

            int maxIndex = page*10;
            var res = context.votes.Where(v => v.Id <= maxIndex).Where(v => v.Id > maxIndex - 10);

            return Ok(res);
        }

        //claims indexes
        //id - 0
        //nick - 1
        //email - 2
        //role - 3

        [HttpPost("create")]
        public ActionResult CreateVote([FromBody] VoteDto dto)
        {
            if(dto is null)
            {
                return BadRequest();
            }

            var vote = mapper.Map<Vote>(dto);

            var userId = User.Claims.ToList()[0].Value;
            var user = context.users.FirstOrDefault(u => u.Id == int.Parse(userId));



            vote.CreatedBy = user.Id;
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




        //claims indexes
        //id - 0
        //nick - 1
        //email - 2
        //role - 3

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
