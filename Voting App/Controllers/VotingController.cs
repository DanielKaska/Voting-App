using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
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

        [HttpPost("create")]
        public ActionResult CreateVote([FromBody] VoteDto dto)
        {
            if(dto is null)
            {
                return BadRequest();
            }

            var vote = mapper.Map<Vote>(dto);

            var user = context.users.FirstOrDefault(u => u.Id == 1);

            vote.CreatedBy = user;
            context.votes.Add(vote);
            context.SaveChanges();
            return Ok(vote.Id);

        }

        

        [HttpPost]
        public ActionResult Vote([FromBody] int voteId)
        {
            var clientIp = Request.HttpContext.Connection.RemoteIpAddress;
            var wantedVote = context.votes.FirstOrDefault(v => v.Id == voteId);

            return Ok(); 
        }



    }
}
