using ShareX.ImageEditor.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.Annotations;

/// <summary>
/// Magnify annotation - zooms into the area
/// </summary>
public partial class MagnifyAnnotation : BaseEffectAnnotation
{
    public MagnifyAnnotation()
    {
        ToolType = EditorTool.Magnify;
        StrokeColor = "#00000000"; // Transparent border
        StrokeWidth = 0;
        Amount = 2.0f; // Zoom level (2x)
    }

    internal override string GetInteractionCacheKey()
    {
        return GetType().FullName ?? nameof(MagnifyAnnotation);
    }

    internal override SKBitmap? CreateInteractionCacheBitmap(SKBitmap source)
    {
        return source?.Copy();
    }

    internal override void UpdateEffectFromInteractionCache(SKBitmap source, SKBitmap cachedEffectBitmap)
    {
        UpdateEffectCore(source, cachedEffectBitmap);
    }

    public override void UpdateEffect(SKBitmap source)
    {
        if (source == null) return;

        UpdateEffectCore(source, source);
    }

    private void UpdateEffectCore(SKBitmap source, SKBitmap drawSource)
    {
        if (source == null || drawSource == null) return;

        var rect = GetBounds();
        int fullW = (int)rect.Width;
        int fullH = (int)rect.Height;
        if (fullW <= 0 || fullH <= 0) return;

        // Convert annotation bounds to integer rect
        var annotationRect = new SKRectI((int)rect.Left, (int)rect.Top, (int)rect.Right, (int)rect.Bottom);

        // Find intersection with source image bounds
        var validRect = annotationRect;
        validRect.Intersect(new SKRectI(0, 0, source.Width, source.Height));

        // Create result bitmap at FULL annotation size
        var result = new SKBitmap(fullW, fullH);
        result.Erase(SKColors.Transparent);

        if (validRect.Width <= 0 || validRect.Height <= 0)
        {
            EffectBitmap?.Dispose();
            EffectBitmap = result;
            return;
        }

        // For magnification, capture a SMALLER area from the CENTER OF THE VALID REGION and scale it UP
        // Use the valid region's center to avoid capturing outside the image
        float zoom = Math.Max(1.0f, Amount);

        // Calculate capture size based on valid region (not full annotation)
        float captureWidth = validRect.Width / zoom;
        float captureHeight = validRect.Height / zoom;

        // Center the capture within the valid region
        float centerX = validRect.Left + validRect.Width / 2f;
        float centerY = validRect.Top + validRect.Height / 2f;

        float captureX = centerX - (captureWidth / 2);
        float captureY = centerY - (captureHeight / 2);

        var captureRect = new SKRectI(
            (int)captureX,
            (int)captureY,
            (int)(captureX + captureWidth),
            (int)(captureY + captureHeight)
        );

        // Ensure capture is within source bounds
        captureRect.Intersect(new SKRectI(0, 0, source.Width, source.Height));

        if (captureRect.Width <= 0 || captureRect.Height <= 0)
        {
            EffectBitmap?.Dispose();
            EffectBitmap = result;
            return;
        }

        // Draw scaled content at the correct offset within the full-size result
        int drawX = validRect.Left - annotationRect.Left;
        int drawY = validRect.Top - annotationRect.Top;

        using (var resultCanvas = new SKCanvas(result))
        using (var paint = new SKPaint())
        {
            var sourceRect = new SKRect(captureRect.Left, captureRect.Top, captureRect.Right, captureRect.Bottom);
            var destinationRect = new SKRect(drawX, drawY, drawX + validRect.Width, drawY + validRect.Height);
            SkiaCompat.DrawBitmap(resultCanvas, drawSource, sourceRect, destinationRect, SkiaCompat.MediumQualitySampling, paint);
        }

        EffectBitmap?.Dispose();
        EffectBitmap = result;
    }
}
