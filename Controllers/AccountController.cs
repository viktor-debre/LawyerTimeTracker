using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LawyerTimeTracker.Models;
using LawyerTimeTracker.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LawyerTimeTracker.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext databaseContext;

        public AccountController(ApplicationContext context)
        {
            databaseContext = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await databaseContext.Users
                    .Include(userInDatabase => userInDatabase.Role)
                    .FirstOrDefaultAsync(userInDatabase =>
                        userInDatabase.Email == model.Email && userInDatabase.Password == model.Password
                    );
                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Incorrect name or password");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (Regex.IsMatch(model.Email, @"\w+\@\D+\.\D+$"))
                {
                    User userByEmail = await databaseContext.Users
                        .FirstOrDefaultAsync(userInDatabase => userInDatabase.Email == model.Email);
                    if (userByEmail == null)
                    {
                        User userByName = await databaseContext.Users
                            .FirstOrDefaultAsync(userInDatabase => userInDatabase.Name == model.Name);
                        if (userByName == null)
                        {
                            userByName = new User { Email = model.Email, Name = model.Name, Password = model.Password };
                            Role userRole =
                                await databaseContext.Roles.FirstOrDefaultAsync(role => role.Name == "user");
                            if (userRole != null)
                            {
                                userByName.Role = userRole;
                            }

                            databaseContext.Users.Add(userByName);
                            await databaseContext.SaveChangesAsync();
                            // TODO: oleksiii: write message 'User registered successfully'
                            return RedirectToAction("ViewUsers", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "The user with this nickname already exists.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The user with this email already exists.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong email form.");
                }
            }

            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims,
                "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}