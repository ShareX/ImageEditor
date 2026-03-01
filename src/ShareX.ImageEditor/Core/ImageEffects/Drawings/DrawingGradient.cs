using SkiaSharp;
using System.Globalization;

namespace ShareX.ImageEditor.ImageEffects.Drawings;

public sealed class DrawingGradient
{
    public DrawingGradientMode Mode { get; set; } = DrawingGradientMode.Vertical;

    public List<DrawingGradientStop> Stops { get; set; } = [];

    public bool IsValid => Stops is { Count: > 0 };

    public bool IsVisible => IsValid && Stops.Any(x => x.Color.Alpha > 0);

    public DrawingGradient()
    {
    }

    public DrawingGradient(DrawingGradientMode mode, params DrawingGradientStop[] stops)
    {
        Mode = mode;
        Stops = stops?.ToList() ?? [];
    }

    public SKShader? CreateShader(SKRect rect)
    {
        if (!IsValid || rect.Width <= 0 || rect.Height <= 0)
        {
            return null;
        }

        List<DrawingGradientStop> orderedStops = GetNormalizedStops();
        if (orderedStops.Count == 0)
        {
            return null;
        }

        SKColor[] colors = orderedStops.Select(x => x.Color).ToArray();
        float[] positions = orderedStops.Select(x => x.Location / 100f).ToArray();
        (SKPoint start, SKPoint end) = GetGradientPoints(rect);

        return SKShader.CreateLinearGradient(start, end, colors, positions, SKShaderTileMode.Clamp);
    }

    public static List<DrawingGradientStop> ParseStops(string? value, IEnumerable<DrawingGradientStop>? fallback = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return fallback?.Select(x => new DrawingGradientStop(x.Color, x.Location)).ToList() ?? [];
        }

        List<DrawingGradientStop> parsedStops = [];
        string[] entries = value.Split([';', '|'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (string entry in entries)
        {
            int separatorIndex = entry.IndexOf(':');
            if (separatorIndex <= 0 || separatorIndex >= entry.Length - 1)
            {
                continue;
            }

            string locationText = entry[..separatorIndex].Trim();
            string colorText = entry[(separatorIndex + 1)..].Trim();

            if (!float.TryParse(locationText, NumberStyles.Float, CultureInfo.InvariantCulture, out float location))
            {
                continue;
            }

            if (!SKColor.TryParse(colorText, out SKColor color))
            {
                continue;
            }

            parsedStops.Add(new DrawingGradientStop(color, location));
        }

        if (parsedStops.Count == 0)
        {
            return fallback?.Select(x => new DrawingGradientStop(x.Color, x.Location)).ToList() ?? [];
        }

        return parsedStops;
    }

    public static string ToStopsString(IEnumerable<DrawingGradientStop> stops)
    {
        if (stops is null)
        {
            return string.Empty;
        }

        return string.Join("; ", stops.Select(x =>
            $"{x.Location.ToString("0.##", CultureInfo.InvariantCulture)}:{ToHex(x.Color)}"));
    }

    private List<DrawingGradientStop> GetNormalizedStops()
    {
        if (!IsValid)
        {
            return [];
        }

        List<DrawingGradientStop> orderedStops = Stops
            .Select(x => new DrawingGradientStop(x.Color, x.Location))
            .OrderBy(x => x.Location)
            .ToList();

        if (orderedStops.Count == 0)
        {
            return [];
        }

        if (orderedStops[0].Location > 0f)
        {
            orderedStops.Insert(0, new DrawingGradientStop(orderedStops[0].Color, 0f));
        }

        if (orderedStops[^1].Location < 100f)
        {
            orderedStops.Add(new DrawingGradientStop(orderedStops[^1].Color, 100f));
        }

        return orderedStops;
    }

    private (SKPoint start, SKPoint end) GetGradientPoints(SKRect rect)
    {
        return Mode switch
        {
            DrawingGradientMode.Horizontal => (new SKPoint(rect.Left, rect.MidY), new SKPoint(rect.Right, rect.MidY)),
            DrawingGradientMode.ForwardDiagonal => (new SKPoint(rect.Left, rect.Top), new SKPoint(rect.Right, rect.Bottom)),
            DrawingGradientMode.BackwardDiagonal => (new SKPoint(rect.Right, rect.Top), new SKPoint(rect.Left, rect.Bottom)),
            _ => (new SKPoint(rect.MidX, rect.Top), new SKPoint(rect.MidX, rect.Bottom))
        };
    }

    private static string ToHex(SKColor color)
    {
        return $"#{color.Alpha:X2}{color.Red:X2}{color.Green:X2}{color.Blue:X2}";
    }
}
