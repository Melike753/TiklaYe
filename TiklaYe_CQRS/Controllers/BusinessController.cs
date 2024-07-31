using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Controllers
{
    // İşletme sahiplerinin kayıt, giriş ve çıkış işlemlerini yönetmek için. 
    public class BusinessController : Controller
    {
        private readonly RegisterBusinessOwnerCommandHandler _registerHandler; // Kayıt işlemlerini yönetmek için
        private readonly LoginBusinessOwnerCommandHandler _loginHandler; // Giriş işlemlerini yönetmek için

        //  Dependency Injection
        public BusinessController(
            RegisterBusinessOwnerCommandHandler registerHandler,
            LoginBusinessOwnerCommandHandler loginHandler)
        {
            _registerHandler = registerHandler;
            _loginHandler = loginHandler;
        }

        [HttpGet]
        // Kayıt sayfasını görüntüler.
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(BusinessOwner model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = new RegisterBusinessOwnerCommand
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Password = model.Password
                    };

                    await _registerHandler.Handle(command);
                    await SignInUser(model.Email);
                    return RedirectToAction("Success");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return View(model);
        }

        [HttpGet]
        // Giriş sayfasını görüntüler.
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(BusinessLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var command = new LoginBusinessOwnerCommand
                    {
                        Email = model.Email,
                        Password = model.Password
                    };

                    var businessOwner = await _loginHandler.Handle(command);

                    if (businessOwner == null)
                    {
                        ModelState.AddModelError("", "Geçersiz e-posta veya şifre.");
                        return View(model);
                    }

                    // Giriş işlemi başarılı
                    await SignInUser(businessOwner.Email);

                    HttpContext.Session.SetInt32("BusinessOwnerId", businessOwner.BusinessOwnerId);

                    if (businessOwner.Email == "admintiklaye@gmail.com")
                    {
                        // Admin sayfasına yönlendir
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        // Normal işletmeciyi işletmeci ana sayfasına yönlendir
                        return RedirectToAction("Index", "PartnerProduct");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);
        }

        // Kayıt işlemi başarılı olduğunda gösterilecek sayfayı görüntüler.
        public IActionResult Success()
        {
            return View();
        }

        //  Kullanıcıyı kimlik doğrulama sistemi ile oturum açmak için kullanılır. 
        private async Task SignInUser(string email)
        {
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Email, email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                // İsteğe bağlı olarak buraya ek özellikler eklenebilir.
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}