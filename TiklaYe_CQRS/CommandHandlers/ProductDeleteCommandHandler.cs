using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Adminin ürünü pasif hale getirmesini sağlar.
    public class ProductDeleteCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public ProductDeleteCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(ProductDeleteCommand command)
        {
            var product = await _context.Products.FindAsync(command.Id);
            if (product == null)
            {
                throw new Exception("Product not found.");
            }

            // Ürünün aktif olmadığını belirtir 
            product.IsActive = false;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}