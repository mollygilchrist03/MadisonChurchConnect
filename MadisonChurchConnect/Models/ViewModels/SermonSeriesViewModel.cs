/*
 * Molly Gilchrist
 * 2/19/2026
 * STG-456
 * Capstone Project
 */

namespace MadisonChurchConnect.Models.ViewModels
{
    public class SermonSeriesViewModel
    {
        public string SeriesName { get; set; } = string.Empty;
        public List<SermonVideoViewModel> Videos { get; set; } = new();
    }
}
