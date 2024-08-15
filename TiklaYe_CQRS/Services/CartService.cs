using System.Security.Claims;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Models;

namespace TiklaYe_CQRS.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void AddToCart(CartItem item)
        {
            var userId = GetUserId();
            if (userId == null)
                return;

            var existingItem = _context.CartItems.FirstOrDefault(ci => ci.UserId == userId && ci.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                _context.CartItems.Update(existingItem);
            }
            else
            {
                item.UserId = userId.Value;
                _context.CartItems.Add(item);
            }

            _context.SaveChanges();
        }

        public void ClearCart(int userId)
        {
            var cartItems = _context.CartItems.Where(ci => ci.UserId == userId).ToList();
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }

        public List<CartItem> GetCartItems(int userId)
        {
            return _context.CartItems.Where(ci => ci.UserId == userId).ToList();
        }

        public void RemoveFromCart(int productId)
        {
            var userId = GetUserId();
            if (userId == null)
                return;

            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.UserId == userId && ci.ProductId == productId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                _context.SaveChanges();
            }
        }

        private int? GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
            {
                return userId;
            }
            return null;
        }
    }
}