using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Services
{
    public interface ICartService
    {
        void AddToCart(CartItem item);
        void ClearCart(int userId);
        List<CartItem> GetCartItems(int userId);
        void RemoveFromCart(int productId);
    }
}