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
            
            Role adminRole = new Role { Id = 1, Name = adminRoleName };
            Role userRole = new Role { Id = 2, Name = userRoleName };
            
            User adminUser = new User { Id = 1, Name = adminName, Password = adminPassword, RoleId = adminRole.Id };
            
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            base.OnModelCreating(modelBuilder);
        }
    }
}