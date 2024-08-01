using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Services
{
    public interface ICartQueryService
    {
        Task<IEnumerable<CartItem>> GetCartItems(int userId);
    }
}