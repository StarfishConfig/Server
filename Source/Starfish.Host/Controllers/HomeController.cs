using Microsoft.AspNetCore.Mvc;

namespace Nerosoft.Starfish.Host.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
