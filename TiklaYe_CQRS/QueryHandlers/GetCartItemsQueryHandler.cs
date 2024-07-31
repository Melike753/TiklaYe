using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;

namespace TiklaYe_CQRS.QueryHandlers
{
    public class GetCartItemsQueryHandler
    {
        private readonly ICartService _cartService;

        public GetCartItemsQueryHandler(ICartService cartService)
        {
            _cartService = cartService;
        }

        public IEnumerable<CartItem> Handle(GetCartItemsQuery query)
        {
            return _cartService.GetCartItems();
        }
    }
}