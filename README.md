# Spotify Analytics Web

A web application for exploring music performance data across Spotify and YouTube. Combines an ASP.NET Core Razor Pages frontend with a Python FastAPI analytics backend, using Pandas and NumPy for data processing.

## Overview

The app loads a dataset of Spotify and YouTube music metadata and lets you browse top artists, top tracks, audio feature profiles, album type breakdowns, and a correlation analysis between Spotify streams and YouTube views. There is also an artist search feature for looking up individual artist stats.

## Tech Stack

- **ASP.NET Core 10.0** (Razor Pages) -- web frontend, server-rendered UI
- **Python 3.12** + **FastAPI** -- analytics API service
- **Pandas** + **NumPy** -- data processing and analysis
- **Chart.js** -- client-side charts
- **Spotify + YouTube CSV dataset** -- source data

## Architecture

```
Browser
  |
  v
ASP.NET Core / Razor Pages  (port 5006)
  |  HTTP
  v
Python FastAPI              (port 8000)
  |
  v
Pandas / NumPy
  |
  v
Spotify + YouTube CSV
```

The ASP.NET Core app serves the UI and calls the FastAPI backend for analytics data via HTTP. The FastAPI service loads the CSV into a Pandas DataFrame on startup and exposes endpoints for different analytics views.

## Project Structure

```
SpotifyAnalytics/
  README.md
  SpotifyAnalytics.web/
    Program.cs                          # ASP.NET Core entry point
    SpotifyAnalytics.web.csproj         # .NET project file
    appsettings.json
    Properties/
      launchSettings.json
    Pages/
      Index.cshtml                      # Main dashboard page
      Index.cshtml.cs                   # Page model (loads data from API)
      Privacy.cshtml
      Privacy.cshtml.cs
      Error.cshtml
      Error.cshtml.cs
      Shared/
        _Layout.cshtml                  # Layout template
        _Layout.cshtml.css
        _ValidationScriptsPartial.cshtml
    Models/
      Songs.cs
      AnalyticsSummary.cs
    SpotifyAnalytics.web/
      Services/
        AnalyticsApiClient.cs           # HTTP client for FastAPI
    data/
      Spotify_Youtube.csv               # Raw dataset
      Spotify_YouTube_Cleaned.csv       # Cleaned dataset
    wwwroot/
      css/site.css
      js/site.js
      favicon.ico
      lib/                              # Bootstrap, jQuery
    analytics-services/
      requirements.txt                  # Python dependencies
      app/
        main.py                         # FastAPI application
```

## How It Works

1. On startup, FastAPI reads `Spotify_YouTube_Cleaned.csv` into a Pandas DataFrame.
2. When you open the web app, `Index.cshtml.cs` calls every analytics endpoint on the FastAPI service.
3. The data is passed to the Razor view, which renders summary cards, tables, and Chart.js charts.
4. Searching for an artist calls `/analytics/artist/{name}` and displays per-artist stats.

## Dataset

The dataset (`data/Spotify_YouTube_Cleaned.csv`) contains Spotify and YouTube music metadata with approximately 546,000 rows. Columns include:

| Column | Description |
|--------|-------------|
| `Artist` | Artist name |
| `Track` | Track/song name |
| `Album` | Album name |
| `Album_type` | Album type (album, single, compilation) |
| `Stream` | Spotify stream count |
| `Views` | YouTube view count |
| `Danceability` | Spotify audio feature (0-1) |
| `Energy` | Spotify audio feature (0-1) |
| `Valence` | Spotify audio feature (0-1) |
| `Acousticness` | Spotify audio feature (0-1) |
| `Speechiness` | Spotify audio feature (0-1) |
| `Instrumentalness` | Spotify audio feature (0-1) |
| `Liveness` | Spotify audio feature (0-1) |
| `Tempo` | BPM |
| `Likes` | YouTube likes |
| `Comments` | YouTube comments |

## Local Development Setup

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Python 3.12+](https://www.python.org/downloads/)

### 1. Start the FastAPI analytics service

```bash
cd SpotifyAnalytics.web/analytics-services

# Create and activate virtual environment
python -m venv .venv

# Windows
.venv\Scripts\activate

# macOS/Linux
source .venv/bin/activate

# Install dependencies
pip install -r requirements.txt

# Start the server
uvicorn app.main:app --reload --port 8000
```

The API will be available at `http://127.0.0.1:8000`. You can verify it is running by opening `http://127.0.0.1:8000/health`.

### 2. Start the ASP.NET Core web app

In a separate terminal:

```bash
cd SpotifyAnalytics.web

dotnet run
```

The web app will be available at `http://localhost:5006`.

## API Endpoints

All endpoints are served by the FastAPI service at `http://127.0.0.1:8000`.

| Method | Path | Description |
|--------|------|-------------|
| GET | `/health` | Health check |
| GET | `/analytics/summary` | Dashboard summary stats |
| GET | `/analytics/top-artists` | Top 10 artists by Spotify streams |
| GET | `/analytics/top-artists-chart` | Top 10 artists (chart format) |
| GET | `/analytics/top-artists-youtube` | Top 10 artists by YouTube views |
| GET | `/analytics/platform-comparison` | Spotify vs YouTube for top 10 artists |
| GET | `/analytics/artist/{artist_name}` | Per-artist analytics |
| GET | `/analytics/audio-features` | Average audio feature values |
| GET | `/analytics/audio-profile` | Audio features with min/max stats |
| GET | `/analytics/top-tracks-spotify` | Top 10 tracks by Spotify streams |
| GET | `/analytics/top-tracks-youtube` | Top 10 tracks by YouTube views |
| GET | `/analytics/correlation` | Spotify streams vs YouTube views correlation |
| GET | `/analytics/album-types` | Performance breakdown by album type |

### Example: Get top artists

```bash
curl http://127.0.0.1:8000/analytics/top-artists
```

Response:

```json
{
  "Artist Name": 1234567890,
  "Another Artist": 987654321
}
```

### Example: Search for an artist

```bash
curl http://127.0.0.1:8000/analytics/artist/Gorillaz
```

Response:

```json
{
  "found": true,
  "artist": "Gorillaz",
  "tracks": 10,
  "albums": 3,
  "spotify_streams": 1234567890,
  "youtube_views": 987654321,
  "average_danceability": 0.72,
  "average_energy": 0.65
}
```

## Current Status

The project is functional with the following features working:

- Dashboard with summary stats (total tracks, artists, albums, streams, views)
- Top 10 artists by Spotify streams (bar chart)
- Top 10 artists by YouTube views (bar chart)
- Top 10 tracks on Spotify (table)
- Top 10 tracks on YouTube (table)
- Platform comparison: Spotify streams vs YouTube views (table)
- Audio feature profile with bar visualization
- Audio characteristics chart
- Spotify streams vs YouTube views correlation (scatter plot)
- Album type analysis (grouped bar chart)
- Top artists ranking (table)
- Artist search with per-artist stats

## Roadmap

- Sorting and filtering on tables
- Time-based analysis (if date data becomes available)
- Artist comparison view
- Export data to CSV
- Responsive chart improvements
- Error handling improvements when the FastAPI service is down

## License

This project does not currently have a license. Contact the author for usage terms.
