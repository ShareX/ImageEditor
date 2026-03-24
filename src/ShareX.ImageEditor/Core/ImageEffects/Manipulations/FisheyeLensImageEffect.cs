using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public class FisheyeLensImageEffect : ImageEffect
{
    public override string Name => "Fisheye lens";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override bool HasParameters => true;

    public float Strength { get; set; } = 58f;
    public float RadiusPercentage { get; set; } = 100f;
    public float CenterXPercentage { get; set; } = 50f;
    public float CenterYPercentage { get; set; } = 50f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        float strength01 = Math.Clamp(Strength, 0f, 100f) / 100f;
        if (strength01 <= 0f)
        {
            return source.Copy();
        }

        int width = source.Width;
        int height = source.Height;
        float radius = Math.Max(1f, Math.Min(width, height) * Math.Clamp(RadiusPercentage, 1f, 100f) / 100f);
        float centerX = DistortionEffectHelper.PercentageToX(width, CenterXPercentage);
        float centerY = DistortionEffectHelper.PercentageToY(height, CenterYPercentage);
        float exponent = 1f + (strength01 * 2.4f);

        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];

        Parallel.For(0, height, y =>
        {
            int row = y * width;
            for (int x = 0; x < width; x++)
            {
                float dx = x - centerX;
                float dy = y - centerY;
                float distance = MathF.Sqrt((dx * dx) + (dy * dy));

                if (distance <= 0.0001f || distance >= radius)
                {
                    dstPixels[row + x] = srcPixels[row + x];
                    continue;
                }

                float normalized = distance / radius;
                float sampleDistance = MathF.Pow(normalized, exponent) * radius;
                float scale = sampleDistance / distance;

                float sampleX = centerX + (dx * scale);
                float sampleY = centerY + (dy * scale);

                dstPixels[row + x] = DistortionEffectHelper.SampleClamped(srcPixels, width, height, sampleX, sampleY);
            }
        });

        return DistortionEffectHelper.CreateBitmap(source, width, height, dstPixels);
    }
}
