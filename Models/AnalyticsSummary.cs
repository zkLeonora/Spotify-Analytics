namespace SpotifyAnalytics.Web.Models;

public class AnalyticsSummary
{
    public int TotalTracks { get; set; }
    public int TotalArtists { get; set; }
    public int TotalAlbums { get; set; }
    public long TotalStreams { get; set; }
    public long TotalYoutubeViews { get; set; }
    public double AverageDanceability { get; set; }
    public double AverageEnergy { get; set; }
}