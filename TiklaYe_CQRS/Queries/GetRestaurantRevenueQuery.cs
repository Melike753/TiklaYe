using MediatR;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Queries
{
    public class GetRestaurantRevenueQuery : IRequest<IEnumerable<RestaurantRevenueViewModel>>
    {
        public int BusinessOwnerId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}