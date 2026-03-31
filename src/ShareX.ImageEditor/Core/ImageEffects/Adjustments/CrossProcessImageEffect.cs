using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class CrossProcessImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "cross_process";
    public override string Name => "Cross process";
    public override string IconKey => LucideIcons.camera;
    public override string Description => "Simulates film cross-processing with shifted colors and high contrast.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<CrossProcessImageEffect>("strength", "Strength", 0, 100, 100, (e, v) => e.Strength = v)
    ];

    public float Strength { get; set; } = 100f;

    public override SKBitmap Apply(SKBitmap source)
    {
        float s = Math.Clamp(Strength / 100f, 0f, 1f);
        if (s <= 0f) return source.Copy();

        // Cross-process color matrix: boost greens/yellows, shift blues to cyan, warm shadows
        float[] crossMatrix =
        {
            1.2f,  0.1f, -0.1f, 0, 0.05f,
           -0.1f,  1.3f,  0.0f, 0, 0.02f,
           -0.1f,  0.0f,  0.8f, 0, 0.1f,
            0,     0,     0,    1, 0
        };

        float[] identity =
        {
            1, 0, 0, 0, 0,
            0, 1, 0, 0, 0,
            0, 0, 1, 0, 0,
            0, 0, 0, 1, 0
        };

        float[] matrix = new float[20];
        for (int i = 0; i < 20; i++)
            matrix[i] = identity[i] * (1f - s) + crossMatrix[i] * s;

        return ApplyColorMatrix(source, matrix);
    }
}
