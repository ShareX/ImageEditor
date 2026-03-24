using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public class RippleRefractionImageEffect : ImageEffect
{
    public override string Name => "Ripple refraction";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override bool HasParameters => true;

    public float Amplitude { get; set; } = 12f;
    public float Wavelength { get; set; } = 32f;
    public float Phase { get; set; } = 0f;
    public float Refraction { get; set; } = 35f;
    public float CenterXPercentage { get; set; } = 50f;
    public float CenterYPercentage { get; set; } = 50f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        float amplitude = Math.Clamp(Amplitude, 0f, 80f);
        if (amplitude <= 0.01f)
        {
            return source.Copy();
        }

        float wavelength = Math.Clamp(Wavelength, 4f, 400f);
        float phaseRadians = Phase * (MathF.PI / 180f);
        float refraction01 = Math.Clamp(Refraction, 0f, 100f) / 100f;
        float centerX = DistortionEffectHelper.PercentageToX(source.Width, CenterXPercentage);
        float centerY = DistortionEffectHelper.PercentageToY(source.Height, CenterYPercentage);
        float waveScale = (MathF.PI * 2f) / wavelength;

        int width = source.Width;
        int height = source.Height;
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

                if (distance <= 0.0001f)
                {
                    dstPixels[row + x] = srcPixels[row + x];
                    continue;
                }

                float wavePhase = (distance * waveScale) + phaseRadians;
                float radialOffset = MathF.Sin(wavePhase) * amplitude;
                float refractedOffset = MathF.Cos(wavePhase) * amplitude * refraction01 * 0.35f;
                float sampleDistance = distance + radialOffset + refractedOffset;
                float scale = sampleDistance / distance;

                float sampleX = centerX + (dx * scale);
                float sampleY = centerY + (dy * scale);

                dstPixels[row + x] = DistortionEffectHelper.SampleClamped(srcPixels, width, height, sampleX, sampleY);
            }
        });

        return DistortionEffectHelper.CreateBitmap(source, width, height, dstPixels);
    }
}
