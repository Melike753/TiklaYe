using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Services;
using TiklaYe_CQRS.Data;
using Microsoft.EntityFrameworkCore;

namespace TiklaYe_CQRS.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductCreateCommandHandler _createCommandHandler;
        private readonly ProductUpdateCommandHandler _updateCommandHandler;
        private readonly ProductDeleteCommandHandler _deleteCommandHandler;
        private readonly ProductQueryService _queryService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            ProductCreateCommandHandler createCommandHandler,
            ProductUpdateCommandHandler updateCommandHandler,
            ProductDeleteCommandHandler deleteCommandHandler,
            ProductQueryService queryService,
            ApplicationDbContext context,
            ILogger<ProductController> logger)
        {
            _createCommandHandler = createCommandHandler;
            _updateCommandHandler = updateCommandHandler;
            _deleteCommandHandler = deleteCommandHandler;
            _queryService = queryService;
            _context = context;
            _logger = logger;
        }

        // Ürünleri listeleme
        public async Task<IActionResult> Index()
        {
            var products = await _queryService.GetAllProductsAsync();
            return View(products);
        }

        // Ürün ekleme formu
        public async Task<IActionResult> Create()
        {
            var model = new Product();

            // Aktif kategorileri dropdown menüsüne ekle
            ViewData["CategoryId"] = await GetActiveCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = await GetActiveCategoriesAsync();
                return View(product);
            }

            try
            {
                var command = new ProductCreateCommand
                {
                    Product = product,
                    ImageFile = imageFile
                };
                await _createCommandHandler.ExecuteAsync(command);
                return RedirectToAction(nameof(Index)); // Başarıyla eklenen ürünü listele
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                ModelState.AddModelError("", "Ürün eklenirken bir hata oluştu.");
                ViewData["CategoryId"] = await GetActiveCategoriesAsync();
                return View(product);
            }
        }

        // Ürün bilgileri güncelleme 
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _queryService.GetProductByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            // Aktif kategorileri dropdown menüsüne ekle
            ViewData["CategoryId"] = await GetActiveCategoriesAsync();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile imageFile)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["CategoryId"] = await GetActiveCategoriesAsync();
                return View(product);
            }

            try
            {
                var command = new ProductUpdateCommand
                {
                    Id = id,
                    Product = product,
                    ImageFile = imageFile
                };
                await _updateCommandHandler.ExecuteAsync(command);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                ModelState.AddModelError("", "Ürün bilgileri güncellenirken bir hata oluştu.");
                ViewData["CategoryId"] = await GetActiveCategoriesAsync();
                return View(product);
            }
        }

        // Ürün silme işlemi
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var command = new ProductDeleteCommand { Id = id };
                await _deleteCommandHandler.ExecuteAsync(command);
                return RedirectToAction(nameof(Index)); // Başarıyla silinen ürünü listele
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                ModelState.AddModelError("", "Ürün silinirken bir hata oluştu.");
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task<IEnumerable<SelectListItem>> GetActiveCategoriesAsync()
        {
            return new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "CategoryId", "Name");
        }
    }
}