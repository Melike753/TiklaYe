using Microsoft.AspNetCore.Mvc;

namespace TiklaYe.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
