/*
 * Molly Gilchrist
 * 2/9/2026
 * STG-456
 * Capstone Project
 */

using Microsoft.AspNetCore.Mvc;

namespace MadisonChurchConnect.Controllers
{
    public class SermonsController : Controller
    {
        /// <summary>
        /// returns the default view for the action.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}
