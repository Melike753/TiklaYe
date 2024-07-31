using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Belirli bir ürünü veritabanında güncellemek için kullanılır.
    public class EditProductCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public EditProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(EditProductCommand command)
        {
            var product = await _context.PartnerProducts.FindAsync(command.ProductId);
            if (product == null) throw new Exception("Product not found.");

            product.Name = command.Name;
            product.Description = command.Description;
            product.Price = command.Price;
            product.Quantity = command.Quantity;
            product.CategoryId = command.CategoryId;
            product.ImageUrl = command.ImageUrl;
            product.IsActive = command.IsActive;

            _context.PartnerProducts.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}