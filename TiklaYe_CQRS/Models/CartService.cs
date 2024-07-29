namespace TiklaYe_CQRS.Models
{
    public interface ICartService
    {
        void AddToCart(CartItem item);
        void RemoveFromCart(int productId);
        List<CartItem> GetCartItems();
        void ClearCart();
    }

    public class CartService : ICartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCartSessionKey()
        {
            var userId = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated
                ? _httpContextAccessor.HttpContext.User.Identity.Name
                : "guest"; // Oturum açmamış kullanıcılar için

            return $"{CartSessionKey}_{userId}";
        }

        public void AddToCart(CartItem item)
        {
            var cart = GetCartItems();
            var existingItem = cart.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            SaveCart(cart);
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
            }

            SaveCart(cart);
        }

        public List<CartItem> GetCartItems()
        {
            return _httpContextAccessor.HttpContext.Session.GetObject<List<CartItem>>(GetCartSessionKey()) ?? new List<CartItem>();
        }

        public void ClearCart()
        {
            SaveCart(new List<CartItem>());
        }

        private void SaveCart(List<CartItem> cart)
        {
            _httpContextAccessor.HttpContext.Session.SetObject(GetCartSessionKey(), cart);
        }
    }
}