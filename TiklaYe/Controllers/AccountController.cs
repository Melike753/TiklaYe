using Microsoft.AspNetCore.Mvc;
using TiklaYe.Data;
using TiklaYe.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace TiklaYe.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı adı veya e-posta zaten var mı kontrol et
                if (UserExists(model.Username, model.Email))
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya e-posta zaten kullanılıyor.");
                    return View(model);
                }

                // Parolayı veritabanına kaydetmeden önce hashle
                var user = new User
                {
                    Name = model.Name,
                    Username = model.Username,
                    Email = model.Email,
                    Mobile = model.Mobile,
                    Address = model.Address,
                    PostCode = model.PostCode,
                    Password = HashPassword(model.Password),
                    CreatedDate = DateTime.Now
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                // Başarılı kayıt sonrası giriş sayfasına yönlendir
                return RedirectToAction("Login", "Login");
            }

            return View(model);
        }

        private bool UserExists(string username, string email)
        {
            return _context.Users.Any(u => u.Username == username || u.Email == email);
        }

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
    }
}