using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class RemoveFromCartCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public RemoveFromCartCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(RemoveFromCartCommand command)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == command.UserId && ci.ProductId == command.ProductId);

            if (cartItem != null)
            {
                var businessOwnerId = cartItem.BusinessOwnerId;

                if (cartItem.Quantity > 1)
                {
                    // Eğer miktar 1'den fazlaysa, miktarı 1 azalt
                    cartItem.Quantity -= 1;
                }
                else
                {
                    // Eğer miktar 1 ise, ürünü tamamen kaldır
                    _context.CartItems.Remove(cartItem);
                }

                // Sepette başka ürün var mı kontrol et
                var remainingItems = await _context.CartItems
                    .Where(ci => ci.UserId == command.UserId && ci.BusinessOwnerId == businessOwnerId)
                    .ToListAsync();

                if (!remainingItems.Any())
                {
                    // Eğer sepette başka ürün yoksa, ilgili restoran ID'sini temizle
                    _context.CartItems.RemoveRange(
                        await _context.CartItems
                            .Where(ci => ci.UserId == command.UserId && ci.BusinessOwnerId == businessOwnerId)
                            .ToListAsync()
                    );
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}