using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class EmbossImageEffect : ImageEffectBase
{
    public override string Id => "emboss";
    public override string Name => "Emboss";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => "Stamp";
    public override string Description => "Applies an emboss effect.";
    public override EffectExecutionMode ExecutionMode => EffectExecutionMode.Immediate;

    private static readonly float[] Kernel =
    {
        -1,  0, -1,
         0,  4,  0,
        -1,  0, -1
    };

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        return ConvolutionHelper.Apply3x3(source, Kernel, gain: 1f, bias: 127f);
    }
}
