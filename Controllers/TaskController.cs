using Microsoft.AspNetCore.Mvc;

namespace LawyerTimeTracker.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult MyTasks()
        {
            return View();
        }
    }
}