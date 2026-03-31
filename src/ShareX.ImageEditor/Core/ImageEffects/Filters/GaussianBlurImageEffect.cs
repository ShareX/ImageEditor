using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class GaussianBlurImageEffect : ImageEffectBase
{
    public override string Id => "gaussian_blur";
    public override string Name => "Gaussian blur";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.circle_gauge;
    public override string Description => "Applies a Gaussian blur effect.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<GaussianBlurImageEffect>("radius", "Radius", 1, 200, 15, (effect, value) => effect.Radius = value)
    ];

    public int Radius { get; set; } = 15;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int radius = Math.Max(1, Radius);
        float sigma = radius / 3f;

        int padding = radius * 2;
        int expandedWidth = source.Width + padding * 2;
        int expandedHeight = source.Height + padding * 2;

        SKBitmap expanded = new SKBitmap(expandedWidth, expandedHeight, source.ColorType, source.AlphaType);
        using (SKCanvas expandCanvas = new SKCanvas(expanded))
        {
            using SKShader shader = SKShader.CreateBitmap(
                source,
                SKShaderTileMode.Clamp,
                SKShaderTileMode.Clamp,
                SKMatrix.CreateTranslation(padding, padding));
            using SKPaint paint = new SKPaint { Shader = shader };
            expandCanvas.DrawRect(new SKRect(0, 0, expandedWidth, expandedHeight), paint);
        }

        SKBitmap blurred = new SKBitmap(expandedWidth, expandedHeight, source.ColorType, source.AlphaType);
        using (SKCanvas blurCanvas = new SKCanvas(blurred))
        {
            using SKPaint blurPaint = new SKPaint
            {
                ImageFilter = SKImageFilter.CreateBlur(sigma, sigma)
            };
            blurCanvas.DrawBitmap(expanded, 0, 0, blurPaint);
        }

        expanded.Dispose();

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using (SKCanvas resultCanvas = new SKCanvas(result))
        {
            resultCanvas.DrawBitmap(
                blurred,
                new SKRect(padding, padding, padding + source.Width, padding + source.Height),
                new SKRect(0, 0, source.Width, source.Height));
        }

        blurred.Dispose();
        return result;
    }
}
