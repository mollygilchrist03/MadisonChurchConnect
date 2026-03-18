/*
 * Molly Gilchrist
 * 2/19/2026
 * STG-456
 * Capstone Project
 */

namespace MadisonChurchConnect.Models.ViewModels
{
    public class AllSermonsPageViewModel
    {
        public List<SermonSeriesViewModel> Series { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public string? CurrentSeriesName { get; set; }
    }
}
