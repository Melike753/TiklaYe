using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.CommandHandlers
{
    public class DeleteCategoryCommandHandler
    {
        private readonly ApplicationDbContext _context;

        public DeleteCategoryCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteCategoryCommand command)
        {
            var category = await _context.Categories.FindAsync(command.CategoryId);
            if (category != null)
            {
                // Kategori aktiflik durumunu "Pasif" olarak ayarla
                category.IsActive = false;
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}