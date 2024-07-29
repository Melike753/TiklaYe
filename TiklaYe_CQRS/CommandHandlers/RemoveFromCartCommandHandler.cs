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
                .FirstOrDefaultAsync(ci => ci.ProductId == command.ProductId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}