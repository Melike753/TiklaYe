using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class ProductCreateCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public ProductCreateCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(ProductCreateCommand command)
        {
            if (command.ImageFile != null && command.ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(command.ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await command.ImageFile.CopyToAsync(stream);
                }

                command.Product.ImageUrl = "/images/" + fileName; // Görsel yolunu kaydet
            }

            _context.Add(command.Product);
            await _context.SaveChangesAsync();
        }
    }
}