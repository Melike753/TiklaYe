using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Adminin eklediği aktif ürünleri listelemek için kullanılır. 
        //public async Task<IActionResult> Index()
        //{
        //    // Sadece aktif ürünleri getir
        //    var products = await _context.Products.Where(p => p.IsActive).ToListAsync();
        //    return View(products);
        //}

        // Aktif işletmecilerin listesini getirir.
        public async Task<IActionResult> Restaurants()
        {
            var businessOwners = await _context.BusinessOwners
                .Where(bo => bo.IsActive)
                .ToListAsync();

            return View(businessOwners);
        }

        // Belirli bir işletmecinin eklediği ürünlerin listesini getirir.
        public async Task<IActionResult> PartnerProducts(int businessOwnerId)
        {
            var products = await _context.PartnerProducts
                .Where(p => p.BusinessOwnerId == businessOwnerId && p.IsActive) // İşletmeciye ait ve aktif ürünleri getirir.
                .ToListAsync();

            return View(products);
        }
    }
}