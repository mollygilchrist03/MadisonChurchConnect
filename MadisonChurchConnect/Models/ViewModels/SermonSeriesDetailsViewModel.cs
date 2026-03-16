namespace MadisonChurchConnect.Models.ViewModels
{
    public class SermonSeriesDetailsViewModel
    {
        public string SeriesName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<SermonVideoViewModel> Videos { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }
}
