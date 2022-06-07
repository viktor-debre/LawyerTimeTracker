using System;
using Microsoft.EntityFrameworkCore;

namespace LawyerTimeTracker.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Issue> Issues { get; set; }

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

            string adminEmail = "dev@nure.ua";
            string adminName = "Viktor";
            string adminPassword = "123456";
            
            Role adminRole = new Role { Name = adminRoleName }; 
            Role userRole = new Role { Name = userRoleName };
            
            User adminUser = new User { Id = 1, Email = adminEmail, Name = adminName, Password = adminPassword, RoleName = adminRole.Name };
            
            Issue firstTask = new Issue
            {
                Id = 1, Title = "Test issue 1", TypeOfTask = "Lawyer documentation", UserId = 1
            };
            Issue secondTask = new Issue
            {
                Id = 2, Title = "Test issue 2", TypeOfTask = "Lawyer documentation", UserId = 1,
                StartTime = DateTime.Now
            };
            Issue thirdTask = new Issue
            {
                Id = 3, Title = "Test issue 3", TypeOfTask = "Lawyer documentation", UserId = 1,
                StartTime = DateTime.Now, EndTime = DateTime.Now.AddMinutes(30)
            };
            Issue fifthTask = new Issue
            {
                Id = 4, Title = "Test issue 4", TypeOfTask = "Lawyer documentation", UserId = 1
            };
            
            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole });
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });
            modelBuilder.Entity<Issue>().HasData(new Issue[] { firstTask, secondTask, thirdTask, fifthTask });
            base.OnModelCreating(modelBuilder);
        }
    }
}