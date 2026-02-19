namespace MadisonChurchConnect.Models.ViewModels
{
    public class SermonSeriesViewModel
    {
        public string SeriesName { get; set; } = string.Empty;
        public List<SermonVideoViewModel> Videos { get; set; } = new();
    }
}
