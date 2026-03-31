using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class SmoothImageEffect : ImageEffectBase
{
    public override string Id => "smooth";
    public override string Name => "Smooth";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.waves;
    public override string Description => "Applies a smoothing effect.";
    public override EffectExecutionMode ExecutionMode => EffectExecutionMode.Immediate;

    private static readonly float[] Kernel =
    {
        1f / 9f, 1f / 9f, 1f / 9f,
        1f / 9f, 1f / 9f, 1f / 9f,
        1f / 9f, 1f / 9f, 1f / 9f
    };

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        return ConvolutionHelper.Apply3x3(source, Kernel);
    }
}
