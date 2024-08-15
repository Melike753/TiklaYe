using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Sepetindeki tüm ürünleri temizlemek için
    public class ClearCartCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public ClearCartCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        // Alışveriş sepetindeki tüm öğeleri temizler.
        public async Task Handle(ClearCartCommand command)
        {
            var cartItems = _context.CartItems
           .Where(ci => ci.UserId == command.UserId)
           .ToList();
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}