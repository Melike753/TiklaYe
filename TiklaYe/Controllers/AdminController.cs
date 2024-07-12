using Microsoft.AspNetCore.Mvc;

namespace TiklaYe.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
