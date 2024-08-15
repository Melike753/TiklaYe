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
        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var userId = GetUserIdFromSession();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var query = new GetUserProfileQuery(userId.Value);
            var model = await _mediator.Send(query);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // Kullanıcı profilini güncelleme
        // Kullanıcıdan gelen verileri alır, doğrular ve güncelleme işlemini gerçekleştirir.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUserProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("UserProfile", model);
            }

            var userId = GetUserIdFromSession();
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var command = new UpdateUserProfileCommand
            {
                UserId = userId.Value,
                OldPassword = model.OldPassword,
                NewPassword = model.NewPassword,
                Username = model.Username,
                Email = model.Email,
                Mobile = model.Mobile,
                Address = model.Address
            };

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Bilgiler başarıyla güncellendi.";
                return RedirectToAction("UserProfile");
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return View("UserProfile", model);
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
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Satın alma geçmişi verilerini almak için query oluşturur.
            var query = new GetPurchaseHistoryQuery(userId.Value);
            var purchases = await _mediator.Send(query);

            // Satın alma geçmişi verilerini görüntülemek için view'e gönderir.
            return View(purchases);
        }

        // PDF İndirme İşlemi
        // Kullanıcının belirttiği satın alma için faturayı PDF olarak indirir.
        public async Task<IActionResult> DownloadInvoice(int id)
        {
            // Fatura indirme komutunu oluşturur.
            var command = new DownloadInvoiceCommand
            {
                PurchaseId = id
            };

            // Komutu gönder ve PDF verilerini al.
            var pdfBytes = await _mediator.Send(command);

            if (pdfBytes == null)
            {
                return NotFound();
            }

            // PDF verilerini dosya olarak döndürür.
            return File(pdfBytes, "application/pdf", "TıklaYeFatura.pdf");
        }

        // Kullanıcı ID'sini session'dan alır.
        private int? GetUserIdFromSession()
        {
            return HttpContext.Session.GetInt32("UserId");
        }
    }
}