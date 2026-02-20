/*
 * Molly Gilchrist
 * 2/19/2026
 * STG-456
 * Capstone Project
 */

namespace MadisonChurchConnect.Models.ViewModels
{
    public class SermonVideoViewModel
    {
        public string VideoId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string VideoUrl => $"https://www.youtube.com/watch?v={VideoId}";
    }
}
