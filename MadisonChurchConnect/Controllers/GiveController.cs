/*
 * Molly Gilchrist
 * 1/15/2026
 * STG-456
 * Capstone Project
 */

using Microsoft.AspNetCore.Mvc;

namespace MadisonChurchConnect.Controllers
{
    public class GiveController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("https://madisonphx.faithteams.com/give");
        }
    }
}
