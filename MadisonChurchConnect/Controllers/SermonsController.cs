/*
 * Molly Gilchrist
 * 2/9/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.ViewModels;
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

        /// <summary>
        /// displays the view for all sermons.
        /// </summary>
        /// <param name="allSermons"></param>
        /// <returns></returns>
        public IActionResult AllSermons(List<SermonViewModel> allSermons)
        {
            return View();
        }
    }
}
