using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public abstract class AdjustmentImageEffectBase : ImageEffectBase
{
    public override ImageEffectCategory Category => ImageEffectCategory.Adjustments;

    protected static SKBitmap ApplyColorMatrix(SKBitmap source, float[] matrix)
    {
        using SKColorFilter filter = SKColorFilter.CreateColorMatrix(matrix);
        return ApplyColorFilter(source, filter);
    }

    protected static SKBitmap ApplyColorFilter(SKBitmap source, SKColorFilter filter)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = new(source.Width, source.Height, source.ColorType, source.AlphaType);
        using SKCanvas canvas = new(result);
        canvas.Clear(SKColors.Transparent);

        using SKPaint paint = new() { ColorFilter = filter };
        canvas.DrawBitmap(source, 0, 0, paint);
        return result;
    }

    protected unsafe static SKBitmap ApplyPixelOperation(SKBitmap source, Func<SKColor, SKColor> operation)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = new(source.Width, source.Height, source.ColorType, source.AlphaType);

        if (source.ColorType == SKColorType.Bgra8888)
        {
            int count = source.Width * source.Height;
            SKColor* srcPtr = (SKColor*)source.GetPixels();
            SKColor* dstPtr = (SKColor*)result.GetPixels();

            for (int i = 0; i < count; i++)
            {
                *dstPtr++ = operation(*srcPtr++);
            }
        }
        else
        {
            SKColor[] srcPixels = source.Pixels;
            SKColor[] dstPixels = new SKColor[srcPixels.Length];

            for (int i = 0; i < srcPixels.Length; i++)
            {
                dstPixels[i] = operation(srcPixels[i]);
            }

            result.Pixels = dstPixels;
        }

        return result;
    }
}
