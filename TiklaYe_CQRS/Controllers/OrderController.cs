using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartService _cartService;
        private readonly CompleteOrderCommandHandler _completeOrderHandler;

        public OrderController(ICartService cartService, CompleteOrderCommandHandler completeOrderHandler)
        {
            _cartService = cartService;
            _completeOrderHandler = completeOrderHandler;
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
                return RedirectToAction("Login", "Account");
            }

            var userId = GetUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var command = new CompleteOrderCommand
            {
                CardName = CardName,
                CardNumber = CardNumber,
                ExpiryDate = ExpiryDate,
                CVV = CVV,
                SaveCard = SaveCard,
                DeliveryAddress = DeliveryAddress,
                UserId = userId.Value
            };

            try
            {
                await _completeOrderHandler.Handle(command);
                return RedirectToAction("OrderConfirmation");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Payment", _cartService.GetCartItems());
            }
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