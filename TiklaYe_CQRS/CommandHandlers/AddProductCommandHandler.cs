using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.CommandHandlers
{
    // Ürün ekleme komutunu işlemek için kullanılır.
    public class AddProductCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public AddProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(AddProductCommand command)
        {
            var product = new PartnerProduct
            {
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                Quantity = command.Quantity,
                CategoryId = command.CategoryId,
                BusinessOwnerId = command.BusinessOwnerId,
                ImageUrl = command.ImageUrl,
                IsActive = true
            };

            _context.PartnerProducts.Add(product);
            await _context.SaveChangesAsync(); // Veritabanı değişikliklerini kaydetmek için
        }
    }
}