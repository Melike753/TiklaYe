using Microsoft.AspNetCore.Mvc;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Queries;
using TiklaYe_CQRS.QueryHandlers;

namespace TiklaYe_CQRS.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly GetUserPurchasesHandler _getUserPurchasesHandler;
        private readonly DownloadInvoiceHandler _downloadInvoiceHandler;

        public PurchaseController(GetUserPurchasesHandler getUserPurchasesHandler, DownloadInvoiceHandler downloadInvoiceHandler)
        {
            _getUserPurchasesHandler = getUserPurchasesHandler;
            _downloadInvoiceHandler = downloadInvoiceHandler;
        }

        // Satın Alma Geçmişi Sayfası
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }

            var query = new GetUserPurchasesQuery
            {
                UserName = User.Identity.Name
            };

            var purchases = await _getUserPurchasesHandler.Handle(query);
            return View(purchases);
        }

        // PDF İndirme İşlemi
        public async Task<IActionResult> DownloadInvoice(int id)
        {
            var command = new DownloadInvoiceCommand
            {
                PurchaseId = id
            };

            return await _downloadInvoiceHandler.Handle(command);
        }
    }
}