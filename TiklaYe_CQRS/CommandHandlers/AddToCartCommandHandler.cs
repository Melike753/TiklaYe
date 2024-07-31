using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Sepete ürün eklemek için kullanılır.
    public class AddToCartCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public AddToCartCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(AddToCartCommand command)
        {
            // Kullanıcının sepetini bul
            var userCart = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == command.ProductId);

            if (userCart != null)
            {
                // Ürün zaten sepette var, miktarı artır
                userCart.Quantity += command.Quantity;
            }
            else
            {
                // Ürün sepette yok, yeni ürün ekle
                var newCartItem = new CartItem
                {
                    ProductId = command.ProductId,
                    Name = command.Name,
                    ImageUrl = command.ImageUrl,
                    Price = command.Price,
                    Quantity = command.Quantity
                };
                await _context.CartItems.AddAsync(newCartItem);
            }
            await _context.SaveChangesAsync();
        }
    }
}