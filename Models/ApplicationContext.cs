using Microsoft.EntityFrameworkCore;

namespace LawyerTimeTracker.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Issue> Issues { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}