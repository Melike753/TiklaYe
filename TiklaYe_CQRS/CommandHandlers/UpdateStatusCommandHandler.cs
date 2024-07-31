using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Bir siparişin durumunu güncellemek için kullanılır.
    public class UpdateStatusCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public UpdateStatusCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateStatusCommand command)
        {
            var purchases = await _context.Purchases
                .Where(p => p.OrderNumber == command.OrderNumber)
                .ToListAsync(); // Veritabanında OrderNumber ile eşleşen tüm satın alma kayıtlarını bulur.

            if (purchases == null || !purchases.Any())
            {
                return false;
            }

            // Bulunan her satın alma kaydının durumunu günceller.
            foreach (var purchase in purchases)
            {
                purchase.Status = command.Status;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}