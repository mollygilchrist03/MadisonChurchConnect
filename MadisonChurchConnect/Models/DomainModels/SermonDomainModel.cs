/*
 * Molly Gilchrist
 * 2/5/2026
 * STG-456
 * Capstone Project
 */

namespace MadisonChurchConnect.Models.DomainModels
{
    public class SermonDomainModel
    {
        public int SermonId { get; set; }
        public string? SermonTitle { get; set; }
        public string? Speaker { get; set; }
        public DateTime SermonDate { get; set; }
        public string? VideoUrl { get; set; }
        public string? Summary { get; set; }
        public string? Series { get; set; }
        public bool IsFeatured { get; set; }
    }
}
