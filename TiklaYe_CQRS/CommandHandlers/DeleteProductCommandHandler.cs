using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Bir ürünü veritabanından silmek için kullanılır.
    public class DeleteProductCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public DeleteProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteProductCommand command)
        {
            var product = await _context.PartnerProducts.FindAsync(command.ProductId);
            if (product != null)
            {
                // Ürün aktiflik durumunu "Pasif" olarak ayarla
                product.IsActive = false;
                _context.PartnerProducts.Update(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}