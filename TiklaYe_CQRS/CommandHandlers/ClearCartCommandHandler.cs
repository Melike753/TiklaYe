using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class ClearCartCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public ClearCartCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(ClearCartCommand command)
        {
            var cartItems = _context.CartItems.ToList();
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }
    }
}