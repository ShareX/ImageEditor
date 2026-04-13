using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Helpers;

internal static class SkiaImageHelper
{
    public static readonly SKSamplingOptions HighQualitySampling = new(SKCubicResampler.Mitchell);
    public static readonly SKSamplingOptions MediumQualitySampling = new(SKFilterMode.Linear, SKMipmapMode.Linear);
    public static readonly SKSamplingOptions LinearSampling = new(SKFilterMode.Linear);
    public static readonly SKSamplingOptions NearestNeighborSampling = new(SKFilterMode.Nearest);

    public static SKBitmap Resize(SKBitmap source, SKImageInfo info, SKSamplingOptions sampling)
    {
        return source.Resize(info, sampling) ?? new SKBitmap(info);
    }

    public static void DrawBitmap(SKCanvas canvas, SKBitmap bitmap, SKRect destination, SKSamplingOptions sampling, SKPaint? paint = null)
    {
        using SKImage image = SKImage.FromBitmap(bitmap);
        canvas.DrawImage(image, destination, sampling, paint);
    }

    public static void DrawBitmap(SKCanvas canvas, SKBitmap bitmap, SKRect source, SKRect destination, SKSamplingOptions sampling, SKPaint? paint = null)
    {
        using SKImage image = SKImage.FromBitmap(bitmap);
        canvas.DrawImage(image, source, destination, sampling, paint);
    }

    public static void DrawBitmap(SKCanvas canvas, SKBitmap bitmap, float x, float y, SKSamplingOptions sampling, SKPaint? paint = null)
    {
        using SKImage image = SKImage.FromBitmap(bitmap);
        canvas.DrawImage(image, x, y, sampling, paint);
    }

    public static SKShader CreateShader(SKBitmap bitmap, SKShaderTileMode tileX, SKShaderTileMode tileY, SKSamplingOptions sampling)
    {
        return bitmap.ToShader(tileX, tileY, sampling);
    }
}
