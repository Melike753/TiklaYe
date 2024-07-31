using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Services
{
    public interface ICartQueryService
    {
        Task<IEnumerable<CartItem>> GetCartItems();
    }

    public class CartQueryService : ICartQueryService
    {
        private readonly ApplicationDbContext _context;

        public CartQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetCartItems()
        {
            return await _context.CartItems.ToListAsync();
        }
    }
}