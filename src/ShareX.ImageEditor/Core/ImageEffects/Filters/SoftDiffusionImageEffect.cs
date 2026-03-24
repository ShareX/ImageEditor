using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public class SoftDiffusionImageEffect : FilterImageEffect
{
    public override string Name => "Soft diffusion";
    public override bool HasParameters => true;

    public float Amount { get; set; } = 52f;
    public float Radius { get; set; } = 12f;
    public float HighlightBloom { get; set; } = 40f;
    public float ContrastSoftening { get; set; } = 28f;
    public float Warmth { get; set; } = 22f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        float amount = Math.Clamp(Amount, 0f, 100f) / 100f;
        float radius = Math.Clamp(Radius, 0f, 30f);
        float bloom = Math.Clamp(HighlightBloom, 0f, 100f) / 100f;
        float softness = Math.Clamp(ContrastSoftening, 0f, 100f) / 100f;
        float warmth = Math.Clamp(Warmth, 0f, 100f) / 100f;

        if (amount <= 0.0001f)
        {
            return source.Copy();
        }

        int width = source.Width;
        int height = source.Height;
        SKColor[] srcPixels = source.Pixels;
        using SKBitmap blurBitmap = AnalogEffectHelper.CreateBlurredClamp(source, Math.Max(0.1f, radius));
        SKColor[] blurPixels = blurBitmap.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];
        float postContrast = 1f - (softness * 0.28f);

        Parallel.For(0, height, y =>
        {
            int row = y * width;
            for (int x = 0; x < width; x++)
            {
                SKColor src = srcPixels[row + x];
                SKColor blur = blurPixels[row + x];

                float sr = src.Red / 255f;
                float sg = src.Green / 255f;
                float sb = src.Blue / 255f;
                float br = blur.Red / 255f;
                float bg = blur.Green / 255f;
                float bb = blur.Blue / 255f;
                float blurLum = AnalogEffectHelper.Luminance01(blur);

                float bloomMask = 0.45f + (bloom * MathF.Pow(blurLum, 1.7f) * 0.85f);
                float diffR = AnalogEffectHelper.Screen(sr, br * bloomMask * (1.02f + (warmth * 0.08f)));
                float diffG = AnalogEffectHelper.Screen(sg, bg * bloomMask * 0.96f);
                float diffB = AnalogEffectHelper.Screen(sb, bb * bloomMask * (0.92f - (warmth * 0.06f)));

                float r = ProceduralEffectHelper.Lerp(sr, diffR, amount);
                float g = ProceduralEffectHelper.Lerp(sg, diffG, amount);
                float b = ProceduralEffectHelper.Lerp(sb, diffB, amount);

                r = AnalogEffectHelper.ApplyContrast(r, postContrast);
                g = AnalogEffectHelper.ApplyContrast(g, postContrast);
                b = AnalogEffectHelper.ApplyContrast(b, postContrast);

                if (warmth > 0.0001f)
                {
                    float warmMask = warmth * MathF.Pow(blurLum, 1.35f) * 0.22f;
                    r = AnalogEffectHelper.Screen(r, warmMask);
                    g = AnalogEffectHelper.Screen(g, warmMask * 0.55f);
                    b = ProceduralEffectHelper.Lerp(b, MathF.Max(0f, b - (warmMask * 0.12f)), warmMask);
                }

                dstPixels[row + x] = new SKColor(
                    ProceduralEffectHelper.ClampToByte(r * 255f),
                    ProceduralEffectHelper.ClampToByte(g * 255f),
                    ProceduralEffectHelper.ClampToByte(b * 255f),
                    src.Alpha);
            }
        });

        return AnalogEffectHelper.CreateBitmap(source, dstPixels);
    }
}
