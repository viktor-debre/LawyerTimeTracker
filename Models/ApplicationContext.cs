using Microsoft.EntityFrameworkCore;

namespace LawyerTimeTracker.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Need remove in future, required for better testing
            string adminRoleName = "admin";
            string userRoleName = "user";
 
            string adminName = "Viktor";
            string adminPassword = "123456";
            
            Role adminRole = new Role { Name = adminRoleName };
            Role userRole = new Role { Name = userRoleName };
            
            User adminUser = new User { Id = 1, Name = adminName, Password = adminPassword, RoleName = adminRole.Name };

            Issue firstTask = new Issue {Id = 1, Title = "Test issue", TypeOfTask = "Lawyer documentation", UserId = 1};
            
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            modelBuilder.Entity<Issue>().HasData(new Issue[] { firstTask });
            base.OnModelCreating(modelBuilder);
        }
    }
}