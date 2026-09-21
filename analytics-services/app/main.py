# 1. import library 
import pandas as pd
import numpy as np
from fastapi import FastAPI

# 2. Load dataset
from pathlib import Path

BASE_DIR = Path(__file__).resolve().parents[2]
file_path = BASE_DIR / "data" / "Spotify_YouTube_Cleaned.csv"

df = pd.read_csv(file_path)

# 3. buat instance API nya
app = FastAPI(
    title="Spotify Analytics API",
    description="API for Spotify and YouTube analytics",
    version="1.0.0"
)

# HEALTH CHECK
@app.get("/health")
def health_check():
    return {
        "status": "ok"
    }

@app.get("/analytics/top-artists")
def top_artists():
    result = (
        df.groupby("Artist")["Stream"]
        .sum()
        .sort_values(ascending=False)
        .head(10)
    )

    return result.to_dict()

@app.get("/analytics/summary")
def analytics_summary():
    return {
        "total_tracks": int(df["Track"].nunique()),
        "total_artists": int(df["Artist"].nunique()),
        "total_albums": int(df["Album"].nunique()),
        "total_streams": int(df["Stream"].sum()),
        "total_youtube_views": int(df["Views"].sum()),
        "average_danceability": float(df["Danceability"].mean()),
        "average_energy": float(df["Energy"].mean())
    }

@app.get("/analytics/top-artists-chart")
def top_artists_chart():
    result = (
        df.groupby("Artist")["Stream"]
        .sum()
        .sort_values(ascending=False)
        .head(10)
    )

    return [
        {
            "artist": artist,
            "streams": int(streams)
        }
        for artist, streams in result.items()
    ]

@app.get("/analytics/top-artists-youtube")
def top_artists_youtube():
    result = (
        df.groupby("Artist")["Views"]
        .sum()
        .sort_values(ascending=False)
        .head(10)
    )

    return [
        {
            "artist": artist,
            "views": int(views)
        }
        for artist, views in result.items()
    ]

@app.get("/analytics/platform-comparison")
def platform_comparison():
    spotify = (
        df.groupby("Artist")["Stream"]
        .sum()
        .rename("spotify_streams")
    )

    youtube = (
        df.groupby("Artist")["Views"]
        .sum()
        .rename("youtube_views")
    )

    result = (
        pd.concat([spotify, youtube], axis=1)
        .fillna(0)
        .sort_values("spotify_streams", ascending=False)
        .head(10)
    )

    return [
        {
            "artist": artist,
            "spotify_streams": int(row["spotify_streams"]),
            "youtube_views": int(row["youtube_views"])
        }
        for artist, row in result.iterrows()
    ]

@app.get("/analytics/artist/{artist_name}")
def artist_analytics(artist_name: str):
    artist_df = df[
        df["Artist"].str.lower() == artist_name.lower()
    ]

    if artist_df.empty:
        return {
            "found": False,
            "artist": artist_name
        }

    return {
        "found": True,
        "artist": artist_df["Artist"].iloc[0],
        "tracks": int(artist_df["Track"].nunique()),
        "albums": int(artist_df["Album"].nunique()),
        "spotify_streams": int(artist_df["Stream"].sum()),
        "youtube_views": int(artist_df["Views"].sum()),
        "average_danceability": float(artist_df["Danceability"].mean()),
        "average_energy": float(artist_df["Energy"].mean())
    }

@app.get("/analytics/audio-features")
def audio_features():
    return {
        "danceability": float(df["Danceability"].mean()),
        "energy": float(df["Energy"].mean()),
        "valence": float(df["Valence"].mean()),
        "acousticness": float(df["Acousticness"].mean()),
        "speechiness": float(df["Speechiness"].mean()),
        "instrumentalness": float(df["Instrumentalness"].mean()),
        "liveness": float(df["Liveness"].mean()),
        "tempo": float(df["Tempo"].mean())
    }

@app.get("/analytics/audio-profile")
def audio_profile():
    return {
        "danceability": {
            "average": float(df["Danceability"].mean()),
            "minimum": float(df["Danceability"].min()),
            "maximum": float(df["Danceability"].max())
        },
        "energy": {
            "average": float(df["Energy"].mean()),
            "minimum": float(df["Energy"].min()),
            "maximum": float(df["Energy"].max())
        },
        "valence": {
            "average": float(df["Valence"].mean()),
            "minimum": float(df["Valence"].min()),
            "maximum": float(df["Valence"].max())
        },
        "acousticness": {
            "average": float(df["Acousticness"].mean()),
            "minimum": float(df["Acousticness"].min()),
            "maximum": float(df["Acousticness"].max())
        },
        "speechiness": {
            "average": float(df["Speechiness"].mean()),
            "minimum": float(df["Speechiness"].min()),
            "maximum": float(df["Speechiness"].max())
        },
        "instrumentalness": {
            "average": float(df["Instrumentalness"].mean()),
            "minimum": float(df["Instrumentalness"].min()),
            "maximum": float(df["Instrumentalness"].max())
        },
        "liveness": {
            "average": float(df["Liveness"].mean()),
            "minimum": float(df["Liveness"].min()),
            "maximum": float(df["Liveness"].max())
        },
        "tempo": {
            "average": float(df["Tempo"].mean()),
            "minimum": float(df["Tempo"].min()),
            "maximum": float(df["Tempo"].max())
        }
    }

@app.get("/analytics/top-tracks-spotify")
def top_tracks_spotify():
    result = (
        df.groupby(["Artist", "Track"])["Stream"]
        .sum()
        .sort_values(ascending=False)
        .head(10)
    )

    return [
        {
            "artist": artist,
            "track": track,
            "streams": int(streams)
        }
        for (artist, track), streams in result.items()
    ]


@app.get("/analytics/top-tracks-youtube")
def top_tracks_youtube():
    result = (
        df.groupby(["Artist", "Track"])["Views"]
        .sum()
        .sort_values(ascending=False)
        .head(10)
    )

    return [
        {
            "artist": artist,
            "track": track,
            "views": int(views)
        }
        for (artist, track), views in result.items()
    ]

@app.get("/analytics/correlation")
def correlation_analysis():
    correlation_df = df[["Stream", "Views"]].dropna().copy()

    correlation = correlation_df["Stream"].corr(
        correlation_df["Views"]
    )

    correlation_df["spotify_streams_log"] = (
        np.log10(correlation_df["Stream"].clip(lower=1))
    )

    correlation_df["youtube_views_log"] = (
        np.log10(correlation_df["Views"].clip(lower=1))
    )

    return {
        "correlation": float(correlation),
        "sample_size": int(len(correlation_df)),
        "data": [
            {
                "spotify_streams": float(row["spotify_streams_log"]),
                "youtube_views": float(row["youtube_views_log"])
            }
            for _, row in correlation_df.iterrows()
        ]
    }

@app.get("/analytics/album-types")
def album_types():
    result = (
        df.groupby("Album_type")
        .agg(
            tracks=("Track", "nunique"),
            streams=("Stream", "sum"),
            youtube_views=("Views", "sum")
        )
        .reset_index()
        .sort_values("streams", ascending=False)
    )

    return [
        {
            "album_type": row["Album_type"],
            "tracks": int(row["tracks"]),
            "streams": int(row["streams"]) if pd.notna(row["streams"]) else 0,
            "youtube_views": int(row["youtube_views"]) if pd.notna(row["youtube_views"]) else 0
        }
        for _, row in result.iterrows()
    ]