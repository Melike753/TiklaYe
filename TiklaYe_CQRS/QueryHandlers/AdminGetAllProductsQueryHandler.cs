using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class AdminGetAllProductsQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public AdminGetAllProductsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(AdminGetAllProductsQuery query)
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }
    }
}