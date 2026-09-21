using System.Text.Json;
using SpotifyAnalytics.Web.Pages;

namespace SpotifyAnalytics.Web.Services;

public class AnalyticsApiClient
{
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AnalyticsApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AnalyticsSummary?> GetSummaryAsync()
    {
        return await GetAsync<AnalyticsSummary>(
            "/analytics/summary"
        );
    }

    public async Task<Dictionary<string, double>?> GetTopArtistsAsync()
    {
        return await GetAsync<Dictionary<string, double>>(
            "/analytics/top-artists"
        );
    }

    public async Task<List<ArtistChartData>?> GetTopArtistsChartAsync()
    {
        return await GetAsync<List<ArtistChartData>>(
            "/analytics/top-artists-chart"
        );
    }

    public async Task<List<ArtistYoutubeData>?> GetTopArtistsYoutubeAsync()
    {
        return await GetAsync<List<ArtistYoutubeData>>(
            "/analytics/top-artists-youtube"
        );
    }

    public async Task<List<PlatformComparisonData>?> GetPlatformComparisonAsync()
    {
        return await GetAsync<List<PlatformComparisonData>>(
            "/analytics/platform-comparison"
        );
    }

    public async Task<AudioFeatures?> GetAudioFeaturesAsync()
    {
        return await GetAsync<AudioFeatures>(
            "/analytics/audio-features"
        );
    }

    public async Task<AudioProfile?> GetAudioProfileAsync()
{
    return await GetAsync<AudioProfile>(
        "/analytics/audio-profile"
    );
}

    public async Task<List<TopTrackSpotify>?> GetTopTracksSpotifyAsync()
    {
        return await GetAsync<List<TopTrackSpotify>>(
            "/analytics/top-tracks-spotify"
        );
    }

    public async Task<List<TopTrackYoutube>?> GetTopTracksYoutubeAsync()
    {
        return await GetAsync<List<TopTrackYoutube>>(
            "/analytics/top-tracks-youtube"
        );
    }

    public async Task<ArtistAnalytics?> GetArtistAsync(string artistName)
    {
        var encodedArtist = Uri.EscapeDataString(artistName);

        return await GetAsync<ArtistAnalytics>(
            $"/analytics/artist/{encodedArtist}"
        );
    }

    private async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var json = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(
                json,
                _jsonOptions
            );
        }
        catch (HttpRequestException)
        {
            return default;
        }
        catch (JsonException)
        {
            return default;
        }
    }

    public async Task<CorrelationAnalysis?> GetCorrelationAsync()
    {
        return await GetAsync<CorrelationAnalysis>(
            "/analytics/correlation"
        );
    }

    public async Task<List<AlbumTypeData>?> GetAlbumTypesAsync()
    {
        return await GetAsync<List<AlbumTypeData>>(
            "/analytics/album-types"
        );
    }
}