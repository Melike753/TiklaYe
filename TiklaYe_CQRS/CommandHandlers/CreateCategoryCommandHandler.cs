using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class CreateCategoryCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public CreateCategoryCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateCategoryCommand command)
        {
            var category = new Category
            {
                Name = command.Name,
                IsActive = command.IsActive
            };

            if (command.ImageUrlFile != null && command.ImageUrlFile.Length > 0)
            {
                // Dosyayı sunucuya kaydetmek için gerekli işlemler
                var fileName = Path.GetFileName(command.ImageUrlFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await command.ImageUrlFile.CopyToAsync(stream);
                }

                category.ImageUrl = "/images/" + fileName;
            }

            _context.Add(category);
            await _context.SaveChangesAsync();
        }
    }
}