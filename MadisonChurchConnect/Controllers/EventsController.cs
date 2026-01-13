using Microsoft.AspNetCore.Mvc;

namespace MadisonChurchConnect.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
