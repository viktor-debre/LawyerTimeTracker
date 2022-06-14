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
        
        [HttpPost]
        public async Task<IActionResult> NewTask(NewTaskModel newTask)  
        {
            await _taskService.AddTaskForUser(newTask, User.Identity.Name);
            return RedirectToAction("MyTasks", "Task");
        }
        
        [HttpPost]
        public async Task<IActionResult> SingleIssue(NewTaskModel issue)
        {
            return PartialView(issue);
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateTask(NewTaskModel issue)
        {
            Issue currentIssue = await _taskService.GetTaskById(issue.Id);
            currentIssue.Title = issue.Title;
            currentIssue.Description = issue.Description;
            currentIssue.TypeOfTask = issue.TypeOfTask;
            await _taskService.UpdateTask(currentIssue);
            return RedirectToAction("MyTasks", "Task");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int id)
        {
            await _taskService.DeleteTask(id);
            return RedirectToAction("MyTasks", "Task");
        }
    }
}