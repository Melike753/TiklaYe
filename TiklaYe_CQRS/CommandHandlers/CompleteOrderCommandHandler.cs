using MediatR;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Services;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommand, bool>
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;

        public CompleteOrderCommandHandler(ApplicationDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<bool> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            var cartItems = _cartService.GetCartItems(request.UserId);
            var orderNumber = Guid.NewGuid().ToString();

            foreach (var item in cartItems)
            {
                var purchase = new Purchase
                {
                    UserId = request.UserId,
                    PurchaseDate = DateTime.Now,
                    OrderNumber = orderNumber,
                    ProductName = item.Name,
                    UnitPrice = item.Price,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice,
                    Status = "Hazırlanıyor"
                };

                _context.Purchases.Add(purchase);
            }

            await _context.SaveChangesAsync();
            _cartService.ClearCart(request.UserId);
            return true;
        }
    }
}