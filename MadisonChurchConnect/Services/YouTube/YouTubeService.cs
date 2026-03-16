/*
 * Molly Gilchrist
 * 2/19/2026
 * STG-456
 * Capstone Project
 */

using System.Text.Json;
using MadisonChurchConnect.Models.ViewModels;
using Microsoft.Extensions.Options;

namespace MadisonChurchConnect.Services.YouTube
{
    public class YouTubeService : IYouTubeService
    {
        private static readonly HashSet<string> ExcludedPlaylistNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "MC Rewind",
            "Small Group Material",
            "Latest Sermons"
        };

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
                    ErrorMessage = "YouTube API key is not configured. If you run in Development, ensure appsettings.Development.json does not override YouTube:ApiKey with an empty value."
                };
            }

            string? channelId = await GetChannelIdAsync();
            if (string.IsNullOrWhiteSpace(channelId))
            {
                return new AllSermonsPageViewModel
                {
                    ErrorMessage = "Unable to load sermon series from YouTube right now."
                };
            }

            List<SermonSeriesViewModel> playlistSeries = await GetSeriesFromPlaylistsAsync(channelId);

            return new AllSermonsPageViewModel
            {
                Series = playlistSeries
            };
        }

        private async Task<string?> GetChannelIdAsync()
        {
            string requestUrl =
                $"https://www.googleapis.com/youtube/v3/channels?part=id&forHandle={_options.ChannelHandle}&key={_options.ApiKey}";

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
            return firstItem.GetProperty("id").GetString();
        }

        private async Task<List<SermonSeriesViewModel>> GetSeriesFromPlaylistsAsync(string channelId)
        {
            List<SermonSeriesViewModel> series = new();
            string? nextPageToken = null;

            do
            {
                string requestUrl =
                    $"https://www.googleapis.com/youtube/v3/playlists?part=snippet,contentDetails&channelId={channelId}&maxResults=50&key={_options.ApiKey}";

                if (!string.IsNullOrWhiteSpace(nextPageToken))
                {
                    requestUrl += $"&pageToken={nextPageToken}";
                }

                using HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    break;
                }

                using Stream contentStream = await response.Content.ReadAsStreamAsync();
                using JsonDocument document = await JsonDocument.ParseAsync(contentStream);

                if (!document.RootElement.TryGetProperty("items", out JsonElement items))
                {
                    break;
                }

                foreach (JsonElement item in items.EnumerateArray())
                {
                    JsonElement snippet = item.GetProperty("snippet");
                    string playlistId = item.GetProperty("id").GetString() ?? string.Empty;
                    string playlistTitle = snippet.GetProperty("title").GetString() ?? "Untitled Series";
                    string playlistDescription = snippet.GetProperty("description").GetString() ?? string.Empty;

                    if (string.IsNullOrWhiteSpace(playlistId))
                    {
                        continue;
                    }

                    if (ExcludedPlaylistNames.Contains(playlistTitle))
                    {
                        continue;
                    }

                    List<SermonVideoViewModel> playlistVideos = await GetVideosFromPlaylistAsync(playlistId);
                    if (!playlistVideos.Any())
                    {
                        continue;
                    }

                    series.Add(new SermonSeriesViewModel
                    {
                        PlaylistId = playlistId,
                        SeriesName = playlistTitle,
                        Description = playlistDescription,
                        Videos = playlistVideos
                            .OrderBy(video => video.PublishedAt)
                            .ToList()
                    });
                }

                nextPageToken = document.RootElement.TryGetProperty("nextPageToken", out JsonElement pageTokenElement)
                    ? pageTokenElement.GetString()
                    : null;
            }
            while (!string.IsNullOrWhiteSpace(nextPageToken));

            return series
                .OrderByDescending(sermonSeries => sermonSeries.Videos.Max(video => video.PublishedAt))
                .ThenBy(sermonSeries => sermonSeries.SeriesName)
                .ToList();
        }

        private async Task<List<SermonVideoViewModel>> GetVideosFromPlaylistAsync(string playlistId)
        {
            List<SermonVideoViewModel> videos = new();
            string? nextPageToken = null;

            do
            {
                string requestUrl =
                    $"https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId={playlistId}&maxResults=50&key={_options.ApiKey}";

                if (!string.IsNullOrWhiteSpace(nextPageToken))
                {
                    requestUrl += $"&pageToken={nextPageToken}";
                }

                using HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                if (!response.IsSuccessStatusCode)
                {
                    break;
                }

                using Stream contentStream = await response.Content.ReadAsStreamAsync();
                using JsonDocument document = await JsonDocument.ParseAsync(contentStream);

                if (!document.RootElement.TryGetProperty("items", out JsonElement items))
                {
                    break;
                }

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

                    if (videos.Count >= _options.MaxResults)
                    {
                        break;
                    }
                }

                if (videos.Count >= _options.MaxResults)
                {
                    break;
                }

                nextPageToken = document.RootElement.TryGetProperty("nextPageToken", out JsonElement pageTokenElement)
                    ? pageTokenElement.GetString()
                    : null;
            }
            while (!string.IsNullOrWhiteSpace(nextPageToken));

            return videos;
        }
    }
}
