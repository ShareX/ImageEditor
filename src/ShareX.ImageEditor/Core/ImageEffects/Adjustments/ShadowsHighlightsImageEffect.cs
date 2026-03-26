using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class ShadowsHighlightsImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "shadows_highlights";
    public override string Name => "Shadows / Highlights";
    public override string IconKey => "Lightbulb";
    public override string Description => "Adjusts shadows and highlights.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<ShadowsHighlightsImageEffect>("shadows", "Shadows", -100, 100, 0, (effect, value) => effect.Shadows = value),
        EffectParameters.FloatSlider<ShadowsHighlightsImageEffect>("highlights", "Highlights", -100, 100, 0, (effect, value) => effect.Highlights = value)
    ];

    // Positive Shadows brightens dark areas; positive Highlights darkens bright areas.
    public float Shadows { get; set; } // -100..100
    public float Highlights { get; set; } // -100..100

    public override SKBitmap Apply(SKBitmap source)
    {
        float shadows = Math.Clamp(Shadows, -100f, 100f);
        float highlights = Math.Clamp(Highlights, -100f, 100f);

        if (Math.Abs(shadows) < 0.0001f && Math.Abs(highlights) < 0.0001f)
        {
            return source.Copy();
        }

        float sStrength = shadows / 100f;
        float hStrength = highlights / 100f;

        return ApplyPixelOperation(source, c =>
        {
            float luma = (0.2126f * c.Red + 0.7152f * c.Green + 0.0722f * c.Blue) / 255f;
            float shadowWeight = (1f - luma) * (1f - luma);
            float highlightWeight = luma * luma;

            float delta = (sStrength * shadowWeight - hStrength * highlightWeight) * 255f;

            return new SKColor(
                ClampToByte(c.Red + delta),
                ClampToByte(c.Green + delta),
                ClampToByte(c.Blue + delta),
                c.Alpha);
        });
    }

    private static byte ClampToByte(float value)
    {
        if (value <= 0f) return 0;
        if (value >= 255f) return 255;
        return (byte)MathF.Round(value);
    }
}
