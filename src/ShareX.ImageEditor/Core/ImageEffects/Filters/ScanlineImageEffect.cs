using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class ScanlineImageEffect : ImageEffectBase
{
    public override string Id => "scanline";
    public override string Name => "Scanlines";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.scan_line;
    public override string Description => "Adds retro CRT-style horizontal scanlines.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<ScanlineImageEffect>("spacing", "Line spacing", 2, 20, 4, (e, v) => e.Spacing = v),
        EffectParameters.FloatSlider<ScanlineImageEffect>("opacity", "Opacity", 0, 100, 50, (e, v) => e.Opacity = v)
    ];

    public int Spacing { get; set; } = 4;
    public float Opacity { get; set; } = 50f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        float alpha = Math.Clamp(Opacity / 100f, 0f, 1f);
        if (alpha <= 0f) return source.Copy();

        SKBitmap result = source.Copy();
        using SKCanvas canvas = new(result);
        using SKPaint paint = new()
        {
            Color = new SKColor(0, 0, 0, (byte)(255 * alpha)),
            StrokeWidth = 1,
            IsAntialias = false
        };

        int step = Math.Max(2, Spacing);
        for (int y = 0; y < source.Height; y += step)
        {
            canvas.DrawLine(0, y, source.Width, y, paint);
        }

        return result;
    }
}
