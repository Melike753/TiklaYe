using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetSalesReportQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public GetSalesReportQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<SalesReportViewModel>> Handle(GetSalesReportQuery query)
        {
            var salesReportQuery = _context.Purchases
                .Include(p => p.User)
                .Where(p => (!query.StartDate.HasValue || p.PurchaseDate >= query.StartDate.Value) && (!query.EndDate.HasValue || p.PurchaseDate <= query.EndDate.Value))
                .GroupBy(p => new { p.UserId, p.User.Name, p.User.Email })
                .Select(g => new SalesReportViewModel
                {
                    UserId = g.Key.UserId,
                    Name = g.Key.Name,
                    Email = g.Key.Email,
                    TotalOrders = g.Count(),
                    TotalAmount = g.Sum(p => p.TotalPrice)
                });

            return await salesReportQuery.ToListAsync();
        }
    }
}