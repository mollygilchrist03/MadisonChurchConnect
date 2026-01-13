using Microsoft.AspNetCore.Mvc;

namespace MadisonChurchConnect.Controllers
{
    public class ConnectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
