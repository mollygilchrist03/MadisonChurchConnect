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
        public async Task<IActionResult> Index()
        {
            AllSermonsPageViewModel allSermons = await _youTubeService.GetSermonsBySeriesAsync();

            if (!string.IsNullOrWhiteSpace(allSermons.ErrorMessage))
            {
                return View(new AllSermonsPageViewModel
                {
                    ErrorMessage = allSermons.ErrorMessage
                });
            }

            List<SermonSeriesViewModel> recentSeries = allSermons.Series
                .OrderByDescending(series => series.Videos.Max(video => video.PublishedAt))
                .Take(4)
                .ToList();

            return View(new AllSermonsPageViewModel
            {
                Series = recentSeries
            });
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

        /// <summary>
        /// displays a single series and all sermons in chronological order.
        /// </summary>
        /// <param name="seriesName"></param>
        /// <returns></returns>
        public async Task<IActionResult> SeriesDetails(string seriesName)
        {
            AllSermonsPageViewModel allSermons = await _youTubeService.GetSermonsBySeriesAsync();

            if (!string.IsNullOrWhiteSpace(allSermons.ErrorMessage))
            {
                return View(new SermonSeriesDetailsViewModel
                {
                    SeriesName = seriesName ?? string.Empty,
                    ErrorMessage = allSermons.ErrorMessage
                });
            }

            SermonSeriesViewModel? matchedSeries = allSermons.Series
                .FirstOrDefault(series => string.Equals(series.SeriesName, seriesName, StringComparison.OrdinalIgnoreCase));

            if (matchedSeries == null)
            {
                return View(new SermonSeriesDetailsViewModel
                {
                    SeriesName = seriesName ?? string.Empty,
                    ErrorMessage = "That sermon series could not be found."
                });
            }

            SermonSeriesDetailsViewModel model = new SermonSeriesDetailsViewModel
            {
                SeriesName = matchedSeries.SeriesName,
                Videos = matchedSeries.Videos
                    .OrderBy(video => video.PublishedAt)
                    .ToList()
            };

            return View(model);
        }
    }
}
