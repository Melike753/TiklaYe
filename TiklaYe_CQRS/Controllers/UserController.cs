using MediatR;
using Microsoft.AspNetCore.Mvc;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;

        public UserController(IMediator mediator, ApplicationDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        // Kullanıcı profilini görüntüleme
        public async Task<IActionResult> Profile()
        {
            // Kullanıcı oturum açmamışsa giriş sayfasına yönlendir
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Oturumdan kullanıcı ID'sini al
            var userId = GetUserIdFromSession();

            // Kullanıcı profil verilerini almak için query oluştur
            var query = new GetProfileQuery(userId);
            var model = await _mediator.Send(query);

            // Profil verisi bulunamazsa 404 Not Found döndür
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            // Model geçerli değilse profil sayfasına geri dön
            if (!ModelState.IsValid)
            {
                return View("Profile", model);
            }

            var userId = GetUserIdFromSession();

            var userProfileQuery = new GetProfileQuery(userId);
            var userProfile = await _mediator.Send(userProfileQuery);

            if (userProfile == null)
            {
                return NotFound();
            }

            // Profil güncelleme komutunu oluştur
            var command = new UpdateProfileCommand
            {
                UserId = userId,
                Username = model.Username,
                Email = model.Email,
                Mobile = model.Mobile,
                Address = model.Address,
                Password = model.Password
            };

            // Profil güncelleme komutunu gönder
            var result = await _mediator.Send(command);

            if (result)
            {
                // Güncelleme başarılıysa başarı mesajı göster ve profil sayfasına yönlendir
                TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";
                return RedirectToAction("Profile");
            }
            else
            {
                // Güncelleme başarısızsa hata mesajı göster
                ModelState.AddModelError(string.Empty, "Profil güncellenemedi.");
                return View("Profile", model);
            }
        }

        // Kullanıcının satın alma geçmişini görüntüleme
        public async Task<IActionResult> PurchaseHistory()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = GetUserIdFromSession();

            // Satın alma geçmişi verilerini almak için query oluştur
            var query = new GetPurchaseHistoryQuery(userId);
            var purchases = await _mediator.Send(query);

            // Satın alma geçmişi verilerini görüntülemek için view'e gönder
            return View(purchases);
        }

        // PDF İndirme İşlemi
        public async Task<IActionResult> DownloadInvoice(int id)
        {
            // Fatura indirme komutunu oluştur
            var command = new DownloadInvoiceCommand
            {
                PurchaseId = id
            };

            // Komutu gönder ve PDF verilerini al
            var pdfBytes = await _mediator.Send(command);

            if (pdfBytes == null)
            {
                return NotFound();
            }

            // PDF verilerini dosya olarak döndür
            return File(pdfBytes, "application/pdf", "TıklaYeFatura.pdf");
        }

        private int GetUserIdFromSession()
        {
            // Kullanıcı kimlik doğrulama bilgilerini kullanarak UserId'yi alıyoruz.
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                throw new InvalidOperationException("Kullanıcı bulunamadı.");
            }
            return userId.Value;
        }
    }
}