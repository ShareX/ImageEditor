using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class ReplaceColorImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "replace_color";
    public override string Name => "Replace Color";
    public override string IconKey => "Replace";
    public override string Description => "Replaces a specific color.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.Color<ReplaceColorImageEffect>("target_color", "Target color", SKColors.White, (effect, value) => effect.TargetColor = value),
        EffectParameters.Color<ReplaceColorImageEffect>("replace_color", "Replace color", SKColors.Black, (effect, value) => effect.ReplaceColor = value),
        EffectParameters.FloatSlider<ReplaceColorImageEffect>("tolerance", "Tolerance", 0, 255, 40, (effect, value) => effect.Tolerance = value)
    ];
    public SKColor TargetColor { get; set; }
    public SKColor ReplaceColor { get; set; }
    public float Tolerance { get; set; }

    public override SKBitmap Apply(SKBitmap source)
    {
        int tol = (int)(Tolerance * 2.55f);
        return ApplyPixelOperation(source, (c) =>
        {
            if (ImageHelpers.ColorsMatch(c, TargetColor, tol))
            {
                return ReplaceColor;
            }
            return c;
        });
    }
}

