using System.Text.Json;
using MadisonChurchConnect.Models.ViewModels;
using Microsoft.Extensions.Options;

namespace MadisonChurchConnect.Services.YouTube
{
    public class YouTubeService : IYouTubeService
    {
        private readonly HttpClient _httpClient;
        private readonly YouTubeOptions _options;

        public YouTubeService(HttpClient httpClient, IOptions<YouTubeOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<AllSermonsPageViewModel> GetSermonsBySeriesAsync()
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
            {
                return new AllSermonsPageViewModel
                {
                    ErrorMessage = "YouTube API key is not configured."
                };
            }

            string? uploadsPlaylistId = await GetUploadsPlaylistIdAsync();
            if (string.IsNullOrWhiteSpace(uploadsPlaylistId))
            {
                return new AllSermonsPageViewModel
                {
                    ErrorMessage = "Unable to load videos from YouTube right now."
                };
            }

            List<SermonVideoViewModel> videos = await GetVideosFromUploadsPlaylistAsync(uploadsPlaylistId);
            List<SermonSeriesViewModel> groupedSeries = videos
                .GroupBy(video => GetSeriesNameFromTitle(video.Title))
                .OrderBy(group => group.Key)
                .Select(group => new SermonSeriesViewModel
                {
                    SeriesName = group.Key,
                    Videos = group
                        .OrderByDescending(video => video.PublishedAt)
                        .ToList()
                })
                .ToList();

            return new AllSermonsPageViewModel
            {
                Series = groupedSeries
            };
        }

        private async Task<string?> GetUploadsPlaylistIdAsync()
        {
            string requestUrl =
                $"https://www.googleapis.com/youtube/v3/channels?part=contentDetails&forHandle={_options.ChannelHandle}&key={_options.ApiKey}";

            using HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            using Stream contentStream = await response.Content.ReadAsStreamAsync();
            using JsonDocument document = await JsonDocument.ParseAsync(contentStream);

            if (!document.RootElement.TryGetProperty("items", out JsonElement items) || items.GetArrayLength() == 0)
            {
                return null;
            }

            JsonElement firstItem = items[0];
            return firstItem
                .GetProperty("contentDetails")
                .GetProperty("relatedPlaylists")
                .GetProperty("uploads")
                .GetString();
        }

        private async Task<List<SermonVideoViewModel>> GetVideosFromUploadsPlaylistAsync(string uploadsPlaylistId)
        {
            string requestUrl =
                $"https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId={uploadsPlaylistId}&maxResults={_options.MaxResults}&key={_options.ApiKey}";

            using HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                return new List<SermonVideoViewModel>();
            }

            using Stream contentStream = await response.Content.ReadAsStreamAsync();
            using JsonDocument document = await JsonDocument.ParseAsync(contentStream);

            if (!document.RootElement.TryGetProperty("items", out JsonElement items))
            {
                return new List<SermonVideoViewModel>();
            }

            List<SermonVideoViewModel> videos = new();

            foreach (JsonElement item in items.EnumerateArray())
            {
                JsonElement snippet = item.GetProperty("snippet");
                string? videoId = snippet.GetProperty("resourceId").GetProperty("videoId").GetString();

                if (string.IsNullOrWhiteSpace(videoId))
                {
                    continue;
                }

                string thumbnailUrl = string.Empty;
                if (snippet.GetProperty("thumbnails").TryGetProperty("high", out JsonElement highThumb))
                {
                    thumbnailUrl = highThumb.GetProperty("url").GetString() ?? string.Empty;
                }
                else if (snippet.GetProperty("thumbnails").TryGetProperty("medium", out JsonElement mediumThumb))
                {
                    thumbnailUrl = mediumThumb.GetProperty("url").GetString() ?? string.Empty;
                }
                else if (snippet.GetProperty("thumbnails").TryGetProperty("default", out JsonElement defaultThumb))
                {
                    thumbnailUrl = defaultThumb.GetProperty("url").GetString() ?? string.Empty;
                }

                DateTime publishedAt = DateTime.MinValue;
                DateTime.TryParse(snippet.GetProperty("publishedAt").GetString(), out publishedAt);

                videos.Add(new SermonVideoViewModel
                {
                    VideoId = videoId,
                    Title = snippet.GetProperty("title").GetString() ?? "Untitled Sermon",
                    Description = snippet.GetProperty("description").GetString() ?? string.Empty,
                    PublishedAt = publishedAt,
                    ThumbnailUrl = thumbnailUrl
                });
            }

            return videos;
        }

        private static string GetSeriesNameFromTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return "Other Sermons";
            }

            string[] separators = [" | ", " - ", ": ", " — "];
            foreach (string separator in separators)
            {
                if (title.Contains(separator, StringComparison.Ordinal))
                {
                    return title.Split(separator, 2, StringSplitOptions.TrimEntries)[0];
                }
            }

            return "Other Sermons";
        }
    }
}
