using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.Controllers
{
    public class BusinessController : Controller
    {
        private readonly IMediator _mediator;

        public BusinessController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        // Register sayfasını gösterir.
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Register formunu işler.
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = await _mediator.Send(command);
                if (result)
                {
                    return RedirectToAction("Success");
                }

                ModelState.AddModelError("", "E-posta zaten kullanılıyor.");
            }
            
            // Model doğrulama başarısızsa veya kayıt başarısızsa formu tekrar gösterir.
            return View(command);
        }

        [HttpGet]
        // Login sayfasını gösterir.
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // Login formunu işler.
        public async Task<IActionResult> Login(LoginQuery query)
        {
            if (ModelState.IsValid)
            {
                var businessOwner = await _mediator.Send(query);
                if (businessOwner != null)
                {
                    // Kullanıcı girişini gerçekleştirir.
                    await SignInUser(businessOwner.Email);
                    HttpContext.Session.SetInt32("BusinessOwnerId", businessOwner.BusinessOwnerId);
                    
                    if (businessOwner.Email == "admintiklaye@gmail.com")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "PartnerProduct");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz giriş bilgileri veya hesabınız henüz onaylanmamış.");
                }
            }

            // Model doğrulama başarısızsa formu tekrar gösterir.
            return View(query);
        }

        // Kayıt başarılı sayfasını gösterir.
        public IActionResult Success()
        {
            return View();
        }

        // Kullanıcıyı giriş yapar.
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