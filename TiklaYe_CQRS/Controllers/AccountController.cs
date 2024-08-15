using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.Controllers
{
    // Kullanıcı hesap işlemleri
    public class AccountController : Controller
    {
        private readonly RegisterUserCommandHandler _registerUserCommandHandler;
        private readonly UserExistsQueryHandler _userExistsQueryHandler;
        private readonly LoginCommandHandler _loginHandler;

        public AccountController(RegisterUserCommandHandler registerUserCommandHandler, UserExistsQueryHandler userExistsQueryHandler, LoginCommandHandler loginHandler)
        {
            _registerUserCommandHandler = registerUserCommandHandler;
            _userExistsQueryHandler = userExistsQueryHandler;
            _loginHandler = loginHandler;
        }

        [HttpGet]
        // Kullanıcı kayıt formunu görüntülemek için.
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        // Kullanıcı kayıt işlemini gerçekleştirir.
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                var userExistsQuery = new UserExistsQuery
                {
                    Username = model.Username,
                    Email = model.Email
                };

                //  Kullanıcının var olup olmadığını kontrol eder. 
                if (await _userExistsQueryHandler.Handle(userExistsQuery))
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya e-posta zaten kullanılıyor.");
                    return View(model);
                }

                // Yeni bir kullanıcı oluşturmak için.
                var registerUserCommand = new RegisterUserCommand
                {
                    Name = model.Name,
                    Username = model.Username,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Address = model.Address,
                    PostCode = model.PostCode,
                    Password = model.Password
                };

                // Kullanıcı kayıt işlemini gerçekleştirir.
                await _registerUserCommandHandler.Handle(registerUserCommand);

                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
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
                        ModelState.AddModelError("", "Geçersiz giriş denemesi.");
                        return View(model);
                    }

                    await SignInUser(user);

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
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}   