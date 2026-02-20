/*
 * Molly Gilchrist
 * 2/19/2026
 * STG-456
 * Capstone Project
 */

namespace MadisonChurchConnect.Services.YouTube
{
    public class YouTubeOptions
    {
        public const string SectionName = "YouTube";

        public string ApiKey { get; set; } = string.Empty;
        public string ChannelHandle { get; set; } = "madisonchurchphx";
        public int MaxResults { get; set; } = 50;
    }
}
