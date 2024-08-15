using MediatR;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Queries
{
    public class GetPaymentQuery : IRequest<PaymentViewModel>
    {
        public int UserId { get; set; }
    }
}