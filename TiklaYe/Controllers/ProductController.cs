using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TiklaYe.Data;
using TiklaYe.Models;

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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            var product = new Product
            {
                IsActive = true // Varsayılan olarak aktif olarak ayarla
            };
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (product.ImageUrlFile != null && product.ImageUrlFile.Length > 0)
                {
                    try
                    {
                        var fileName = Path.GetFileName(product.ImageUrlFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageUrlFile.CopyToAsync(stream);
                        }
                        product.ImageUrl = "/images/" + fileName;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Dosya yüklenirken bir hata oluştu: " + ex.Message);
                        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
                        return View(product);
                    }
                }

                try
                {
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "Ürün başarıyla eklendi." });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Veritabanına kaydederken bir hata oluştu: " + ex.Message);
                    return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }


            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", product.CategoryId);
            return View(product);
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

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _context.Products.FindAsync(id);
                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    if (product.ImageUrlFile != null && product.ImageUrlFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(product.ImageUrlFile.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageUrlFile.CopyToAsync(stream);
                        }

                        existingProduct.ImageUrl = "/images/" + fileName; // Görsel yolunu güncelle
                    }

                    existingProduct.Name = product.Name;
                    existingProduct.IsActive = product.IsActive;

                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
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

            // Kategori aktiflik durumunu "Pasif" olarak ayarla
            product.IsActive = false;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsActive = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }


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
