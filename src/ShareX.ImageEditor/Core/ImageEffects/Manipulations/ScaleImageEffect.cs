using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public sealed class ScaleImageEffect : ImageEffectBase
{
    public override string Id => "scale";
    public override string Name => "Scale";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => LucideIcons.scaling;
    public override string Description => "Scales the image by width and height percentages.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<ScaleImageEffect>("width_percentage", "Width %", 0f, 500f, 100f, (e, v) => e.WidthPercentage = v),
        EffectParameters.FloatSlider<ScaleImageEffect>("height_percentage", "Height %", 0f, 500f, 0f, (e, v) => e.HeightPercentage = v)
    ];

    public float WidthPercentage { get; set; } = 100f;
    public float HeightPercentage { get; set; } = 0f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        if (WidthPercentage <= 0f && HeightPercentage <= 0f)
        {
            return source.Copy();
        }

        int width = (int)Math.Round(WidthPercentage / 100f * source.Width);
        int height = (int)Math.Round(HeightPercentage / 100f * source.Height);

        if (width == 0)
        {
            width = (int)Math.Round((float)height / source.Height * source.Width);
        }
        else if (height == 0)
        {
            height = (int)Math.Round((float)width / source.Width * source.Height);
        }

        if (width <= 0 || height <= 0)
        {
            return source.Copy();
        }

        SKImageInfo info = new SKImageInfo(width, height, source.ColorType, source.AlphaType, source.ColorSpace);
        return source.Resize(info, SKFilterQuality.High) ?? source.Copy();
    }
}
