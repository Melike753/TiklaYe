using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TiklaYe.Models;
using System.Linq;
using TiklaYe.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace TiklaYe.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ApplicationDbContext _context;

        public OrderController(ICartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        [HttpGet]
        // Sepet öğelerini alır ve toplam tutarı hesaplar.
        public IActionResult Payment()
        {
            var cartItems = _cartService.GetCartItems();
            var totalAmount = cartItems.Sum(item => item.TotalPrice);
            ViewBag.TotalAmount = totalAmount.ToString("C");
            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> CompleteOrder(string CardName, string CardNumber, string ExpiryDate, string CVV, bool SaveCard, string DeliveryAddress)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var cartItems = _cartService.GetCartItems();
            foreach (var item in cartItems)
            {
                var purchase = new Purchase
                {
                    UserId = user.UserId,
                    PurchaseDate = DateTime.Now,
                    OrderNumber = Guid.NewGuid().ToString(),
                    ProductName = item.Name,
                    UnitPrice = item.Price,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice,
                    Status = "Hazırlanıyor"
                };

                _context.Purchases.Add(purchase);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("OrderConfirmation");
        }

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            return null;
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}