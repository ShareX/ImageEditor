using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class InvertImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "invert";
    public override string Name => "Invert";
    public override string IconKey => LucideIcons.refresh_ccw_dot;
    public override string Description => "Inverts image colors.";
    public override EffectExecutionMode ExecutionMode => EffectExecutionMode.Immediate;

    public override SKBitmap Apply(SKBitmap source)
    {
        float[] matrix = {
            -1,  0,  0, 0, 1,
             0, -1,  0, 0, 1,
             0,  0, -1, 0, 1,
             0,  0,  0, 1, 0
        };
        return ApplyColorMatrix(source, matrix);
    }
}

