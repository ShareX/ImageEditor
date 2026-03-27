using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class ColorSplashImageEffect : ImageEffectBase
{
    public override string Id => "color_splash";
    public override string Name => "Color splash";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.droplets;
    public override string Description => "Keeps one color and desaturates the rest of the image.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<ColorSplashImageEffect>("hue", "Target hue", 0, 360, 0, (e, v) => e.TargetHue = v),
        EffectParameters.IntSlider<ColorSplashImageEffect>("tolerance", "Tolerance", 1, 180, 30, (e, v) => e.Tolerance = v)
    ];

    public int TargetHue { get; set; } = 0;
    public int Tolerance { get; set; } = 30;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int w = source.Width, h = source.Height;
        SKColor[] src = source.Pixels;
        SKColor[] dst = new SKColor[src.Length];

        float targetH = TargetHue;
        float tol = Tolerance;

        for (int i = 0; i < src.Length; i++)
        {
            SKColor c = src[i];
            c.ToHsl(out float hue, out float sat, out float lum);

            float diff = MathF.Abs(hue - targetH);
            if (diff > 180f) diff = 360f - diff;

            if (diff <= tol && sat > 5f)
            {
                dst[i] = c; // Keep original color
            }
            else
            {
                // Desaturate
                byte gray = (byte)(0.2126f * c.Red + 0.7152f * c.Green + 0.0722f * c.Blue);
                dst[i] = new SKColor(gray, gray, gray, c.Alpha);
            }
        }

        return new SKBitmap(w, h, source.ColorType, source.AlphaType) { Pixels = dst };
    }
}
