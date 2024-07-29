using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginCommandHandler _loginHandler;
        private readonly LogoutCommandHandler _logoutHandler;

        public LoginController(LoginCommandHandler loginHandler, LogoutCommandHandler logoutHandler)
        {
            _loginHandler = loginHandler;
            _logoutHandler = logoutHandler;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = new LoginCommand
                    {
                        Email = model.Email,
                        Password = model.Password
                    };

                    var user = await _loginHandler.Handle(command);

                    if (user == null)
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                    }

                    await SignInUser(user.Email);

                    HttpContext.Session.SetInt32("UserId", user.UserId);

                    if (user.Email == "admintiklaye@gmail.com")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var command = new LogoutCommand();
            await _logoutHandler.Handle(command);

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Clear(); 

            return RedirectToAction("Login");
        }

        private async Task SignInUser(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true 
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}