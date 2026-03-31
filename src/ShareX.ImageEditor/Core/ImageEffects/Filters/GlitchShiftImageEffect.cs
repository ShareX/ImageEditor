using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class GlitchShiftImageEffect : ImageEffectBase
{
    public override string Id => "glitch_shift";
    public override string Name => "Glitch shift";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.zap;
    public override string Description => "Randomly shifts horizontal bands of the image for a digital glitch look.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<GlitchShiftImageEffect>("intensity", "Intensity", 1, 100, 30, (e, v) => e.Intensity = v),
        EffectParameters.IntSlider<GlitchShiftImageEffect>("bands", "Band count", 2, 50, 12, (e, v) => e.Bands = v)
    ];

    public int Intensity { get; set; } = 30;
    public int Bands { get; set; } = 12;
    public int Seed { get; set; } = 42;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (Intensity <= 0) return source.Copy();

        int w = source.Width, h = source.Height;
        SKColor[] src = source.Pixels;
        SKColor[] dst = (SKColor[])src.Clone();
        Random rng = new(Seed);

        int maxShift = w * Intensity / 100;
        int bandCount = Math.Max(2, Bands);

        for (int b = 0; b < bandCount; b++)
        {
            int bandStart = rng.Next(h);
            int bandHeight = rng.Next(2, Math.Max(3, h / bandCount));
            int shift = rng.Next(-maxShift, maxShift + 1);

            for (int y = bandStart; y < Math.Min(bandStart + bandHeight, h); y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int sx = x - shift;
                    if (sx >= 0 && sx < w)
                        dst[y * w + x] = src[y * w + sx];
                    else
                        dst[y * w + x] = SKColors.Black;
                }
            }
        }

        return new SKBitmap(w, h, source.ColorType, source.AlphaType) { Pixels = dst };
    }
}
