using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public enum CylinderWrapOrientation
{
    Vertical,
    Horizontal
}

public class CylinderWrapImageEffect : ImageEffect
{
    public override string Name => "Cylinder wrap";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override bool HasParameters => true;

    public CylinderWrapOrientation Orientation { get; set; } = CylinderWrapOrientation.Vertical;
    public float Curvature { get; set; } = 65f;
    public float EdgeShading { get; set; } = 35f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        float curvature01 = Math.Clamp(Curvature, 0f, 100f) / 100f;
        if (curvature01 <= 0f)
        {
            return source.Copy();
        }

        float shading01 = Math.Clamp(EdgeShading, 0f, 100f) / 100f;
        float maxAngle = 0.12f + (curvature01 * 1.28f);
        float sinMax = MathF.Sin(maxAngle);

        int width = source.Width;
        int height = source.Height;
        float widthRange = Math.Max(1f, width - 1);
        float heightRange = Math.Max(1f, height - 1);
        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];

        Parallel.For(0, height, y =>
        {
            int row = y * width;
            for (int x = 0; x < width; x++)
            {
                float sampleX;
                float sampleY;
                float shade;

                if (Orientation == CylinderWrapOrientation.Vertical)
                {
                    float projected = ((x / widthRange) * 2f) - 1f;
                    float angle = MathF.Asin(Math.Clamp(projected * sinMax, -1f, 1f));
                    float u = ((angle / maxAngle) + 1f) * 0.5f;
                    sampleX = u * widthRange;
                    sampleY = y;
                    shade = 1f - (shading01 * (1f - MathF.Cos(angle)) * 0.65f);
                }
                else
                {
                    float projected = ((y / heightRange) * 2f) - 1f;
                    float angle = MathF.Asin(Math.Clamp(projected * sinMax, -1f, 1f));
                    float v = ((angle / maxAngle) + 1f) * 0.5f;
                    sampleX = x;
                    sampleY = v * heightRange;
                    shade = 1f - (shading01 * (1f - MathF.Cos(angle)) * 0.65f);
                }

                SKColor sampled = DistortionEffectHelper.SampleClamped(srcPixels, width, height, sampleX, sampleY);
                dstPixels[row + x] = DistortionEffectHelper.MultiplyRgb(sampled, shade);
            }
        });

        return DistortionEffectHelper.CreateBitmap(source, width, height, dstPixels);
    }
}
