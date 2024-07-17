using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TiklaYe.Data;
using TiklaYe.Models;

namespace TiklaYe.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel
            {
                Username = user.Username,
                Email = user.Email,
                Mobile = user.Mobile,
                Address = user.Address,
                Password = string.Empty // Şifreyi göstermiyoruz.
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                user.Username = model.Username;
                user.Email = model.Email;
                user.Mobile = model.Mobile;
                user.Address = model.Address;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    user.Password = HashPassword(model.Password);
                }

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home"); // Kullanıcıyı anasayfaya yönlendir
            }

            return View("Profile", model);
        }

        public async Task<IActionResult> PurchaseHistory()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }

            var userId = _context.Users.FirstOrDefault(u => u.Username == User.Identity.Name)?.UserId;
            var purchases = await _context.Purchases.Where(p => p.UserId == userId).ToListAsync();

            return View(purchases);
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
