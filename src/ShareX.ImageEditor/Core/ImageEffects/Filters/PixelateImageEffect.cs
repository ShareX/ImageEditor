using ShareX.ImageEditor.Helpers;
using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class PixelateImageEffect : ImageEffectBase
{
    public override string Id => "pixelate";
    public override string Name => "Pixelate";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.grid_2x2;
    public override string Description => "Pixelates the image.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<PixelateImageEffect>("size", "Size", 1, 200, 10, (effect, value) => effect.Size = value)
    ];

    public int Size { get; set; } = 10;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (Size <= 1) return source.Copy();

        // Downscale then upscale to create pixelation effect
        int smallWidth = Math.Max(1, source.Width / Size);
        int smallHeight = Math.Max(1, source.Height / Size);
        SKImageInfo downscaleInfo = new SKImageInfo(smallWidth, smallHeight, source.ColorType, source.AlphaType, source.ColorSpace);
        using SKBitmap? small = source.Resize(downscaleInfo, SkiaCompat.LowQualitySampling);
        if (small == null)
        {
            return source.Copy();
        }

        SKImageInfo upscaleInfo = new SKImageInfo(source.Width, source.Height, source.ColorType, source.AlphaType, source.ColorSpace);
        return small.Resize(upscaleInfo, SkiaCompat.NearestNeighborSampling) ?? source.Copy();
    }
}
