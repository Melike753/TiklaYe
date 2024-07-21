using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TiklaYe.Data;
using TiklaYe.Models;

namespace TiklaYe.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SalesReport(DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Purchases
                .Include(p => p.User)
                .Where(p => (!startDate.HasValue || p.PurchaseDate >= startDate.Value) && (!endDate.HasValue || p.PurchaseDate <= endDate.Value))
                .GroupBy(p => new { p.UserId, p.User.Name, p.User.Email })
                .Select(g => new SalesReportViewModel
                {
                    UserId = g.Key.UserId,
                    Name = g.Key.Name,
                    Email = g.Key.Email,
                    TotalOrders = g.Count(),
                    TotalAmount = g.Sum(p => p.TotalPrice)
                });

            var salesReport = await query.ToListAsync();

            ViewBag.TotalRevenue = salesReport.Sum(sr => sr.TotalAmount);

            return View(salesReport);
        }
    }
}