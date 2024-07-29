using Microsoft.AspNetCore.Mvc;

namespace TiklaYe_CQRS.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
