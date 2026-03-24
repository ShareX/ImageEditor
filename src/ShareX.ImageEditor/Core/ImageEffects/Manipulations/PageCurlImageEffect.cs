using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public enum PageCurlCorner
{
    TopLeft,
    TopRight,
    BottomRight,
    BottomLeft
}

public class PageCurlImageEffect : ImageEffect
{
    public override string Name => "Page curl";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override bool HasParameters => true;

    public PageCurlCorner Corner { get; set; } = PageCurlCorner.BottomRight;
    public float CurlSize { get; set; } = 28f;
    public float CurlDepth { get; set; } = 55f;
    public float ShadowStrength { get; set; } = 60f;
    public SKColor BackColor { get; set; } = new(248, 244, 236);

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int width = source.Width;
        int height = source.Height;
        float curlSize = Math.Max(12f, Math.Min(width, height) * Math.Clamp(CurlSize, 5f, 80f) / 100f);
        float curlDepth01 = Math.Clamp(CurlDepth, 0f, 100f) / 100f;
        float shadow01 = Math.Clamp(ShadowStrength, 0f, 100f) / 100f;
        float foldLine = curlSize * (0.78f + (curlDepth01 * 0.22f));
        float shadowWidth = Math.Max(4f, curlSize * (0.08f + (shadow01 * 0.20f) + (curlDepth01 * 0.10f)));

        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];

        Parallel.For(0, height, y =>
        {
            int row = y * width;
            for (int x = 0; x < width; x++)
            {
                SKColor original = srcPixels[row + x];
                ToCornerLocal(Corner, width, height, x, y, out float localX, out float localY);

                if (localX > curlSize + shadowWidth || localY > curlSize + shadowWidth)
                {
                    dstPixels[row + x] = original;
                    continue;
                }

                float diagonal = localX + localY;
                if (diagonal <= foldLine)
                {
                    float reflectedX = foldLine - localY;
                    float reflectedY = foldLine - localX;
                    FromCornerLocal(Corner, width, height, reflectedX, reflectedY, out float sampleX, out float sampleY);

                    SKColor reflected = DistortionEffectHelper.SampleClamped(srcPixels, width, height, sampleX, sampleY);
                    float curlMix = 0.28f + (curlDepth01 * 0.28f) + ((1f - (diagonal / foldLine)) * 0.18f);
                    SKColor curled = DistortionEffectHelper.Blend(reflected, BackColor, curlMix);

                    float highlight = (0.05f + (curlDepth01 * 0.09f)) *
                        ProceduralEffectHelper.SmoothStep(foldLine * 0.55f, 0f, diagonal);

                    dstPixels[row + x] = DistortionEffectHelper.Blend(
                        curled,
                        new SKColor(255, 255, 255, curled.Alpha),
                        highlight);
                }
                else if (diagonal < foldLine + shadowWidth)
                {
                    float fade = 1f - ((diagonal - foldLine) / shadowWidth);
                    float shade = 1f - (shadow01 * fade * 0.42f);
                    dstPixels[row + x] = DistortionEffectHelper.MultiplyRgb(original, shade);
                }
                else
                {
                    dstPixels[row + x] = original;
                }
            }
        });

        return DistortionEffectHelper.CreateBitmap(source, width, height, dstPixels);
    }

    private static void ToCornerLocal(PageCurlCorner corner, int width, int height, float x, float y, out float localX, out float localY)
    {
        switch (corner)
        {
            case PageCurlCorner.TopLeft:
                localX = x;
                localY = y;
                break;
            case PageCurlCorner.TopRight:
                localX = (width - 1) - x;
                localY = y;
                break;
            case PageCurlCorner.BottomRight:
                localX = (width - 1) - x;
                localY = (height - 1) - y;
                break;
            default:
                localX = x;
                localY = (height - 1) - y;
                break;
        }
    }

    private static void FromCornerLocal(PageCurlCorner corner, int width, int height, float localX, float localY, out float x, out float y)
    {
        switch (corner)
        {
            case PageCurlCorner.TopLeft:
                x = localX;
                y = localY;
                break;
            case PageCurlCorner.TopRight:
                x = (width - 1) - localX;
                y = localY;
                break;
            case PageCurlCorner.BottomRight:
                x = (width - 1) - localX;
                y = (height - 1) - localY;
                break;
            default:
                x = localX;
                y = (height - 1) - localY;
                break;
        }
    }
}
