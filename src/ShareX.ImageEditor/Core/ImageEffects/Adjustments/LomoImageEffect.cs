using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class LomoImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "lomo";
    public override string Name => "Lomo";
    public override string IconKey => LucideIcons.aperture;
    public override string Description => "Applies a lomography look with high contrast, warm tones, and vignette.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<LomoImageEffect>("strength", "Strength", 0, 100, 100, (e, v) => e.Strength = v)
    ];

    public float Strength { get; set; } = 100f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        float s = Math.Clamp(Strength / 100f, 0f, 1f);
        if (s <= 0f) return source.Copy();

        int w = source.Width, h = source.Height;

        // Step 1: High contrast + warm color shift
        float[] lomoMatrix =
        {
            1.3f,  0.05f, -0.05f, 0, 0.02f,
           -0.02f, 1.1f,  -0.02f, 0, 0.01f,
           -0.05f, 0.0f,   0.9f,  0, -0.02f,
            0,     0,      0,     1, 0
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
            matrix[i] = identity[i] * (1f - s) + lomoMatrix[i] * s;

        SKBitmap colored = ApplyColorMatrix(source, matrix);

        // Step 2: Vignette
        using SKCanvas canvas = new(colored);
        float cx = w / 2f, cy = h / 2f;
        float radius = MathF.Sqrt(w * w + h * h) * 0.5f;

        using SKPaint vigPaint = new()
        {
            Shader = SKShader.CreateRadialGradient(
                new SKPoint(cx, cy), radius,
                [SKColors.Transparent, new SKColor(0, 0, 0, (byte)(180 * s))],
                [0.5f, 1f],
                SKShaderTileMode.Clamp)
        };
        canvas.DrawRect(0, 0, w, h, vigPaint);

        return colored;
    }
}
