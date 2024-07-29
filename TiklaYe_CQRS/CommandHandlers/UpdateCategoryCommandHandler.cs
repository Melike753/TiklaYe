using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class UpdateCategoryCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public UpdateCategoryCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(UpdateCategoryCommand command)
        {
            var existingCategory = await _context.Categories.FindAsync(command.CategoryId);
            if (existingCategory == null)
            {
                throw new Exception("Kategori bulunamadı.");
            }

            if (command.ImageUrlFile != null && command.ImageUrlFile.Length > 0)
            {
                var fileName = Path.GetFileName(command.ImageUrlFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await command.ImageUrlFile.CopyToAsync(stream);
                }

                existingCategory.ImageUrl = "/images/" + fileName;
            }

            existingCategory.Name = command.Name;
            existingCategory.IsActive = command.IsActive;

            _context.Update(existingCategory);
            await _context.SaveChangesAsync();
        }
    }
}