using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Queries;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Controllers
{
    // BusinessController sınıfı, işletme işlemlerini gerçekleştirmek için kullanılır.
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
                var result = await _mediator.Send(command); // Komutu MediatR aracılığıyla işler.
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
                    HttpContext.Session.SetInt32("BusinessOwnerId", businessOwner.BusinessOwnerId); // İşletme sahibi ID'sini oturuma kaydeder.

                    return RedirectToAction("BusinessProfile");
                }

                ModelState.AddModelError("", "Geçersiz giriş bilgileri veya hesabınız henüz onaylanmamış.");
            }

            // Model doğrulama başarısızsa formu tekrar gösterir.
            return View(query);
        }

        // Kayıt başarılı sayfasını gösterir.
        public IActionResult Success()
        {
            return View();
        }

        // Giriş yapma
        private async Task SignInUser(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        }

        // İşletme sahibinin ID'sini oturumdan alır.
        private int? GetBusinessOwnerId()
        {
            return HttpContext.Session.GetInt32("BusinessOwnerId");
        }

        [HttpGet]
        // İşletmeci profilini gösterir.
        public async Task<IActionResult> BusinessProfile()
        {
            var businessOwnerId = GetBusinessOwnerId();
            if (!businessOwnerId.HasValue)
            {
                return RedirectToAction("Login");
            }

            var query = new GetBusinessProfileQuery(businessOwnerId.Value);
            var businessOwner = await _mediator.Send(query);

            if (businessOwner == null)
            {
                return NotFound();
            }

            // Modeli ViewModel'e dönüştür
            var model = new BusinessProfileViewModel
            {
                BusinessOwnerId = businessOwner.BusinessOwnerId,
                Email = businessOwner.Email,
                PhoneNumber = businessOwner.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // İşletmeci profilini günceller.
        public async Task<IActionResult> UpdateBusinessProfile(BusinessProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var businessOwnerId = GetBusinessOwnerId();
                if (!businessOwnerId.HasValue)
                {
                    return RedirectToAction("Login");
                }

                var command = new UpdateBusinessProfileCommand
                {
                    BusinessOwnerId = businessOwnerId.Value,
                    OldPassword = model.OldPassword,
                    NewPassword = model.NewPassword,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };

                var result = await _mediator.Send(command);

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Bilgiler başarıyla güncellendi.";
                    return RedirectToAction("BusinessProfile");
                }

                TempData["ErrorMessage"] = result.ErrorMessage;
            }

            return View("BusinessProfile", model);
        }

        [HttpGet]
        // Restoran gelir raporunu gösterir.
        public async Task<IActionResult> RestaurantRevenue(DateTime? startDate, DateTime? endDate)
        {
            // İşletmeci ID'sini oturumdan al.
            var businessOwnerId = GetBusinessOwnerId();
            if (!businessOwnerId.HasValue)
            {
                return RedirectToAction("Login");
            }

            var query = new GetRestaurantRevenueQuery
            {
                BusinessOwnerId = businessOwnerId.Value,
                StartDate = startDate,
                EndDate = endDate
            };

            var salesReport = await _mediator.Send(query);
            return View(salesReport);
        }

        [HttpPost]
        // İşletmeci çıkış yapma
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}