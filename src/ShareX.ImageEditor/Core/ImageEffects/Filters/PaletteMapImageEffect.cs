using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class PaletteMapImageEffect : ImageEffectBase
{
    public override string Id => "palette_map";
    public override string Name => "Palette map";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.swatch_book;
    public override string Description => "Reduces the image to a limited color palette by quantization.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<PaletteMapImageEffect>("levels", "Color levels", 2, 32, 6, (e, v) => e.Levels = v)
    ];

    public int Levels { get; set; } = 6;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int levels = Math.Clamp(Levels, 2, 32);
        int w = source.Width, h = source.Height;
        SKColor[] src = source.Pixels;
        SKColor[] dst = new SKColor[src.Length];

        float step = 255f / (levels - 1);

        for (int i = 0; i < src.Length; i++)
        {
            SKColor c = src[i];
            byte r = Quantize(c.Red, step, levels);
            byte g = Quantize(c.Green, step, levels);
            byte b = Quantize(c.Blue, step, levels);
            dst[i] = new SKColor(r, g, b, c.Alpha);
        }

        return new SKBitmap(w, h, source.ColorType, source.AlphaType) { Pixels = dst };
    }

    private static byte Quantize(byte value, float step, int levels)
    {
        int index = (int)MathF.Round(value / step);
        index = Math.Clamp(index, 0, levels - 1);
        return (byte)Math.Clamp((int)(index * step), 0, 255);
    }
}
