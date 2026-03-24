using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public class LiquifyPushSmudgeImageEffect : ImageEffect
{
    public override string Name => "Liquify push / smudge";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override bool HasParameters => true;

    public float Angle { get; set; } = 0f;
    public float Distance { get; set; } = 40f;
    public float RadiusPercentage { get; set; } = 25f;
    public float Smudge { get; set; } = 35f;
    public float CenterXPercentage { get; set; } = 50f;
    public float CenterYPercentage { get; set; } = 50f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        float distance = Math.Clamp(Distance, -200f, 200f);
        if (Math.Abs(distance) <= 0.01f)
        {
            return source.Copy();
        }

        int width = source.Width;
        int height = source.Height;
        float radius = Math.Max(1f, Math.Min(width, height) * Math.Clamp(RadiusPercentage, 1f, 100f) / 100f);
        float smudge01 = Math.Clamp(Smudge, 0f, 100f) / 100f;
        int samples = 1 + (int)MathF.Round(smudge01 * 5f);
        float angle = Angle * (MathF.PI / 180f);
        float directionX = MathF.Cos(angle);
        float directionY = MathF.Sin(angle);
        float centerX = DistortionEffectHelper.PercentageToX(width, CenterXPercentage);
        float centerY = DistortionEffectHelper.PercentageToY(height, CenterYPercentage);

        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];

        Parallel.For(0, height, y =>
        {
            int row = y * width;
            for (int x = 0; x < width; x++)
            {
                float dx = x - centerX;
                float dy = y - centerY;
                float pointDistance = MathF.Sqrt((dx * dx) + (dy * dy));

                if (pointDistance >= radius)
                {
                    dstPixels[row + x] = srcPixels[row + x];
                    continue;
                }

                float normalized = pointDistance / radius;
                float influence = 1f - ProceduralEffectHelper.SmoothStep(0f, 1f, normalized);
                influence *= influence;

                float push = distance * influence;
                if (samples <= 1)
                {
                    dstPixels[row + x] = DistortionEffectHelper.SampleClamped(
                        srcPixels,
                        width,
                        height,
                        x - (directionX * push),
                        y - (directionY * push));
                    continue;
                }

                float sumR = 0f;
                float sumG = 0f;
                float sumB = 0f;
                float sumA = 0f;

                for (int i = 0; i < samples; i++)
                {
                    float t = samples == 1 ? 0f : i / (float)(samples - 1);
                    float offset = push * (1f - (t * smudge01));
                    SKColor sample = DistortionEffectHelper.SampleClamped(
                        srcPixels,
                        width,
                        height,
                        x - (directionX * offset),
                        y - (directionY * offset));

                    sumR += sample.Red;
                    sumG += sample.Green;
                    sumB += sample.Blue;
                    sumA += sample.Alpha;
                }

                float inv = 1f / samples;
                dstPixels[row + x] = new SKColor(
                    ProceduralEffectHelper.ClampToByte(sumR * inv),
                    ProceduralEffectHelper.ClampToByte(sumG * inv),
                    ProceduralEffectHelper.ClampToByte(sumB * inv),
                    ProceduralEffectHelper.ClampToByte(sumA * inv));
            }
        });

        return DistortionEffectHelper.CreateBitmap(source, width, height, dstPixels);
    }
}
