using MediatR;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetRestaurantRevenueQueryHandler : IRequestHandler<GetRestaurantRevenueQuery, IEnumerable<RestaurantRevenueViewModel>>
    {
        private readonly ApplicationDbContext _context;

        // Constructor: Dependency Injection ile ApplicationDbContext'i alır
        public GetRestaurantRevenueQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RestaurantRevenueViewModel>> Handle(GetRestaurantRevenueQuery query, CancellationToken cancellationToken)
        {
            var salesReportQuery = from p in _context.Purchases
                                   where p.BusinessOwnerId == query.BusinessOwnerId  // Belirli bir işletme sahibine ait alımlar ve isteğe bağlı tarih filtreleri ayarlandı.
                                         && (!query.StartDate.HasValue || p.PurchaseDate >= query.StartDate.Value)
                                         && (!query.EndDate.HasValue || p.PurchaseDate <= query.EndDate.Value)
                                   group p by new { p.UserId, p.User.Email, p.User.Name } into g // Kullanıcıya göre gruplama
                                   select new RestaurantRevenueViewModel
                                   {
                                       UserId = g.Key.UserId,
                                       Name = g.Key.Name,
                                       Email = g.Key.Email,
                                       TotalOrders = g.Count(), // Toplam sipariş sayısı (Aldığı ürün sayısı)
                                       TotalAmount = g.Sum(x => x.TotalPrice),  // Toplam tutar
                                       Sales = (from purchase in g
                                                group purchase by new { purchase.UserId } into ug
                                                select new BusinessOwnerSalesReportViewModel
                                                {
                                                    TotalAmount = ug.Sum(x => x.TotalPrice),
                                                    PurchasedItems = ug.Select(item => new PurchaseItemViewModel
                                                    {
                                                        ProductName = item.ProductName,
                                                        UnitPrice = item.UnitPrice,
                                                        Quantity = item.Quantity,
                                                        TotalPrice = item.TotalPrice
                                                    }).ToList() // Satın alınan ürünlerin listesi
                                                }).ToList()
                                   };

            var salesReport = await salesReportQuery.ToListAsync(cancellationToken);
            return salesReport;
        }
    }
}