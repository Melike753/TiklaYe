using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TiklaYe.Data;
using TiklaYe.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace TiklaYe.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        public IActionResult Create()
        {
            var product = new Product();
            product.IsActive = true;
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(product);
            }

            try
            {
                if (product.imageFile != null && product.imageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(product.imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.imageFile.CopyToAsync(stream);
                    }

                    product.ImageUrl = "/images/" + fileName; // Görsel yolunu kaydet
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Hatanın ayrıntılarını konsola yazdır
                ModelState.AddModelError("", "An error occurred while saving the product.");
                return View(product); // Hata durumunda kullanıcıya geri dön
            }
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(product);
            }

            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                if (product.imageFile != null && product.imageFile.Length > 0)
                {
                    // Dosyayı sunucuya kaydetmek için gerekli işlemler
                    var fileName = Path.GetFileName(product.imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.imageFile.CopyToAsync(stream);
                    }

                    existingProduct.ImageUrl = "/images/" + fileName; // Görsel yolunu kaydet
                }

                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                existingProduct.IsActive = product.IsActive;


                _context.Update(existingProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Hatanın ayrıntılarını konsola yazdır
                ModelState.AddModelError("", "An error occurred while saving the product.");
                return View(product); // Hata durumunda kullanıcıya geri dön
            }
        }


        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            try
            {
                // Ürün aktiflik durumunu "Pasif" olarak ayarla
                product.IsActive = false;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Hatanın ayrıntılarını konsola yazdır
                ModelState.AddModelError("", "An error occurred while deleting the product.");
                return View(product); // Hata durumunda kullanıcıya geri dön
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                try
                {
                    product.IsActive = false;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message); // Hatanın ayrıntılarını konsola yazdır
                    ModelState.AddModelError("", "An error occurred while deleting the product.");
                    return View(product); // Hata durumunda kullanıcıya geri dön
                }
            }
            return RedirectToAction(nameof(Index));
        }


        // Dispose of the DbContext
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}