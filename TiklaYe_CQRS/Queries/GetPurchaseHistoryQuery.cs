using MediatR;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Queries
{
    public class GetPurchaseHistoryQuery : IRequest<List<Purchase>>
    {
        public int UserId { get; set; }

        public GetPurchaseHistoryQuery(int userId)
        {
            UserId = userId;
        }
    }
}