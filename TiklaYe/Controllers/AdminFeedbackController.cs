using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TiklaYe.Data;

namespace TiklaYe.Controllers
{
    public class AdminFeedbackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminFeedbackController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var feedbacks = _context.Feedbacks.OrderByDescending(f => f.CreatedDate).ToList();
            return View(feedbacks);
        }
    }
}
