using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class CompleteOrderCommandHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;

        public CompleteOrderCommandHandler(ApplicationDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task Handle(CompleteOrderCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == command.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var cartItems = _cartService.GetCartItems(); // Alışveriş sepetindeki tüm ürünleri alır.
            var orderNumber = Guid.NewGuid().ToString(); // Tek sipariş numarası oluşturur.

            foreach (var item in cartItems)
            {
                var purchase = new Purchase
                {
                    UserId = user.UserId,
                    PurchaseDate = DateTime.Now,
                    OrderNumber = orderNumber,
                    ProductName = item.Name,
                    UnitPrice = item.Price,
                    Quantity = item.Quantity,
                    TotalPrice = item.Price * item.Quantity, // Toplam fiyatı hesapla
                    Status = "Hazırlanıyor"
                };

                _context.Purchases.Add(purchase);
            }
            await _context.SaveChangesAsync();
        }
    }
}