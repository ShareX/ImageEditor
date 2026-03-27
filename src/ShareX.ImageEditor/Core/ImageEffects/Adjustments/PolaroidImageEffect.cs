using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class PolaroidImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "polaroid";
    public override string Name => "Polaroid";
    public override string IconKey => LucideIcons.camera;
    public override string Description => "Applies a Polaroid effect.";
    public override EffectExecutionMode ExecutionMode => EffectExecutionMode.Immediate;

    public override SKBitmap Apply(SKBitmap source)
    {
        float[] matrix = {
            1.438f, -0.062f, -0.062f, 0, 0,
            -0.122f, 1.378f, -0.122f, 0, 0,
            -0.016f, -0.016f, 1.483f, 0, 0,
            0,       0,       0,      1, 0
        };
        return ApplyColorMatrix(source, matrix);
    }
}

