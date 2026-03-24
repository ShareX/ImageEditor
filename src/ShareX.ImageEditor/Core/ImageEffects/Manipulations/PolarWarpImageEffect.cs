using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public enum PolarWarpMode
{
    CartesianToPolar,
    PolarToCartesian
}

public class PolarWarpImageEffect : ImageEffect
{
    public override string Name => "Polar warp";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override bool HasParameters => true;

    public PolarWarpMode Mode { get; set; } = PolarWarpMode.CartesianToPolar;
    public float Rotation { get; set; } = 0f;
    public float RadiusScale { get; set; } = 100f;
    public float CenterXPercentage { get; set; } = 50f;
    public float CenterYPercentage { get; set; } = 50f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int width = source.Width;
        int height = source.Height;
        float radiusScale = Math.Clamp(RadiusScale, 20f, 200f) / 100f;
        float rotation = Rotation * (MathF.PI / 180f);
        float centerX = DistortionEffectHelper.PercentageToX(width, CenterXPercentage);
        float centerY = DistortionEffectHelper.PercentageToY(height, CenterYPercentage);
        float maxRadius = (Math.Min(width - 1, height - 1) * 0.5f) * radiusScale;

        if (maxRadius <= 0.5f)
        {
            return source.Copy();
        }

        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];
        float widthRange = Math.Max(1f, width - 1);
        float heightRange = Math.Max(1f, height - 1);

        Parallel.For(0, height, y =>
        {
            int row = y * width;
            for (int x = 0; x < width; x++)
            {
                if (Mode == PolarWarpMode.CartesianToPolar)
                {
                    float polarAngle = ((x / widthRange) * MathF.PI * 2f) + rotation;
                    float radius = (y / heightRange) * maxRadius;
                    float sampleX = centerX + (MathF.Cos(polarAngle) * radius);
                    float sampleY = centerY + (MathF.Sin(polarAngle) * radius);
                    dstPixels[row + x] = DistortionEffectHelper.SampleTransparent(srcPixels, width, height, sampleX, sampleY);
                    continue;
                }

                float dx = x - centerX;
                float dy = y - centerY;
                float distance = MathF.Sqrt((dx * dx) + (dy * dy));

                if (distance > maxRadius)
                {
                    dstPixels[row + x] = SKColors.Transparent;
                    continue;
                }

                float angle = DistortionEffectHelper.WrapAngle(MathF.Atan2(dy, dx) - rotation);
                float sampleXPolar = (angle / (MathF.PI * 2f)) * widthRange;
                float sampleYPolar = (distance / maxRadius) * heightRange;

                dstPixels[row + x] = DistortionEffectHelper.SampleClamped(srcPixels, width, height, sampleXPolar, sampleYPolar);
            }
        });

        return DistortionEffectHelper.CreateBitmap(source, width, height, dstPixels);
    }
}
