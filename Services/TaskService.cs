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
        }

        public async Task<Issue> GetTaskById(int id)
        {
            return databaseContext.Issues.Find(id);
        }
        
        public async Task UpdateTask(Issue issue)
        {
            databaseContext.Issues.Update(issue);
            await databaseContext.SaveChangesAsync();
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