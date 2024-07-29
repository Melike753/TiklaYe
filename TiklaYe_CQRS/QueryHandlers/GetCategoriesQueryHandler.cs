using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetCategoriesQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public GetCategoriesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> Handle(GetCategoriesQuery query)
        {
            return await _context.Categories.ToListAsync();
        }
    }
}