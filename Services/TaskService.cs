using System.Threading.Tasks;
using LawyerTimeTracker.Models;

namespace LawyerTimeTracker.Services
{
    public class TaskService
    {
        private ApplicationContext databaseContext;

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
    }
}