using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class RetroSunsetImageEffect : ImageEffectBase
{
    public override string Id => "retro_sunset";
    public override string Name => "Retro sunset";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.sunset;
    public override string Description => "Overlays retro 80s-style sunset gradient bands.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<RetroSunsetImageEffect>("strength", "Strength", 0, 100, 50, (e, v) => e.Strength = v),
        EffectParameters.Bool<RetroSunsetImageEffect>("horizontal_lines", "Horizontal lines", true, (e, v) => e.HorizontalLines = v)
    ];

    public float Strength { get; set; } = 50f;
    public bool HorizontalLines { get; set; } = true;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        float alpha = Math.Clamp(Strength / 100f, 0f, 1f);
        if (alpha <= 0f) return source.Copy();

        int w = source.Width, h = source.Height;
        SKBitmap result = source.Copy();
        using SKCanvas canvas = new(result);

        // Retro sunset gradient: magenta -> orange -> yellow
        SKColor[] colors =
        [
            new SKColor(120, 0, 180),   // purple
            new SKColor(220, 20, 80),   // magenta-red
            new SKColor(255, 100, 0),   // orange
            new SKColor(255, 200, 0),   // yellow
        ];

        float[] positions = [0f, 0.33f, 0.66f, 1f];

        byte gradientAlpha = (byte)(200 * alpha);
        SKColor[] alphaColors = new SKColor[colors.Length];
        for (int i = 0; i < colors.Length; i++)
            alphaColors[i] = colors[i].WithAlpha(gradientAlpha);

        using SKPaint gradPaint = new()
        {
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0), new SKPoint(0, h),
                alphaColors, positions,
                SKShaderTileMode.Clamp),
            BlendMode = SKBlendMode.Overlay
        };
        canvas.DrawRect(0, 0, w, h, gradPaint);

        // Optional horizontal scan lines for retro effect
        if (HorizontalLines)
        {
            using SKPaint linePaint = new()
            {
                Color = new SKColor(0, 0, 0, (byte)(60 * alpha)),
                StrokeWidth = 1,
                IsAntialias = false
            };

            int lineSpacing = Math.Max(3, h / 80);
            for (int y = 0; y < h; y += lineSpacing)
            {
                canvas.DrawLine(0, y, w, y, linePaint);
            }
        }

        return result;
    }
}
