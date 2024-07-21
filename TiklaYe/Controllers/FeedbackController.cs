using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TiklaYe.Data;
using TiklaYe.Models;

namespace TiklaYe.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);
            ViewBag.UserName = user.Name;
            ViewBag.UserEmail = user.Email;
            return View();
        }

        [HttpPost]
        public IActionResult Index(string subject, string message)
        {
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("", "Lütfen tüm alanları doldurun.");
                return View();
            }

            var userName = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == userName);

            var feedback = new Feedback
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Subject = subject,
                Message = message
            };

            _context.Feedbacks.Add(feedback);
            _context.SaveChanges();

            TempData["Success"] = "Geri bildiriminiz başarıyla gönderildi.";
            return RedirectToAction("Index");
        }
    }
}
