namespace MadisonChurchConnect.Models.ViewModels
{
    public class AllSermonsPageViewModel
    {
        public List<SermonSeriesViewModel> Series { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }
}
