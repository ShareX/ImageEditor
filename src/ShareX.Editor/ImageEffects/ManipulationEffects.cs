using SkiaSharp;
using ShareX.Editor.Helpers;

namespace ShareX.Editor.ImageEffects;

public class RoundedCornersImageEffect : ImageEffect
{
    public override string Name => "Rounded Corners";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override bool HasParameters => true;

    public int CornerRadius { get; set; } = 20;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (CornerRadius <= 0) return source.Copy();

        return ImageHelpers.RoundedCorners(source, CornerRadius);
    }
}

public class SkewImageEffect : ImageEffect
{
    public override string Name => "Skew";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override bool HasParameters => true;

    public int Horizontally { get; set; } = 0;
    public int Vertically { get; set; } = 0;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (Horizontally == 0 && Vertically == 0) return source.Copy();

        return ImageHelpers.AddSkew(source, Horizontally, Vertically);
    }
}
