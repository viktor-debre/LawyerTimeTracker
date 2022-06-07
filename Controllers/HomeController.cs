using System.Diagnostics;
using System.Threading.Tasks;
using LawyerTimeTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LawyerTimeTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext databaseContext;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            databaseContext = context;
        }

        [Authorize]
        public async Task<IActionResult> Help()
        {
            ViewBag.AuthorizedUser = await databaseContext.Users
                .FirstOrDefaultAsync(userInDatabase => userInDatabase.Email == User.Identity.Name);
            return View();
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ViewUsers()
        {
            ViewBag.AuthorizedUser = await databaseContext.Users
                .FirstOrDefaultAsync(userInDatabase => userInDatabase.Email == User.Identity.Name);
            return View(await databaseContext.Users.ToListAsync());
        }

        public async Task<IActionResult> GetProfile()
        {
            ViewBag.AuthorizedUser = await databaseContext.Users
                .FirstOrDefaultAsync(userInDatabase => userInDatabase.Email == User.Identity.Name);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}