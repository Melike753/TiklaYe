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

        // Constructor dependency injection
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

        // Sepet sayfasını gösterir.
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Kullanıcı ID'sini almak için
                var userId = HttpContext.Session.GetInt32("UserId");

                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    ModelState.AddModelError("", "Sipariş vermek için lütfen giriş yapınız.");
                    return View();
                }

                var cartItems = await _cartService.GetCartItems(userId.Value);
                return View(cartItems);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        // Sepete ürün ekleme işlemini gerçekleştirir.
        public async Task<IActionResult> AddToCart(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var product = await _context.PartnerProducts.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var command = new AddToCartCommand
            {
                UserId = userId.Value,
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
        // Sepetten ürün çıkarma işlemini gerçekleştirir.
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var command = new RemoveFromCartCommand
            {
                UserId = userId.Value,
                ProductId = productId
            };

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
        // Sepeti temizleme ve ana sayfaya yönlendirme işlemini gerçekleştirir.
        public async Task<IActionResult> ClearCartAndRedirect()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var command = new ClearCartCommand { UserId = userId.Value };
            await _clearCartHandler.Handle(command);
            return RedirectToAction("Index", "Home");
        }
    }
}