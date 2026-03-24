using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Helpers;

internal static class DistortionEffectHelper
{
    public static SKBitmap CreateBitmap(SKBitmap source, int width, int height, SKColor[] pixels)
    {
        return new SKBitmap(width, height, source.ColorType, source.AlphaType)
        {
            Pixels = pixels
        };
    }

    public static float PercentageToX(int width, float percentage)
    {
        return (Math.Clamp(percentage, 0f, 100f) / 100f) * Math.Max(0, width - 1);
    }

    public static float PercentageToY(int height, float percentage)
    {
        return (Math.Clamp(percentage, 0f, 100f) / 100f) * Math.Max(0, height - 1);
    }

    public static SKColor SampleClamped(SKColor[] pixels, int width, int height, float x, float y)
    {
        return ProceduralEffectHelper.BilinearSample(pixels, width, height, x, y);
    }

    public static SKColor SampleTransparent(SKColor[] pixels, int width, int height, float x, float y)
    {
        if (x < 0f || x > width - 1 || y < 0f || y > height - 1)
        {
            return SKColors.Transparent;
        }

        return ProceduralEffectHelper.BilinearSample(pixels, width, height, x, y);
    }

    public static SKColor Blend(SKColor from, SKColor to, float amount)
    {
        float t = ProceduralEffectHelper.Clamp01(amount);
        return new SKColor(
            ProceduralEffectHelper.ClampToByte(ProceduralEffectHelper.Lerp(from.Red, to.Red, t)),
            ProceduralEffectHelper.ClampToByte(ProceduralEffectHelper.Lerp(from.Green, to.Green, t)),
            ProceduralEffectHelper.ClampToByte(ProceduralEffectHelper.Lerp(from.Blue, to.Blue, t)),
            ProceduralEffectHelper.ClampToByte(ProceduralEffectHelper.Lerp(from.Alpha, to.Alpha, t)));
    }

    public static SKColor MultiplyRgb(SKColor color, float factor)
    {
        float value = Math.Max(0f, factor);
        return new SKColor(
            ProceduralEffectHelper.ClampToByte(color.Red * value),
            ProceduralEffectHelper.ClampToByte(color.Green * value),
            ProceduralEffectHelper.ClampToByte(color.Blue * value),
            color.Alpha);
    }

    public static float GetLuminance(SKColor color)
    {
        return (0.2126f * color.Red) + (0.7152f * color.Green) + (0.0722f * color.Blue);
    }

    public static float WrapAngle(float radians)
    {
        float twoPi = MathF.PI * 2f;
        radians %= twoPi;
        if (radians < 0f)
        {
            radians += twoPi;
        }

        return radians;
    }
}
