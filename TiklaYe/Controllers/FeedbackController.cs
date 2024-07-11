using Microsoft.AspNetCore.Mvc;

namespace TiklaYe.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
