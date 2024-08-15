using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Queries;
using TiklaYe_CQRS.QueryHandlers;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.Controllers
{
    public class PartnerProductController : Controller
    {
        private readonly GetAllProductsQueryHandler _getAllProductsHandler;
        private readonly GetProductByIdQueryHandler _getProductByIdHandler;
        private readonly AddProductCommandHandler _addProductHandler;
        private readonly EditProductCommandHandler _editProductHandler;
        private readonly DeleteProductCommandHandler _deleteProductHandler;
        private readonly ApplicationDbContext _context;

        public PartnerProductController(
            GetAllProductsQueryHandler getAllProductsHandler,
            GetProductByIdQueryHandler getProductByIdHandler,
            AddProductCommandHandler addProductHandler,
            EditProductCommandHandler editProductHandler,
            DeleteProductCommandHandler deleteProductHandler,
            ApplicationDbContext context
            )
        {
            _getAllProductsHandler = getAllProductsHandler;
            _getProductByIdHandler = getProductByIdHandler;
            _addProductHandler = addProductHandler;
            _editProductHandler = editProductHandler;
            _deleteProductHandler = deleteProductHandler;
            _context = context;
        }

        // İşletme sahibi için tüm ürünleri listeler.
        public async Task<IActionResult> Index()
        {
            var businessOwnerId = await GetBusinessOwnerIdAsync();

            var query = new GetAllProductsQuery { BusinessOwnerId = businessOwnerId.Value };
            var products = await _getAllProductsHandler.Handle(query);

            return View(products);
        }

        // Ürün ekleme formunu gösterir.
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var businessOwnerId = await GetBusinessOwnerIdAsync();

            var product = new PartnerProduct
            {
                IsActive = true,
                BusinessOwnerId = businessOwnerId.Value
            };

            ViewBag.CategoryId = await GetActiveCategoriesAsync(product.CategoryId);
            return View(product);
        }

        // Yeni ürünü ekler.
        [HttpPost]
        public async Task<IActionResult> AddProduct(PartnerProduct model, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = await GetActiveCategoriesAsync();
                return View(model);
            }

            var businessOwnerId = await GetBusinessOwnerIdAsync();

            model.ImageUrl = await SaveImageAsync(imageFile);

            var command = new AddProductCommand
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Quantity = model.Quantity,
                CategoryId = model.CategoryId.Value,
                BusinessOwnerId = businessOwnerId.Value,
                ImageUrl = model.ImageUrl
            };

            await _addProductHandler.Handle(command);
            return RedirectToAction("Index");
        }

        // Ürün bilgilerini güncelleme formunu gösterir.
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var query = new GetProductByIdQuery { ProductId = id };
            var product = await _getProductByIdHandler.Handle(query);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = await GetActiveCategoriesAsync(product.CategoryId);
            return View(product);
        }

        // Ürün bilgilerini günceller.
        [HttpPost]
        public async Task<IActionResult> EditProduct(PartnerProduct model, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = await GetActiveCategoriesAsync();
                return View(model);
            }

            model.ImageUrl = await SaveImageAsync(imageFile);

            var command = new EditProductCommand
            {
                ProductId = model.ProductId,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Quantity = model.Quantity,
                CategoryId = model.CategoryId.Value,
                ImageUrl = model.ImageUrl,
                IsActive = model.IsActive
            };

            await _editProductHandler.Handle(command);
            return RedirectToAction(nameof(Index));
        }

        // Ürünü siler.
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand { ProductId = id };
            await _deleteProductHandler.Handle(command);
            return RedirectToAction("Index");
        }

        // DbContext'i serbest bırakır.
        protected override void Dispose(bool disposing)
        {
            if (disposing && _context != null)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task<SelectList> GetActiveCategoriesAsync(int? selectedCategoryId = null)
        {
            var categories = await _context.Categories.Where(c => c.IsActive).ToListAsync();
            return new SelectList(categories, "CategoryId", "Name", selectedCategoryId);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return "/images/" + fileName;
            }
            else
            {
                return null;
            }
        }

        private async Task<int?> GetBusinessOwnerIdAsync()
        {
            var businessOwnerId = HttpContext.Session.GetInt32("BusinessOwnerId");
            if (businessOwnerId == null)
            {
                RedirectToAction("Login", "Business");
            }
            return businessOwnerId;
        }
    }
}