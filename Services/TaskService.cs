using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LawyerTimeTracker.Models;
using LawyerTimeTracker.ViewModels;

namespace LawyerTimeTracker.Services
{
    public class TaskService
    {
        private ApplicationContext databaseContext;
        private AccountService _accountService;

        public TaskService(ApplicationContext context)
        {
            databaseContext = context;
            _accountService = new AccountService(context);
        }

        public async Task<Issue> GetTaskById(int id)
        {
            return databaseContext.Issues.Find(id);
        }

        public List<Issue> GetTasksForUser(int userId)
        {
            return databaseContext.Issues.Where(i => i.UserId == userId).ToList();
        }

        public List<Issue> GetAllOrganizationTasks(int organizationId)
        {
            List<Issue> tasks = new List<Issue>();
            foreach (var user in _accountService.GetUsersFromOrganization(organizationId))
            {
                tasks.AddRange(GetTasksForUser(user.Id));
            }

            return tasks;
        }

        public async Task UpdateTask(Issue issue)
        {
            databaseContext.Issues.Update(issue);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<Issue> DuplicateTask(NewTaskModel issue)
        {
            Issue currentIssue = await GetTaskById(issue.Id);
            currentIssue.Title = issue.Title;
            currentIssue.Description = issue.Description;
            currentIssue.TypeOfTask = issue.TypeOfTask;
            return currentIssue;
        }

        public async Task DeleteTask(int id)
        {
            databaseContext.Issues.Remove(GetTaskById(id).Result);
            await databaseContext.SaveChangesAsync();
        }

        public async Task AddTaskForUser(NewTaskModel newTask, string email)
        {
            User currentUser = await _accountService.GetUserByEmail(email);
            int userId = currentUser.Id;
            Issue issue = new Issue
            {
                Title = newTask.Title,
                Description = newTask.Description,
                TypeOfTask = newTask.TypeOfTask,
                UserId = userId
            };
            databaseContext.Issues.Add(issue);
            await databaseContext.SaveChangesAsync();
        }
    }
}