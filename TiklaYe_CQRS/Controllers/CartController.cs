using Microsoft.AspNetCore.Mvc;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;
using TiklaYe_CQRS.Services;

namespace TiklaYe_CQRS.Controllers
{
    public class CartController : Controller
    {
        private readonly AddToCartCommandHandler _addToCartHandler;
        private readonly RemoveFromCartCommandHandler _removeFromCartHandler;
        private readonly ClearCartCommandHandler _clearCartHandler;
        private readonly ICartQueryService _cartService;
        private readonly ApplicationDbContext _context;

        public CartController(
            AddToCartCommandHandler addToCartHandler,
            RemoveFromCartCommandHandler removeFromCartHandler,
            ClearCartCommandHandler clearCartHandler,
            ICartQueryService cartService,
            ApplicationDbContext context)
        {
            _addToCartHandler = addToCartHandler;
            _removeFromCartHandler = removeFromCartHandler;
            _clearCartHandler = clearCartHandler;
            _cartService = cartService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(u => u.Username == userName);

                if (user == null)
                {
                    ModelState.AddModelError("", "Sipariş vermek için lütfen giriş yapınız.");
                    return View();
                }

                var cartItems = await _cartService.GetCartItems();
                return View(cartItems);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var command = new AddToCartCommand
            {
                ProductId = product.ProductId,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Quantity = 1
            };

            try
            {
                await _addToCartHandler.Handle(command);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var command = new RemoveFromCartCommand { ProductId = productId };
            try
            {
                await _removeFromCartHandler.Handle(command);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ClearCartAndRedirect()
        {
            var command = new ClearCartCommand();
            await _clearCartHandler.Handle(command);
            return RedirectToAction("Index", "Home");
        }
    }
}