/*
 * Molly Gilchrist
 * 2/19/2026
 * STG-456
 * Capstone Project
 */

using MadisonChurchConnect.Models.ViewModels;

namespace MadisonChurchConnect.Services.YouTube
{
    public interface IYouTubeService
    {
        Task<AllSermonsPageViewModel> GetSermonsBySeriesAsync();
    }
}
