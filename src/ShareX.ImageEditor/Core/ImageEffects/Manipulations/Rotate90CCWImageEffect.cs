using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public sealed class Rotate90CCWImageEffect : ImageEffectBase
{
    public override string Id => "rotate_90_counter_clockwise";
    public override string Name => "Rotate 90\u00b0 counter clockwise";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => "Undo2";
    public override string Description => "Rotates the image 90 degrees counter-clockwise.";
    public override EffectExecutionMode ExecutionMode => EffectExecutionMode.Immediate;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = new SKBitmap(source.Height, source.Width, source.ColorType, source.AlphaType);
        using SKCanvas canvas = new SKCanvas(result);
        canvas.Translate(0, result.Height);
        canvas.RotateDegrees(270);
        canvas.DrawBitmap(source, 0, 0);
        return result;
    }
}
