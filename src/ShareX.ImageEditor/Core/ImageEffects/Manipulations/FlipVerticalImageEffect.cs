using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public sealed class FlipVerticalImageEffect : ImageEffectBase
{
    public override string Id => "flip_vertical";
    public override string Name => "Flip vertical";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => "FlipVertical";
    public override string Description => "Flips the image vertically.";
    public override EffectExecutionMode ExecutionMode => EffectExecutionMode.Immediate;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using SKCanvas canvas = new SKCanvas(result);
        canvas.Scale(1, -1, source.Width / 2f, source.Height / 2f);
        canvas.DrawBitmap(source, 0, 0);
        return result;
    }
}
