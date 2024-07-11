using Microsoft.AspNetCore.Mvc;

namespace TiklaYe.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
