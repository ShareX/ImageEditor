using SkiaSharp;
using ShareX.ImageEditor.Presentation.Theming;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public sealed class Rotate90CWImageEffect : ImageEffectBase
{
    public override string Id => "rotate_90_clockwise";
    public override string Name => "Rotate 90\u00b0 clockwise";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => LucideIcons.rotate_cw;
    public override string Description => "Rotates the image 90 degrees clockwise.";
    public override EffectExecutionMode ExecutionMode => EffectExecutionMode.Immediate;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = new SKBitmap(source.Height, source.Width, source.ColorType, source.AlphaType);
        using SKCanvas canvas = new SKCanvas(result);
        canvas.Translate(result.Width, 0);
        canvas.RotateDegrees(90);
        canvas.DrawBitmap(source, 0, 0);
        return result;
    }
}
