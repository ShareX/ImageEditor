using SkiaSharp;
using ShareX.ImageEditor.Presentation.Theming;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class BlackAndWhiteImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "black_and_white";
    public override string Name => "Black & White";
    public override string IconKey => LucideIcons.contrast;
    public override string Description => "Converts the image to black and white.";
    public override EffectExecutionMode ExecutionMode => EffectExecutionMode.Immediate;

    public override SKBitmap Apply(SKBitmap source)
    {
        return ApplyPixelOperation(source, (color) =>
        {
            float lum = 0.2126f * color.Red + 0.7152f * color.Green + 0.0722f * color.Blue;
            return lum > 127 ? SKColors.White : SKColors.Black;
        });
    }
}

