using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class EdgeFeatherImageEffect : ImageEffectBase
{
    private const byte MaskThreshold = 16;

    public override string Id => "edge_feather";
    public override string Name => "Edge feather";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => "IconBlend";
    public override string Description => "Softens the edges of non-transparent regions with a feathered alpha falloff.";

    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<EdgeFeatherImageEffect>("radius", "Radius", 0, 100, 12, (e, v) => e.Radius = v)
    ];

    public float Radius { get; set; } = 12f; // 0..100

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int width = source.Width;
        int height = source.Height;
        if (width <= 0 || height <= 0)
        {
            return source.Copy();
        }

        float radius = Math.Clamp(Radius, 0f, 100f);
        if (radius <= 0.01f)
        {
            return source.Copy();
        }

        SKColor[] sourcePixels = source.Pixels;
        SKColor[] maskPixels = new SKColor[sourcePixels.Length];

        for (int i = 0; i < sourcePixels.Length; i++)
        {
            byte alpha = sourcePixels[i].Alpha > MaskThreshold ? (byte)255 : (byte)0;
            maskPixels[i] = new SKColor(alpha, alpha, alpha, alpha);
        }

        using SKBitmap maskBitmap = new SKBitmap(width, height, source.ColorType, source.AlphaType)
        {
            Pixels = maskPixels
        };
        using SKBitmap featheredMask = ApplyTransparentBlur(maskBitmap, radius);

        SKColor[] featheredPixels = featheredMask.Pixels;
        SKColor[] outputPixels = new SKColor[sourcePixels.Length];

        for (int i = 0; i < sourcePixels.Length; i++)
        {
            SKColor sourceColor = sourcePixels[i];
            float feather = featheredPixels[i].Alpha / 255f;
            byte alpha = ProceduralEffectHelper.ClampToByte(sourceColor.Alpha * feather);

            outputPixels[i] = new SKColor(
                sourceColor.Red,
                sourceColor.Green,
                sourceColor.Blue,
                alpha);
        }

        return new SKBitmap(width, height, source.ColorType, source.AlphaType)
        {
            Pixels = outputPixels
        };
    }

    private static SKBitmap ApplyTransparentBlur(SKBitmap source, float radius)
    {
        if (radius <= 0.01f)
        {
            return source.Copy();
        }

        int padding = Math.Max(2, (int)MathF.Ceiling(radius * 2f));
        int expandedWidth = source.Width + (padding * 2);
        int expandedHeight = source.Height + (padding * 2);

        using SKBitmap expanded = new(expandedWidth, expandedHeight, source.ColorType, source.AlphaType);
        using (SKCanvas expandCanvas = new(expanded))
        {
            expandCanvas.Clear(SKColors.Transparent);
            expandCanvas.DrawBitmap(source, padding, padding);
        }

        float sigma = Math.Max(0.001f, radius / 3f);

        using SKBitmap blurredExpanded = new(expandedWidth, expandedHeight, source.ColorType, source.AlphaType);
        using (SKCanvas blurCanvas = new(blurredExpanded))
        using (SKPaint blurPaint = new() { ImageFilter = SKImageFilter.CreateBlur(sigma, sigma) })
        {
            blurCanvas.DrawBitmap(expanded, 0, 0, blurPaint);
        }

        SKBitmap result = new(source.Width, source.Height, source.ColorType, source.AlphaType);
        using (SKCanvas resultCanvas = new(result))
        {
            resultCanvas.Clear(SKColors.Transparent);
            resultCanvas.DrawBitmap(
                blurredExpanded,
                new SKRect(padding, padding, padding + source.Width, padding + source.Height),
                new SKRect(0, 0, source.Width, source.Height));
        }

        return result;
    }
}
