using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;
using ShareX.ImageEditor.Presentation.Theming;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class BrightnessImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "brightness";
    public override string Name => "Brightness";
    public override string IconKey => LucideIcons.sun_medium;
    public override string Description => "Adjusts image brightness.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<BrightnessImageEffect>("amount", "Amount", -100, 100, 0, (effect, value) => effect.Amount = value)
    ];

    public float Amount { get; set; } = 0; // -100 to 100

    public override SKBitmap Apply(SKBitmap source)
    {
        float value = Amount / 100f;
        float[] matrix = {
            1, 0, 0, 0, value,
            0, 1, 0, 0, value,
            0, 0, 1, 0, value,
            0, 0, 0, 1, 0
        };
        return ApplyColorMatrix(source, matrix);
    }
}

