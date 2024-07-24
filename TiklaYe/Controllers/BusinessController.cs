using Microsoft.AspNetCore.Mvc;
using TiklaYe.Data;
using TiklaYe.Models;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace TiklaYe.Controllers
{
    public class BusinessController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BusinessController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(BusinessOwner model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı adı veya e-posta zaten var mı kontrol et
                if (await UserExists(model.Email))
                {
                    ModelState.AddModelError("", "E-posta zaten kullanılıyor.");
                    return View(model);
                }

                // Parolayı veritabanına kaydetmeden önce hashle
                var businessOwner = new BusinessOwner
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Password = HashPassword(model.Password),
                };

                _context.BusinessOwners.Add(businessOwner);
                await _context.SaveChangesAsync();
                await SignInUser(businessOwner.Email);
                return RedirectToAction("Success");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(BusinessLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var businessOwner = await _context.BusinessOwners
                    .FirstOrDefaultAsync(b => b.Email == model.Email);

                if (businessOwner != null && VerifyPasswordHash(model.Password, businessOwner.Password))
                {
                    // Giriş işlemi başarılı
                    await SignInUser(businessOwner.Email);

                    if (businessOwner.Email == "admintiklaye@gmail.com")
                    {
                        // Admin sayfasına yönlendir
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        // Normal işletmeciyi işletmeci ana sayfasına yönlendir
                        return RedirectToAction("Dashboard", "Business");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz giriş bilgileri.");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
            }

            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        // Veritabanında belirtilen e-posta adresine sahip bir kullanıcının olup olmadığını kontrol eder.
        private async Task<bool> UserExists(string email)
        {
            return await _context.BusinessOwners.AnyAsync(u => u.Email == email);
        }

        // Kullanıcı şifresini SHA256 algoritması ile hashler.
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                // Şifrenin hash'ini hesapla
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Byte array'i string'e dönüştür
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        // Kullanıcının girdiği şifreyi hashler ve veritabanındaki hash ile karşılaştırır.
        private bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString() == storedHash;
            }
        }

        private async Task SignInUser(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                // İsteğe bağlı olarak buraya ek özellikler eklenebilir.
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}
