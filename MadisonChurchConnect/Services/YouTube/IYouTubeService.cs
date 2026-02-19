using MadisonChurchConnect.Models.ViewModels;

namespace MadisonChurchConnect.Services.YouTube
{
    public interface IYouTubeService
    {
        Task<AllSermonsPageViewModel> GetSermonsBySeriesAsync();
    }
}
