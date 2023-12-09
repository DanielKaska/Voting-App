using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
    }
}
