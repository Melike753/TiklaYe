using Microsoft.AspNetCore.Mvc;
using TiklaYe.Models;
using System.Linq;

namespace TiklaYe.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartService _cartService;

        public OrderController(ICartService cartService)
        {
            _cartService = cartService;
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
        // Ödeme işlemlerini burada gerçekleştir
        // Ödeme başarılı olursa kullanıcıyı siparişin tamamlandığı bir sayfaya yönlendiriyoruz.
        public IActionResult CompleteOrder(string CardName, string CardNumber, string ExpiryDate, string CVV, bool SaveCard, string DeliveryAddress)
        {
            return RedirectToAction("OrderConfirmation");
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}