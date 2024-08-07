﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiklaYe.Data;

namespace TiklaYe.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Sadece aktif ürünleri getir
            var products = await _context.Products.Where(p => p.IsActive).ToListAsync();
            return View(products);
        }
    }
}
