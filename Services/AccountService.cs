using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LawyerTimeTracker.Models;
using LawyerTimeTracker.Models.ViewModels;
using LawyerTimeTracker.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace LawyerTimeTracker.Services
{
    public class AccountService
    {
        private ApplicationContext databaseContext;

        public AccountService(ApplicationContext context)
        {
            databaseContext = context;
        }

        public async Task SaveUser(User user)
        {
            databaseContext.Users.Add(user);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            return await databaseContext.Users.ToListAsync();
        }

        public async Task<List<User>> GetUsersFromOrganization(int organizationId)
        {
            return await databaseContext.Users.Where(userInDb => userInDb.OrganizationId == organizationId)
                .ToListAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await databaseContext.Users
                .FirstOrDefaultAsync(userInDatabase => userInDatabase.Email == email);
            if (user != null)
            {
                user.Organization = await databaseContext.Organizations
                    .FirstOrDefaultAsync(organization => organization.Id == user.OrganizationId);
            }

            return user;
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            return databaseContext.Users
                .Include(userInDatabase => userInDatabase.Role)
                .FirstOrDefaultAsync(userInDatabase =>
                    userInDatabase.Email == email && userInDatabase.Password == password
                ).Result;
        }

        public async Task RegisterUser(User currentAdmin, RegisterModel model,
            ModelStateDictionary modelState)
        {
            User user = await GetUserByEmail(model.Email);
            if (user == null)
            {
                user = new User
                {
                    Email = model.Email, FirstName = model.FirstName, LastName = model.LastName,
                    Password = model.Password, OrganizationId = currentAdmin.OrganizationId
                };
                Role userRole = await GetRoleByName("user");
                if (userRole != null)
                {
                    user.Role = userRole;
                }

                await SaveUser(user);
            }
            else
            {
                modelState.AddModelError("", "The user with this email already exists.");
            }
        }

        public async Task<Role> GetRoleByName(string rolename)
        {
            return databaseContext.Roles.FirstOrDefaultAsync(role => role.Name == rolename).Result;
        }

        public async Task UpdateUser(UpdateAccountModel model)
        {
            User user = await GetUserByEmail(model.Email);
            user.Phonenumber = model.Phonenumber;
            user.Skype = model.Skype;
            if (model.IsImageToDelete)
            {
                user.Image = null;
            } else if(model.Image != null)
            {
                user.Image = model.Image;
            }
            databaseContext.Users.Update(user);
            await databaseContext.SaveChangesAsync();
        }
    }
}