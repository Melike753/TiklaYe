using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiklaYe.Data;
using TiklaYe.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace TiklaYe.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null)
                {
                    // Şifre doğrulama işlemi
                    if (VerifyPasswordHash(model.Password, user.Password))
                    {
                        // Giriş işlemi başarılı
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Email, user.Email)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                        if ((user.Username == "Admin" || user.Email == "admintiklaye@gmail.com"))
                        {
                            // Admin sayfasına yönlendir
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            // Normal kullanıcıyı uygulamanın ana sayfasına yönlendir
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
                }
            }

            return View(model);
        }

        // Kullanıcının girdiği şifreyi hashleyip, veritabanında saklanan hashlenmiş şifre ile karşılaştırır.
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
    }
}