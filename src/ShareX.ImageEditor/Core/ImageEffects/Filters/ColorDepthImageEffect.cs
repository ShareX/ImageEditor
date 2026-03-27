using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;
using ShareX.ImageEditor.Presentation.Theming;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class ColorDepthImageEffect : ImageEffectBase
{
    public override string Id => "color_depth";
    public override string Name => "Color depth";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.layers_2;
    public override string Description => "Reduces the number of bits per color channel.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<ColorDepthImageEffect>("bitsPerChannel", "Bits per channel", 1, 8, 4, (e, v) => e.BitsPerChannel = v),
    ];

    public int BitsPerChannel { get; set; } = 4;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int bits = Math.Clamp(BitsPerChannel, 1, 8);
        if (bits == 8)
        {
            return source.Copy();
        }

        double colorsPerChannel = Math.Pow(2, bits);
        double interval = 255d / (colorsPerChannel - 1d);

        static byte Remap(byte color, double remapInterval)
        {
            return (byte)Math.Round(Math.Round(color / remapInterval) * remapInterval);
        }

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];

        for (int i = 0; i < srcPixels.Length; i++)
        {
            SKColor src = srcPixels[i];
            dstPixels[i] = new SKColor(
                Remap(src.Red, interval),
                Remap(src.Green, interval),
                Remap(src.Blue, interval),
                src.Alpha);
        }

        result.Pixels = dstPixels;
        return result;
    }
}
