using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;
using ShareX.ImageEditor.Core.ImageEffects.Helpers;

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

        SKBitmap small = new SKBitmap(smallWidth, smallHeight);
        using (SKCanvas smallCanvas = new SKCanvas(small))
        {
            SkiaImageHelper.DrawBitmap(smallCanvas, source, new SKRect(0, 0, smallWidth, smallHeight), SkiaImageHelper.LinearSampling);
        }

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using (SKCanvas canvas = new SKCanvas(result))
        {
            using SKPaint paint = new SKPaint();
            SkiaImageHelper.DrawBitmap(canvas, small, new SKRect(0, 0, source.Width, source.Height), SkiaImageHelper.NearestNeighborSampling, paint);
        }
        small.Dispose();
        return result;
    }
}
