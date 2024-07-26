using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiklaYe.Data;
using TiklaYe.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TiklaYe.Controllers
{
    public class PartnerProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PartnerProductController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            int? businessOwnerId = HttpContext.Session.GetInt32("BusinessOwnerId");
            if (businessOwnerId == null)
            {
                return RedirectToAction("Login", "Business");
            }

            var products = await _context.PartnerProducts
                .Include(p => p.Category)
                .Where(p => p.BusinessOwnerId == businessOwnerId)
                .ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            int? businessOwnerId = HttpContext.Session.GetInt32("BusinessOwnerId");
            if (businessOwnerId == null)
            {
                return RedirectToAction("Login", "Business");
            }

            ViewBag.CategoryId = new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "CategoryId", "Name");
            var product = new PartnerProduct
            {
                IsActive = true,
                BusinessOwnerId = businessOwnerId.Value
            };
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(PartnerProduct model, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
                return View(model);
            }

            var businessOwnerId = HttpContext.Session.GetInt32("BusinessOwnerId");
            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                model.ImageUrl = "/images/" + fileName; // Görsel yolunu kaydet
            }

            if (businessOwnerId == null)
            {
                return RedirectToAction("Login", "Business");
            }

            model.BusinessOwnerId = businessOwnerId.Value;

            _context.PartnerProducts.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.PartnerProducts.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "CategoryId", "Name", product.CategoryId);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(PartnerProduct model, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(await _context.Categories.Where(c => c.IsActive).ToListAsync(), "CategoryId", "Name", model.CategoryId);
                return View(model);
            }

            var existingProduct = await _context.PartnerProducts.FindAsync(model.ProductId);
            if (existingProduct == null)
            {
                return NotFound();
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var fileName = Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                existingProduct.ImageUrl = "/images/" + fileName; // Görsel yolunu güncelle
            }

            existingProduct.Name = model.Name;
            existingProduct.Description = model.Description;
            existingProduct.Price = model.Price;
            existingProduct.Quantity = model.Quantity;
            existingProduct.CategoryId = model.CategoryId;
            existingProduct.IsActive = model.IsActive;

            try
            {
                _context.PartnerProducts.Update(existingProduct);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(model.ProductId))
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

        private bool ProductExists(int id)
        {
            return _context.PartnerProducts.Any(e => e.ProductId == id);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.PartnerProducts.FindAsync(id);
            if (product != null)
            {
                _context.PartnerProducts.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int? businessOwnerId = HttpContext.Session.GetInt32("BusinessOwnerId");
            if (businessOwnerId == null)
            {
                return RedirectToAction("Login", "Business");
            }

            var product = await _context.PartnerProducts.FindAsync(id);
            if (product != null && product.BusinessOwnerId == businessOwnerId)
            {
                try
                {
                    product.IsActive = false;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    ModelState.AddModelError("", "An error occurred while deleting the product.");
                    return View(product);
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