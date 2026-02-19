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
