using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LawyerTimeTracker.Models;
using LawyerTimeTracker.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace LawyerTimeTracker.Services
{
    public class AccountService
    {
        private readonly ApplicationContext _databaseContext;

        public AccountService(ApplicationContext context)
        {
            _databaseContext = context;
        }

        private async Task SaveUser(User user)
        {
            _databaseContext.Users.Add(user);
            await _databaseContext.SaveChangesAsync();
        }

        public List<User> GetUsersFromOrganization(int organizationId)
        {
            return _databaseContext.Users.Where(userInDb => userInDb.OrganizationId == organizationId)
                .ToList();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await _databaseContext.Users
                .FirstOrDefaultAsync(userInDatabase => userInDatabase.Email == email);
            if (user != null)
            {
                user.Organization = await _databaseContext.Organizations
                    .FirstOrDefaultAsync(organization => organization.Id == user.OrganizationId);
            }

            return user;
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            return _databaseContext.Users
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

        public async Task<Role> GetRoleByName(string roleName)
        {
            return _databaseContext.Roles.FirstOrDefaultAsync(role => role.Name == roleName).Result;
        }

        public async Task UpdateUser(UpdateAccountModel model)
        {
            User user = await GetUserByEmail(model.Email);
            user.PhoneNumber = model.PhoneNumber;
            user.Skype = model.Skype;
            if (model.IsImageToDelete)
            {
                user.Image = null;
            }
            else if (model.Image != null)
            {
                user.Image = model.Image;
            }

            _databaseContext.Users.Update(user);
            await _databaseContext.SaveChangesAsync();
        }
    }
}