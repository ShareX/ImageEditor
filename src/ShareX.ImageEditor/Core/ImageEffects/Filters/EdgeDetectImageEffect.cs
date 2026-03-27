using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class EdgeDetectImageEffect : ImageEffectBase
{
    public override string Id => "edge_detect";
    public override string Name => "Edge detect";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.scan_search;
    public override string Description => "Detects visible edges in the image.";
    public override EffectExecutionMode ExecutionMode => EffectExecutionMode.Immediate;

    private static readonly float[] Kernel =
    {
        -1, -1, -1,
         0,  0,  0,
         1,  1,  1
    };

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        return ConvolutionHelper.Apply3x3(source, Kernel, gain: 1f, bias: 127f);
    }
}
