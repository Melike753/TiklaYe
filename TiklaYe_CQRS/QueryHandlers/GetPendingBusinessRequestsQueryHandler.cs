using TiklaYe_CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetPendingBusinessRequestsQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public GetPendingBusinessRequestsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<BusinessOwner>> Handle(GetPendingBusinessRequestsQuery query)
        {
            return await _context.BusinessOwners
                .Where(b => !b.IsApproved)
                .ToListAsync();
        }
    }
}