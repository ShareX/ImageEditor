using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class DreamGlowImageEffect : ImageEffectBase
{
    public override string Id => "dream_glow";
    public override string Name => "Dream glow";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.cloud;
    public override string Description => "Creates a dreamy soft glow by blending a blurred copy over the original.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<DreamGlowImageEffect>("radius", "Blur radius", 1, 50, 15, (e, v) => e.Radius = v),
        EffectParameters.FloatSlider<DreamGlowImageEffect>("opacity", "Blend opacity", 0, 100, 50, (e, v) => e.Opacity = v)
    ];

    public int Radius { get; set; } = 15;
    public float Opacity { get; set; } = 50f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        float alpha = Math.Clamp(Opacity / 100f, 0f, 1f);
        if (alpha <= 0f || Radius <= 0) return source.Copy();

        int w = source.Width, h = source.Height;

        // Create blurred version
        SKBitmap blurred = new(w, h, source.ColorType, source.AlphaType);
        using (SKCanvas blurCanvas = new(blurred))
        {
            using SKPaint blurPaint = new() { ImageFilter = SKImageFilter.CreateBlur(Radius, Radius) };
            blurCanvas.DrawBitmap(source, 0, 0, blurPaint);
        }

        // Blend: original + blurred with Screen mode
        SKBitmap result = source.Copy();
        using SKCanvas canvas = new(result);
        using SKPaint blendPaint = new()
        {
            Color = new SKColor(255, 255, 255, (byte)(255 * alpha)),
            BlendMode = SKBlendMode.Screen
        };
        canvas.DrawBitmap(blurred, 0, 0, blendPaint);
        blurred.Dispose();

        return result;
    }
}
