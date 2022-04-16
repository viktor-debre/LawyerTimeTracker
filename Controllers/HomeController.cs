using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LawyerTimeTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace LawyerTimeTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext db;
        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
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