using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class HueImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "hue";
    public override string Name => "Hue";
    public override string IconKey => LucideIcons.pipette;
    public override string Description => "Adjusts the hue of the image.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<HueImageEffect>("amount", "Amount", -180, 180, 0, (effect, value) => effect.Amount = value)
    ];

    public float Amount { get; set; } = 0; // -180 to 180

    public override SKBitmap Apply(SKBitmap source)
    {
        float radians = (float)(Amount * Math.PI / 180.0);
        float c = (float)Math.Cos(radians);
        float s = (float)Math.Sin(radians);

        float[] matrix = {
            0.213f + c * 0.787f - s * 0.213f, 0.715f - c * 0.715f - s * 0.715f, 0.072f - c * 0.072f + s * 0.928f, 0, 0,
            0.213f - c * 0.213f + s * 0.143f, 0.715f + c * 0.285f + s * 0.140f, 0.072f - c * 0.072f - s * 0.283f, 0, 0,
            0.213f - c * 0.213f - s * 0.787f, 0.715f - c * 0.715f + s * 0.715f, 0.072f + c * 0.928f + s * 0.072f, 0, 0,
            0, 0, 0, 1, 0
        };
        return ApplyColorMatrix(source, matrix);
    }
}

