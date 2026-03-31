using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class GhostTrailImageEffect : ImageEffectBase
{
    public override string Id => "ghost_trail";
    public override string Name => "Ghost trail";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.ghost;
    public override string Description => "Creates fading ghost copies offset from the original image.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<GhostTrailImageEffect>("copies", "Copies", 1, 8, 3, (e, v) => e.Copies = v),
        EffectParameters.IntSlider<GhostTrailImageEffect>("offset_x", "Offset X", -50, 50, 15, (e, v) => e.OffsetX = v),
        EffectParameters.IntSlider<GhostTrailImageEffect>("offset_y", "Offset Y", -50, 50, 5, (e, v) => e.OffsetY = v),
        EffectParameters.FloatSlider<GhostTrailImageEffect>("fade", "Fade", 10, 90, 50, (e, v) => e.Fade = v)
    ];

    public int Copies { get; set; } = 3;
    public int OffsetX { get; set; } = 15;
    public int OffsetY { get; set; } = 5;
    public float Fade { get; set; } = 50f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        int copies = Math.Max(1, Copies);

        int w = source.Width, h = source.Height;
        SKBitmap result = new(w, h, source.ColorType, source.AlphaType);
        using SKCanvas canvas = new(result);
        canvas.Clear(SKColors.Transparent);

        float fadeStep = Math.Clamp(Fade / 100f, 0.1f, 0.9f);

        // Draw ghost copies back-to-front (farthest first)
        for (int i = copies; i >= 1; i--)
        {
            float alpha = MathF.Pow(1f - fadeStep, i);
            using SKPaint paint = new() { Color = new SKColor(255, 255, 255, (byte)(255 * alpha)) };
            canvas.DrawBitmap(source, OffsetX * i, OffsetY * i, paint);
        }

        // Draw original on top
        canvas.DrawBitmap(source, 0, 0);
        return result;
    }
}
