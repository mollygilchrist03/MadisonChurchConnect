/*
 * Molly Gilchrist
 * 1/15/2026
 * STG-456
 * Capstone Project
 * change for deployment
 */

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
