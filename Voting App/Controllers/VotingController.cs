using Microsoft.AspNetCore.Mvc;
using Voting_App.Entities;

namespace Voting_App.Controllers
{
   
    [Route("/vote")]
    public class VotingController : Controller
    {
        VotingDbContext context;

        public VotingController(VotingDbContext _context)
        {
            context = _context;
        }

        [HttpGet("get/{page}")]
        public ActionResult GetAllVotes([FromRoute] int page)
        {
            if(page <= 0)
            {
                return BadRequest();
            }

            page *= 10;

            var votes = context.votes.ToList();

            var paginatedVotes = new List<Vote>();

            for(int i = page-10; i<page; i++)
            {
                paginatedVotes.Add(votes[i]);
            }

            return Ok(paginatedVotes);
        }
    }
}
