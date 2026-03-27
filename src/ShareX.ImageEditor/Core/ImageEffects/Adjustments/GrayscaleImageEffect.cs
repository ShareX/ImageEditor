using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class GrayscaleImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "grayscale";
    public override string Name => "Grayscale";
    public override string IconKey => LucideIcons.eclipse;
    public override string Description => "Converts the image to grayscale.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<GrayscaleImageEffect>("strength", "Strength", 0, 100, 100, (effect, value) => effect.Strength = value)
    ];

    public float Strength { get; set; } = 100f;

    public override SKBitmap Apply(SKBitmap source)
    {
        float strength = Strength;
        if (strength >= 100)
        {
            float[] matrix = {
                0.2126f, 0.7152f, 0.0722f, 0, 0,
                0.2126f, 0.7152f, 0.0722f, 0, 0,
                0.2126f, 0.7152f, 0.0722f, 0, 0,
                0,       0,       0,       1, 0
            };
            return ApplyColorMatrix(source, matrix);
        }
        else if (strength <= 0)
        {
            return source.Copy();
        }
        else
        {
            float s = strength / 100f;
            float invS = 1f - s;

            float[] matrix = {
                0.2126f * s + invS, 0.7152f * s,        0.0722f * s,        0, 0,
                0.2126f * s,        0.7152f * s + invS, 0.0722f * s,        0, 0,
                0.2126f * s,        0.7152f * s,        0.0722f * s + invS, 0, 0,
                0,                  0,                  0,                  1, 0
            };
            return ApplyColorMatrix(source, matrix);
        }
    }
}

