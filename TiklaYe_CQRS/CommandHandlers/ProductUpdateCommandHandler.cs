using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class ProductUpdateCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public ProductUpdateCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(ProductUpdateCommand command)
        {
            var existingProduct = await _context.Products.FindAsync(command.Id);
            if (existingProduct == null)
            {
                throw new Exception("Product not found.");
            }

            if (command.ImageFile != null && command.ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(command.ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await command.ImageFile.CopyToAsync(stream);
                }

                existingProduct.ImageUrl = "/images/" + fileName; // Görsel yolunu güncelle
            }

            existingProduct.Name = command.Product.Name;
            existingProduct.Description = command.Product.Description;
            existingProduct.Price = command.Product.Price;
            existingProduct.Quantity = command.Product.Quantity;
            existingProduct.IsActive = command.Product.IsActive;
            existingProduct.CategoryId = command.Product.CategoryId;

            _context.Update(existingProduct);
            await _context.SaveChangesAsync();
        }
    }
}