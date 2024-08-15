using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Queries;

namespace TiklaYe.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Ödeme sayfasını görüntüleme.
        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            var userId = GetUserId();  // Kullanıcı kimliğini al.

            if (!userId.HasValue)
            {
                // Kullanıcı kimliği alınamıyorsa giriş sayfasına yönlendir.
                return RedirectToAction("Login", "Account");
            }

            var query = new GetPaymentQuery { UserId = userId.Value };
            var viewModel = await _mediator.Send(query);

            ViewBag.TotalAmount = viewModel.TotalAmount.ToString("C");
            return View(viewModel.CartItems);
        }

        // Siparişi tamamlama işlemi.
        [HttpPost]
        public async Task<IActionResult> CompleteOrder(string CardName, string CardNumber, string ExpiryDate, string CVV, bool SaveCard, string DeliveryAddress)
        {
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
                var result = await _mediator.Send(command);
                if (result)
                {
                    return RedirectToAction("OrderConfirmation");
                }

                ModelState.AddModelError(string.Empty, "Sipariş tamamlanırken bir hata oluştu.");
            }
            catch
            {
                ModelState.AddModelError("", "Sipariş tamamlanırken bir hata oluştu.");
            }

            var query = new GetPaymentQuery { UserId = userId.Value };
            var viewModel = await _mediator.Send(query);
            ViewBag.TotalAmount = viewModel.TotalAmount.ToString("C");
            return View("Payment", viewModel.CartItems);
        }

        // Kullanıcının kimliğini alır.
        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            return null;
        }

        // Sipariş onay sayfasını görüntüleme.
        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}