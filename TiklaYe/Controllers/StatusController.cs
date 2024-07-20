using Microsoft.AspNetCore.Mvc;
using TiklaYe.Data;
using TiklaYe.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TiklaYe.Controllers
{
    public class StatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatusController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Tüm siparişleri en yeni tarihe göre sırala
            var purchases = await _context.Purchases
                .OrderByDescending(p => p.PurchaseDate) // Siparişleri en yeni tarihe göre sırala
                .ToListAsync();

            // Siparişleri sipariş numarasına göre grupla
            var groupedPurchases = purchases
                .GroupBy(p => p.OrderNumber)
                .Select(g => new PurchaseGroupViewModel
                {
                    OrderNumber = g.Key,
                    PurchaseDate = g.FirstOrDefault().PurchaseDate,
                    Status = g.FirstOrDefault().Status,
                    Items = g.ToList()
                })
                .ToList();

            return View(groupedPurchases);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string orderNumber, string status)
        {
            var purchases = await _context.Purchases
                .Where(p => p.OrderNumber == orderNumber)
                .ToListAsync();

            if (purchases == null || !purchases.Any())
            {
                return NotFound();
            }

            foreach (var purchase in purchases)
            {
                purchase.Status = status;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
