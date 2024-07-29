using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class UpdateStatusHandler
    {
        private readonly ApplicationDbContext _context;

        public UpdateStatusHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateStatusCommand command)
        {
            var purchases = await _context.Purchases
                .Where(p => p.OrderNumber == command.OrderNumber)
                .ToListAsync();

            if (purchases == null || !purchases.Any())
            {
                return false;
            }

            foreach (var purchase in purchases)
            {
                purchase.Status = command.Status;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}