using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LawyerTimeTracker.Models;
using LawyerTimeTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LawyerTimeTracker.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationContext databaseContext;
        private AccountService _accountService;
        public TaskController(ApplicationContext context)
        {
            databaseContext = context;
            _accountService = new AccountService(context);
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyTasks()
        {
            User currentUser = await _accountService.GetUserByEmail(User.Identity.Name);
            ViewBag.AuthorizedUser = currentUser;
            var userTasks = await databaseContext.Issues
                .Where(i => i.UserId == currentUser.Id)
                .ToListAsync();
            return View(userTasks);
        }
        
        [HttpPost]
        public async Task<IActionResult> Issues(List<Issue> issues)
        {
            ViewBag.AuthorizedUser = await _accountService.GetUserByEmail(User.Identity.Name);
            return PartialView(issues);
        }
    }
}