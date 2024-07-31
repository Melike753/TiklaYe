using Microsoft.AspNetCore.Mvc;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Data;

namespace TiklaYe_CQRS.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CreateFeedbackCommandHandler _createFeedbackHandler;

        public FeedbackController(ApplicationDbContext context, CreateFeedbackCommandHandler createFeedbackHandler)
        {
            _context = context;
            _createFeedbackHandler = createFeedbackHandler;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(u => u.Username == userName);

                if (user == null)
                {
                    ModelState.AddModelError("", "Geri bildirim göndermek için önce giriş yapmalısınız..");
                    return View();
                }

                ViewBag.UserName = user.Name;
                ViewBag.UserEmail = user.Email;
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(string subject, string message)
        {
            if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
            {
                ModelState.AddModelError("", "Lütfen tüm alanları doldurun.");
                return View();
            }

            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(u => u.Username == userName);

                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    return View();
                }

                var command = new CreateFeedbackCommand
                {
                    UserId = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Subject = subject,
                    Message = message
                };

                try
                {
                    await _createFeedbackHandler.Handle(command);
                    TempData["Success"] = "Geri bildiriminiz başarıyla gönderildi.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Geri bildirim gönderilirken bir hata oluştu: " + ex.Message);
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Oturum açmış kullanıcı bulunamadı.");
                return View();
            }
        }
    }
}