using SkiaSharp;
using System.Globalization;
using System.Text.Json.Serialization;

namespace ShareX.ImageEditor.Core.Annotations;

/// <summary>
/// Base class for effect annotations (Blur, Pixelate, Highlight)
/// </summary>
public abstract class BaseEffectAnnotation : Annotation, IDisposable
{
    public override AnnotationCategory Category => AnnotationCategory.Effects;

    /// <summary>
    /// Effect radius / strength
    /// </summary>
    public float Amount { get; set; } = 10;

    /// <summary>
    /// Whether the effect is applied as a region (rectangle) or freehand
    /// </summary>
    public bool IsFreehand { get; set; }

    /// <summary>
    /// The generated bitmap for the effect (pre-rendered effect result)
    /// </summary>
    [JsonIgnore]
    public SKBitmap? EffectBitmap { get; protected set; }

    public override SKRect GetBounds()
    {
        return new SKRect(
            Math.Min(StartPoint.X, EndPoint.X),
            Math.Min(StartPoint.Y, EndPoint.Y),
            Math.Max(StartPoint.X, EndPoint.X),
            Math.Max(StartPoint.Y, EndPoint.Y));
    }

    public override bool HitTest(SKPoint point, float tolerance = 5)
    {
        var bounds = GetBounds();
        var inflatedBounds = SKRect.Inflate(bounds, tolerance, tolerance);
        return inflatedBounds.Contains(point);
    }

    public override Annotation Clone()
    {
        var clone = (BaseEffectAnnotation)base.Clone();
        // Don't copy the bitmap - it will be regenerated when needed
        // This avoids shared bitmap references and potential disposal issues
        clone.EffectBitmap = null;
        return clone;
    }

    /// <summary>
    /// Updates the effect bitmap based on the source image
    /// </summary>
    public virtual void UpdateEffect(SKBitmap source) { }

    internal virtual string GetInteractionCacheKey()
    {
        return $"{GetType().FullName}:{Amount.ToString("R", CultureInfo.InvariantCulture)}";
    }

    internal virtual SKBitmap? CreateInteractionCacheBitmap(SKBitmap source)
    {
        return null;
    }

    internal virtual void UpdateEffectFromInteractionCache(SKBitmap source, SKBitmap cachedEffectBitmap)
    {
        UpdateEffectFromAlignedCache(source, cachedEffectBitmap);
    }

    protected void UpdateEffectFromAlignedCache(SKBitmap source, SKBitmap cachedEffectBitmap)
    {
        if (source == null || cachedEffectBitmap == null) return;
        if (cachedEffectBitmap.Width != source.Width || cachedEffectBitmap.Height != source.Height) return;

        var rect = GetBounds();
        int fullW = (int)rect.Width;
        int fullH = (int)rect.Height;
        if (fullW <= 0 || fullH <= 0) return;

        var annotationRect = new SKRectI((int)rect.Left, (int)rect.Top, (int)rect.Right, (int)rect.Bottom);
        var validRect = annotationRect;
        validRect.Intersect(new SKRectI(0, 0, source.Width, source.Height));

        var result = new SKBitmap(fullW, fullH);
        result.Erase(SKColors.Transparent);

        if (validRect.Width <= 0 || validRect.Height <= 0)
        {
            EffectBitmap?.Dispose();
            EffectBitmap = result;
            return;
        }

        using var cachedRegion = new SKBitmap(validRect.Width, validRect.Height);
        if (!cachedEffectBitmap.ExtractSubset(cachedRegion, validRect))
        {
            EffectBitmap?.Dispose();
            EffectBitmap = result;
            return;
        }

        int drawX = validRect.Left - annotationRect.Left;
        int drawY = validRect.Top - annotationRect.Top;

        using (var resultCanvas = new SKCanvas(result))
        {
            resultCanvas.DrawBitmap(cachedRegion, drawX, drawY);
        }

        EffectBitmap?.Dispose();
        EffectBitmap = result;
    }

    /// <summary>
    /// Disposes the effect bitmap
    /// </summary>
    public void DisposeEffect()
    {
        EffectBitmap?.Dispose();
        EffectBitmap = null;
    }

    /// <summary>
    /// Dispose unmanaged resources (EffectBitmap)
    /// </summary>
    public void Dispose()
    {
        DisposeEffect();
        GC.SuppressFinalize(this);
    }
}
