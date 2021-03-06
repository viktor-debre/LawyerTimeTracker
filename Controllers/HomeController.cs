using System.Diagnostics;
using System.Threading.Tasks;
using LawyerTimeTracker.Models;
using LawyerTimeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LawyerTimeTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly AccountService _accountService;

        public HomeController(ApplicationContext context)
        {
            _accountService = new AccountService(context);
        }

        [Authorize]
        public async Task<IActionResult> Help()
        {
            ViewBag.AuthorizedUser = await _accountService.GetUserByEmail(User.Identity.Name);
            return View();
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ViewUsers()
        {
            User currentAdmin = await _accountService.GetUserByEmail(User.Identity.Name);
            ViewBag.AuthorizedUser = currentAdmin;
            return View(_accountService.GetUsersFromOrganization(currentAdmin.OrganizationId));
        }

        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            ViewBag.AuthorizedUser = await _accountService.GetUserByEmail(User.Identity.Name);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}