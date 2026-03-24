using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public enum FoldCreaseOrientation
{
    Vertical,
    Horizontal
}

public class FoldCreaseWarpImageEffect : ImageEffect
{
    public override string Name => "Fold crease warp";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override bool HasParameters => true;

    public FoldCreaseOrientation Orientation { get; set; } = FoldCreaseOrientation.Vertical;
    public int FoldCount { get; set; } = 3;
    public float FoldDepth { get; set; } = 30f;
    public float ShadowStrength { get; set; } = 40f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int foldCount = Math.Clamp(FoldCount, 1, 10);
        float depth01 = Math.Clamp(FoldDepth, 0f, 100f) / 100f;
        if (depth01 <= 0f)
        {
            return source.Copy();
        }

        float shadow01 = Math.Clamp(ShadowStrength, 0f, 100f) / 100f;
        int width = source.Width;
        int height = source.Height;
        float span = (Orientation == FoldCreaseOrientation.Vertical ? Math.Max(1f, width - 1) : Math.Max(1f, height - 1)) / foldCount;
        float displacement = span * (0.06f + (depth01 * 0.22f));

        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];

        Parallel.For(0, height, y =>
        {
            int row = y * width;
            for (int x = 0; x < width; x++)
            {
                float primary = Orientation == FoldCreaseOrientation.Vertical ? x : y;
                float phase = (primary / Math.Max(1f, span)) * MathF.PI;
                float offset = MathF.Sin(phase) * displacement;

                float sampleX = Orientation == FoldCreaseOrientation.Vertical ? x + offset : x;
                float sampleY = Orientation == FoldCreaseOrientation.Vertical ? y : y + offset;

                SKColor sampled = DistortionEffectHelper.SampleClamped(srcPixels, width, height, sampleX, sampleY);

                float curvature = Math.Abs(MathF.Sin(phase));
                float slope = MathF.Cos(phase);
                float shade = 1f - (shadow01 * curvature * 0.22f) + (shadow01 * slope * 0.08f);

                dstPixels[row + x] = DistortionEffectHelper.MultiplyRgb(sampled, shade);
            }
        });

        return DistortionEffectHelper.CreateBitmap(source, width, height, dstPixels);
    }
}
