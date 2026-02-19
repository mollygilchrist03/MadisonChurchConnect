/*
 * Molly Gilchrist
 * 2/9/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.ViewModels;
using MadisonChurchConnect.Services.YouTube;
using Microsoft.AspNetCore.Mvc;

namespace MadisonChurchConnect.Controllers
{
    public class SermonsController : Controller
    {
        private readonly IYouTubeService _youTubeService;

        public SermonsController(IYouTubeService youTubeService)
        {
            _youTubeService = youTubeService;
        }

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
        /// <returns></returns>
        public async Task<IActionResult> AllSermons()
        {
            AllSermonsPageViewModel model = await _youTubeService.GetSermonsBySeriesAsync();
            return View(model);
        }
    }
}
