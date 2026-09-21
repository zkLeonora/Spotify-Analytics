using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Serialization;
using SpotifyAnalytics.Web.Services;

namespace SpotifyAnalytics.Web.Pages;

public class IndexModel : PageModel
{
    private readonly AnalyticsApiClient _analyticsApi;

    public Dictionary<string, double>? TopArtists { get; set; }

    public AnalyticsSummary? Summary { get; set; }

    public List<ArtistChartData>? TopArtistsChart { get; set; }

    public List<ArtistYoutubeData>? TopArtistsYoutube { get; set; }

    public List<PlatformComparisonData>? PlatformComparison { get; set; }

    public ArtistAnalytics? ArtistResult { get; set; }

    public AudioFeatures? AudioFeatures { get; set; }

    public AudioProfile? AudioProfile { get; set; }

    public List<TopTrackSpotify>? TopTracksSpotify { get; set; }

    public List<TopTrackYoutube>? TopTracksYoutube { get; set; }

    public CorrelationAnalysis? Correlation { get; set; }

    public List<AlbumTypeData>? AlbumTypes { get; set; }

    [BindProperty]
    public string ArtistName { get; set; } = string.Empty;

    public IndexModel(AnalyticsApiClient analyticsApi)
    {
        _analyticsApi = analyticsApi;
    }

    public async Task OnGetAsync()
    {
        await LoadDashboardData();
    }

    public async Task OnPostAsync()
    {
        await LoadDashboardData();

        if (!string.IsNullOrWhiteSpace(ArtistName))
        {
            ArtistResult = await _analyticsApi.GetArtistAsync(
                ArtistName.Trim()
            );
        }
    }

    private async Task LoadDashboardData()
    {
        TopArtists = await _analyticsApi.GetTopArtistsAsync();

        Summary = await _analyticsApi.GetSummaryAsync();

        TopArtistsChart = await _analyticsApi.GetTopArtistsChartAsync();

        TopArtistsYoutube = await _analyticsApi.GetTopArtistsYoutubeAsync();

        PlatformComparison = await _analyticsApi.GetPlatformComparisonAsync();

        AudioFeatures = await _analyticsApi.GetAudioFeaturesAsync();

        AudioProfile = await _analyticsApi.GetAudioProfileAsync();

        TopTracksSpotify = await _analyticsApi.GetTopTracksSpotifyAsync();

        TopTracksYoutube = await _analyticsApi.GetTopTracksYoutubeAsync();

        Correlation = await _analyticsApi.GetCorrelationAsync();

        AlbumTypes = await _analyticsApi.GetAlbumTypesAsync();
    }
}

public class AnalyticsSummary
{
    [JsonPropertyName("total_tracks")]
    public int TotalTracks { get; set; }

    [JsonPropertyName("total_artists")]
    public int TotalArtists { get; set; }

    [JsonPropertyName("total_albums")]
    public int TotalAlbums { get; set; }

    [JsonPropertyName("total_streams")]
    public long TotalStreams { get; set; }

    [JsonPropertyName("total_youtube_views")]
    public long TotalYoutubeViews { get; set; }

    [JsonPropertyName("average_danceability")]
    public double AverageDanceability { get; set; }

    [JsonPropertyName("average_energy")]
    public double AverageEnergy { get; set; }
}

public class ArtistChartData
{
    public string Artist { get; set; } = string.Empty;

    public long Streams { get; set; }
}

public class ArtistYoutubeData
{
    public string Artist { get; set; } = string.Empty;

    public long Views { get; set; }
}

public class PlatformComparisonData
{
    [JsonPropertyName("artist")]
    public string Artist { get; set; } = string.Empty;

    [JsonPropertyName("spotify_streams")]
    public long SpotifyStreams { get; set; }

    [JsonPropertyName("youtube_views")]
    public long YoutubeViews { get; set; }
}

public class ArtistAnalytics
{
    [JsonPropertyName("found")]
    public bool Found { get; set; }

    [JsonPropertyName("artist")]
    public string Artist { get; set; } = string.Empty;

    [JsonPropertyName("tracks")]
    public int Tracks { get; set; }

    [JsonPropertyName("albums")]
    public int Albums { get; set; }

    [JsonPropertyName("spotify_streams")]
    public long SpotifyStreams { get; set; }

    [JsonPropertyName("youtube_views")]
    public long YoutubeViews { get; set; }

    [JsonPropertyName("average_danceability")]
    public double AverageDanceability { get; set; }

    [JsonPropertyName("average_energy")]
    public double AverageEnergy { get; set; }
}

public class AudioFeatures
{
    [JsonPropertyName("danceability")]
    public double Danceability { get; set; }

    [JsonPropertyName("energy")]
    public double Energy { get; set; }

    [JsonPropertyName("valence")]
    public double Valence { get; set; }

    [JsonPropertyName("acousticness")]
    public double Acousticness { get; set; }

    [JsonPropertyName("speechiness")]
    public double Speechiness { get; set; }

    [JsonPropertyName("instrumentalness")]
    public double Instrumentalness { get; set; }

    [JsonPropertyName("liveness")]
    public double Liveness { get; set; }

    [JsonPropertyName("tempo")]
    public double Tempo { get; set; }
}

public class TopTrackSpotify
{
    public string Artist { get; set; } = string.Empty;

    public string Track { get; set; } = string.Empty;

    public long Streams { get; set; }
}

public class TopTrackYoutube
{
    public string Artist { get; set; } = string.Empty;

    public string Track { get; set; } = string.Empty;

    public long Views { get; set; }
}

public class AudioProfile
{
    public AudioFeatureStats? Danceability { get; set; }

    public AudioFeatureStats? Energy { get; set; }

    public AudioFeatureStats? Valence { get; set; }

    public AudioFeatureStats? Acousticness { get; set; }

    public AudioFeatureStats? Speechiness { get; set; }

    public AudioFeatureStats? Instrumentalness { get; set; }

    public AudioFeatureStats? Liveness { get; set; }

    public AudioFeatureStats? Tempo { get; set; }
}

public class AudioFeatureStats
{
    public double Average { get; set; }

    public double Minimum { get; set; }

    public double Maximum { get; set; }
}

public class CorrelationAnalysis
{
    [JsonPropertyName("correlation")]
    public double Correlation { get; set; }

    [JsonPropertyName("sample_size")]
    public int SampleSize { get; set; }

    [JsonPropertyName("data")]
    public List<CorrelationPoint> Data { get; set; } = new();
}

public class CorrelationPoint
{
    [JsonPropertyName("spotify_streams")]
    public double SpotifyStreams { get; set; }

    [JsonPropertyName("youtube_views")]
    public double YoutubeViews { get; set; }
}

public class AlbumTypeData
{
    [JsonPropertyName("album_type")]
    public string AlbumType { get; set; } = string.Empty;

    [JsonPropertyName("tracks")]
    public long Tracks { get; set; }

    [JsonPropertyName("streams")]
    public long Streams { get; set; }

    [JsonPropertyName("youtube_views")]
    public long YoutubeViews { get; set; }
}
