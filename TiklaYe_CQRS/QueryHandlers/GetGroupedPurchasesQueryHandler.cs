using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetGroupedPurchasesQueryHandler
    {
        private readonly ApplicationDbContext _context;

        public GetGroupedPurchasesQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseGroupViewModel>> Handle(GetGroupedPurchasesQuery query)
        {
            // Tüm siparişleri en yeni tarihe göre sırala
            var purchases = await _context.Purchases
                .OrderByDescending(p => p.PurchaseDate) // Siparişleri en yeni tarihe göre sırala
                .ToListAsync();

            // Siparişleri sipariş numarasına göre grupla
            var groupedPurchases = purchases
                .GroupBy(p => p.OrderNumber)
                .Select(g => new PurchaseGroupViewModel
                {
                    OrderNumber = g.Key,
                    PurchaseDate = g.FirstOrDefault().PurchaseDate,
                    Status = g.FirstOrDefault().Status,
                    Items = g.ToList()
                })
                .ToList();

            return groupedPurchases;
        }
    }
}