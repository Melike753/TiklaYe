using MediatR;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;
using TiklaYe_CQRS.Services;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, PaymentViewModel>
    {
        private readonly ICartService _cartService;

        public GetPaymentQueryHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public Task<PaymentViewModel> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var cartItems = _cartService.GetCartItems(request.UserId);
            var totalAmount = cartItems.Sum(item => item.TotalPrice);
            return Task.FromResult(new PaymentViewModel
            {
                CartItems = cartItems,
                TotalAmount = totalAmount
            });
        }
    }
}