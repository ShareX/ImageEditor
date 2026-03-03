using ShareX.ImageEditor.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.ImageEffects.Filters;

public class RainReflectionPuddlesImageEffect : ImageEffect
{
    public override string Name => "Rain reflection puddles";
    public override string IconKey => "IconWeatherRain";
    public override bool HasParameters => true;

    public float PuddleAmount { get; set; } = 55f; // 0..100
    public float ReflectionStrength { get; set; } = 65f; // 0..100
    public float RippleStrength { get; set; } = 30f; // 0..100
    public float Wetness { get; set; } = 35f; // 0..100
    public float Horizon { get; set; } = 58f; // 20..95

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        float puddleAmount = Math.Clamp(PuddleAmount, 0f, 100f) / 100f;
        float reflectionStrength = Math.Clamp(ReflectionStrength, 0f, 100f) / 100f;
        float rippleStrength = Math.Clamp(RippleStrength, 0f, 100f) / 100f;
        float wetness = Math.Clamp(Wetness, 0f, 100f) / 100f;

        if (puddleAmount <= 0.0001f && reflectionStrength <= 0.0001f && wetness <= 0.0001f)
        {
            return source.Copy();
        }

        int width = source.Width;
        int height = source.Height;
        int seed = Random.Shared.Next(1, int.MaxValue);
        int bottom = Math.Max(1, height - 1);
        float horizonPx = (Math.Clamp(Horizon, 20f, 95f) / 100f) * bottom;
        float invGroundSpan = 1f / Math.Max(1f, bottom - horizonPx);

        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];

        Parallel.For(0, height, y =>
        {
            int row = y * width;
            bool isGround = y >= horizonPx;
            float groundT = isGround ? (y - horizonPx) * invGroundSpan : 0f;

            for (int x = 0; x < width; x++)
            {
                SKColor src = srcPixels[row + x];
                float r = src.Red;
                float g = src.Green;
                float b = src.Blue;
                float a = src.Alpha;

                if (isGround)
                {
                    float nA = ProceduralEffectHelper.Hash01((int)(x * 0.055f), (int)(y * 0.09f), seed);
                    float nB = ProceduralEffectHelper.Hash01((int)(x * 0.022f), (int)(y * 0.036f), seed ^ 1203);
                    float puddleNoise = (nA * 0.67f) + (nB * 0.33f);
                    float puddleMask = ProceduralEffectHelper.SmoothStep(1f - puddleAmount, 1f, puddleNoise);
                    puddleMask *= 0.60f + ((1f - groundT) * 0.40f);

                    if (puddleMask > 0.0001f && reflectionStrength > 0.0001f)
                    {
                        float waveA = MathF.Sin((x * 0.16f) + (y * 0.11f) + (ProceduralEffectHelper.Hash01(x / 7, y / 7, seed ^ 371) * MathF.PI * 2f));
                        float waveB = MathF.Cos((x * 0.08f) - (y * 0.14f) + (ProceduralEffectHelper.Hash01(x / 13, y / 11, seed ^ 761) * MathF.PI * 2f));
                        float ripple = rippleStrength * puddleMask;
                        float offsetX = ((waveA * 0.7f) + (waveB * 0.45f)) * ripple * 3.2f;
                        float offsetY = ((waveB * 0.8f) - (waveA * 0.35f)) * ripple * 2.2f;

                        float reflectY = horizonPx - (y - horizonPx) + offsetY;
                        float reflectX = x + offsetX;
                        SKColor reflection = ProceduralEffectHelper.BilinearSample(srcPixels, width, height, reflectX, reflectY);

                        float rr = (reflection.Red * 0.76f) + 6f;
                        float rg = (reflection.Green * 0.82f) + 8f;
                        float rb = (reflection.Blue * 0.93f) + 12f;
                        float blend = puddleMask * reflectionStrength * (0.88f - (groundT * 0.24f));

                        r = ProceduralEffectHelper.Lerp(r, rr, blend);
                        g = ProceduralEffectHelper.Lerp(g, rg, blend);
                        b = ProceduralEffectHelper.Lerp(b, rb, blend);
                    }

                    if (wetness > 0.0001f)
                    {
                        float wetMask = wetness * (0.40f + (0.60f * puddleMask));
                        r *= 1f - (wetMask * 0.17f);
                        g *= 1f - (wetMask * 0.15f);
                        b *= 1f - (wetMask * 0.11f);

                        float specWave = MathF.Sin((x * 0.12f) + (y * 0.06f) + (ProceduralEffectHelper.Hash01(x / 9, y / 9, seed ^ 991) * MathF.PI * 2f));
                        float spec = MathF.Max(0f, specWave) * wetMask * 0.22f;
                        r += 95f * spec;
                        g += 110f * spec;
                        b += 130f * spec;
                    }
                }
                else if (wetness > 0.0001f)
                {
                    float haze = wetness * 0.06f * (1f - (y / horizonPx));
                    r = ProceduralEffectHelper.Lerp(r, 210f, haze);
                    g = ProceduralEffectHelper.Lerp(g, 220f, haze);
                    b = ProceduralEffectHelper.Lerp(b, 235f, haze);
                }

                dstPixels[row + x] = new SKColor(
                    ProceduralEffectHelper.ClampToByte(r),
                    ProceduralEffectHelper.ClampToByte(g),
                    ProceduralEffectHelper.ClampToByte(b),
                    ProceduralEffectHelper.ClampToByte(a));
            }
        });

        return new SKBitmap(width, height, source.ColorType, source.AlphaType)
        {
            Pixels = dstPixels
        };
    }
}
