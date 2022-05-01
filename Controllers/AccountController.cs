using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LawyerTimeTracker.ViewModels;
using LawyerTimeTracker.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
                    .FirstOrDefaultAsync(userInDatabase =>
                        userInDatabase.Name == model.Name && userInDatabase.Password == model.Password
                        );
                if (user != null)
                {
                    await Authenticate(model.Name);
                    
                    return RedirectToAction("Index","Home");
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
                User user = await databaseContext.Users
                    .FirstOrDefaultAsync(userInDatabase => userInDatabase.Name == model.Name);
                if (user == null)
                {
                    databaseContext.Users.Add(new User { Name = model.Name, Password = model.Password });
                    await databaseContext.SaveChangesAsync();
 
                    await Authenticate(model.Name);
 
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "The user with this nickname has already existed.");
                }
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,userName)
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