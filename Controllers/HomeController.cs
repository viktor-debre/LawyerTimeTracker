using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LawyerTimeTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Index()
        {
            return Content(User.Identity.Name);
        }
        
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            databaseContext.Users.Add(user);
            await databaseContext.SaveChangesAsync();
            return RedirectToAction("ViewUser");
        }
        public async Task<IActionResult> ViewUser()
        {
            return View(await databaseContext.Users.ToListAsync());
            //return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult Help()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}