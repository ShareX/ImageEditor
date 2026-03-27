using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public sealed class FlipHorizontalImageEffect : ImageEffectBase
{
    public override string Id => "flip_horizontal";
    public override string Name => "Flip horizontal";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => LucideIcons.flip_horizontal;
    public override string Description => "Flips the image horizontally.";
    public override EffectExecutionMode ExecutionMode => EffectExecutionMode.Immediate;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using SKCanvas canvas = new SKCanvas(result);
        canvas.Scale(-1, 1, source.Width / 2f, source.Height / 2f);
        canvas.DrawBitmap(source, 0, 0);
        return result;
    }
}
