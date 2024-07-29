using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class AdminGetProductByIdQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public AdminGetProductByIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(AdminGetProductByIdQuery query)
        {
            return await _context.Products.FindAsync(query.ProductId);
        }
    }
}