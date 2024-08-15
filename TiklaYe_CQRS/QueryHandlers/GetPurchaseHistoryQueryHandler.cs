using MediatR;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetPurchaseHistoryQueryHandler : IRequestHandler<GetPurchaseHistoryQuery, List<Purchase>>
    {
        private readonly ApplicationDbContext _context;

        public GetPurchaseHistoryQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> Handle(GetPurchaseHistoryQuery request, CancellationToken cancellationToken)
        {
            return await _context.Purchases.Where(p => p.UserId == request.UserId).ToListAsync();
        }
    }
}