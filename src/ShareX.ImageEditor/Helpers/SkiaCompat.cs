using SkiaSharp;

namespace ShareX.ImageEditor.Helpers;

internal static class SkiaCompat
{
    public static readonly SKSamplingOptions NearestNeighborSampling = new(SKFilterMode.Nearest);
    public static readonly SKSamplingOptions LowQualitySampling = new(SKFilterMode.Linear);
    public static readonly SKSamplingOptions MediumQualitySampling = new(SKFilterMode.Linear, SKMipmapMode.Linear);
    public static readonly SKSamplingOptions HighQualitySampling = new(SKCubicResampler.Mitchell);

    public static void DrawBitmap(SKCanvas canvas, SKBitmap bitmap, SKRect destinationRect, SKSamplingOptions sampling, SKPaint? paint = null)
    {
        using SKImage image = SKImage.FromBitmap(bitmap);
        canvas.DrawImage(image, destinationRect, sampling, paint);
    }

    public static void DrawBitmap(SKCanvas canvas, SKBitmap bitmap, SKRect sourceRect, SKRect destinationRect, SKSamplingOptions sampling, SKPaint? paint = null)
    {
        using SKImage image = SKImage.FromBitmap(bitmap);
        canvas.DrawImage(image, sourceRect, destinationRect, sampling, paint);
    }

    public static void DrawBitmap(SKCanvas canvas, SKBitmap bitmap, float x, float y, SKSamplingOptions sampling, SKPaint? paint = null)
    {
        using SKImage image = SKImage.FromBitmap(bitmap);
        canvas.DrawImage(image, x, y, sampling, paint);
    }
}
