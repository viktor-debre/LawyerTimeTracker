using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LawyerTimeTracker.Models;
using LawyerTimeTracker.Services;
using LawyerTimeTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LawyerTimeTracker.Controllers
{
    public class TaskController : Controller
    {
        private ApplicationContext databaseContext;
        private AccountService _accountService;
        private TaskService _taskService;
        public TaskController(ApplicationContext context)
        {
            databaseContext = context;
            _accountService = new AccountService(context);
            _taskService = new TaskService(context);
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
        
        [HttpPost]
        public async Task<IActionResult> StartTask(int id)
        {
            Issue currentIssue = await _taskService.GetTaskById(id);
            currentIssue.StartTime = DateTime.Now;
            await _taskService.UpdateTask(currentIssue);
            return RedirectToAction("MyTasks", "Task");
        }
        
        [HttpPost]
        public async Task<IActionResult> EndTask(int id)
        {
            Issue currentIssue = await _taskService.GetTaskById(id);
            currentIssue.EndTime = DateTime.Now;
            await _taskService.UpdateTask(currentIssue);
            return RedirectToAction("MyTasks", "Task");
        }
        
        [HttpGet]
        public async Task<IActionResult> NewIssue(NewTaskModel newTask)
        {
            ViewBag.AuthorizedUser = await _accountService.GetUserByEmail(User.Identity.Name);
            return PartialView();
        }

        public async Task<IActionResult> NewTask(NewTaskModel newTask)  
        {
            User currentUser = await _accountService.GetUserByEmail(User.Identity.Name);
            await _taskService.AddTask(newTask, currentUser.Id);
            return RedirectToAction("MyTasks", "Task");
        }
    }
}