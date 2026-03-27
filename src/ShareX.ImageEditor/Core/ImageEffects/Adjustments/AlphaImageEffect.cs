using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;
using ShareX.ImageEditor.Presentation.Theming;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class AlphaImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "alpha";
    public override string Name => "Alpha";
    public override string IconKey => LucideIcons.droplet;
    public override string Description => "Adjusts the alpha transparency.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<AlphaImageEffect>("amount", "Alpha", 0, 100, 100, (effect, value) => effect.Amount = value)
    ];

    public float Amount { get; set; } = 100f; // 0 to 100

    public override SKBitmap Apply(SKBitmap source)
    {
        float a = Amount / 100f;
        float[] matrix = {
            1, 0, 0, 0, 0,
            0, 1, 0, 0, 0,
            0, 0, 1, 0, 0,
            0, 0, 0, a, 0
        };
        return ApplyColorMatrix(source, matrix);
    }
}

