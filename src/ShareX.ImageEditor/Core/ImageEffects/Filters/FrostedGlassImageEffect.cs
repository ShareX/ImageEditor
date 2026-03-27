using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class FrostedGlassImageEffect : ImageEffectBase
{
    public override string Id => "frosted_glass";
    public override string Name => "Frosted glass";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.snowflake;
    public override string Description => "Simulates looking through frosted glass by randomizing nearby pixels.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<FrostedGlassImageEffect>("radius", "Radius", 1, 20, 5, (e, v) => e.Radius = v)
    ];

    public int Radius { get; set; } = 5;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        int radius = Math.Max(1, Radius);

        int w = source.Width, h = source.Height;
        SKColor[] src = source.Pixels;
        SKColor[] dst = new SKColor[src.Length];
        Random rng = new(42);

        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                int ox = x + rng.Next(-radius, radius + 1);
                int oy = y + rng.Next(-radius, radius + 1);
                ox = Math.Clamp(ox, 0, w - 1);
                oy = Math.Clamp(oy, 0, h - 1);
                dst[y * w + x] = src[oy * w + ox];
            }
        }

        return new SKBitmap(w, h, source.ColorType, source.AlphaType) { Pixels = dst };
    }
}
