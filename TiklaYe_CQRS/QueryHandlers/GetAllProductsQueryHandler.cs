using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetAllProductsQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public GetAllProductsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PartnerProduct>> Handle(GetAllProductsQuery query)
        {
            return await _context.PartnerProducts
                .Include(p => p.Category)
                .Where(p => p.BusinessOwnerId == query.BusinessOwnerId)
                .ToListAsync();
        }
    }
}