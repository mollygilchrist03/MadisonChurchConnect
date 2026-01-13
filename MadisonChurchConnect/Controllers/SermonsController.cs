using Microsoft.AspNetCore.Mvc;

namespace MadisonChurchConnect.Controllers
{
    public class SermonsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
