using System.Linq;
using System.Threading.Tasks;
using LawyerTimeTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LawyerTimeTracker.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationContext databaseContext;
        public TaskController(ApplicationContext context)
        {
            databaseContext = context;
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> MyTasks()
        {
            return View(await databaseContext.Issues.ToListAsync());
        }
    }
}