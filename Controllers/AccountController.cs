using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using LawyerTimeTracker.Models;
using LawyerTimeTracker.Services;
using LawyerTimeTracker.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace LawyerTimeTracker.Controllers
{
    public class AccountController : Controller
    {
        private AccountService _service;

        public AccountController(ApplicationContext context)
        {
            _service = new AccountService(context);
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
                User user = await _service.GetUserByEmailAndPassword(model.Email, model.Password);
                if (user != null)
                {
                    await Authenticate(user);

                    return RedirectToAction("MyTasks", "Task");
                }

                ModelState.AddModelError("", "Incorrect name or password");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ViewBag.AuthorizedUser = await _service.GetUserByEmail(User.Identity.Name);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            ViewBag.AuthorizedUser = await _service.GetUserByEmail(User.Identity.Name);
            if (ModelState.IsValid)
            {
                User user = await _service.GetUserByEmail(model.Email);
                if (user == null)
                {
                    user = new User
                    {
                        Email = model.Email, FirstName = model.FirstName, LastName = model.LastName,
                        Password = model.Password
                    };
                    Role userRole = await _service.GetRoleByName("user");
                    if (userRole != null)
                    {
                        user.Role = userRole;
                    }

                    await _service.SaveUser(user);
                    return RedirectToAction("ViewUsers", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "The user with this email already exists.");
                }
            }

            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
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
