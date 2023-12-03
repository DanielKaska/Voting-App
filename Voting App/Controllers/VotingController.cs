using Microsoft.AspNetCore.Mvc;

namespace Voting_App.Controllers
{
    public class VotingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
