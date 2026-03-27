using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;
using ShareX.ImageEditor.Presentation.Theming;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class MeanRemovalImageEffect : ImageEffectBase
{
    public override string Id => "mean_removal";
    public override string Name => "Mean removal";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.sigma;
    public override string Description => "Removes the mean value from colors.";
    public override EffectExecutionMode ExecutionMode => EffectExecutionMode.Immediate;

    private static readonly float[] Kernel =
    {
        -1, -1, -1,
        -1,  9, -1,
        -1, -1, -1
    };

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        return ConvolutionHelper.Apply3x3(source, Kernel);
    }
}
