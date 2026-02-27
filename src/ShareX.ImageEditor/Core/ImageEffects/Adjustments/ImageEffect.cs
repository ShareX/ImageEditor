using SkiaSharp;
using ShareX.ImageEditor.Services;

namespace ShareX.ImageEditor.ImageEffects.Adjustments;

public abstract class ImageEffect : ShareX.ImageEditor.ImageEffects.ImageEffect
{
    public override ImageEffectCategory Category => ImageEffectCategory.Adjustments;
    public override bool HasParameters => true;

    // Diagnostic: records whether the host has an active GPU backend.
    // Off-screen GPU effect processing is deferred (see XIP0042 §1.3 Option A / Option B).
    // ApplyColorFilter always runs on the CPU path until the scoped-resource approach
    // (IPlatformGraphics.TryGetGrContext or a dedicated offscreen context) is implemented.
    private static GRContext? _gpuContext;
    private const int GpuPixelThreshold = 160_000; // reserved for future GPU path (≈ 400×400 px)

    public static void SetGpuContext(GRContext? context)
    {
        bool wasNull = _gpuContext == null;
        bool isNull = context == null;
        _gpuContext = context;

        // Log only on state transitions to avoid noise on every Render() frame
        if (wasNull && !isNull)
            EditorServices.ReportInformation(nameof(ImageEffect), "GRContext assigned by host; GPU acceleration active.");
        else if (!wasNull && isNull)
            EditorServices.ReportInformation(nameof(ImageEffect), "GRContext cleared; falling back to software rendering.");
    }

    protected static SKBitmap ApplyColorMatrix(SKBitmap source, float[] matrix)
    {
        using var filter = SKColorFilter.CreateColorMatrix(matrix);
        return ApplyColorFilter(source, filter);
    }

    protected static SKBitmap ApplyColorFilter(SKBitmap source, SKColorFilter filter)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using (SKCanvas canvas = new SKCanvas(result))
        {
            canvas.Clear(SKColors.Transparent);
            using (SKPaint paint = new SKPaint())
            {
                paint.ColorFilter = filter;
                canvas.DrawBitmap(source, 0, 0, paint);
            }
        }
        return result;
    }

    protected unsafe static SKBitmap ApplyPixelOperation(SKBitmap source, Func<SKColor, SKColor> operation)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);

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
            var srcPixels = source.Pixels;
            var dstPixels = new SKColor[srcPixels.Length];

            for (int i = 0; i < srcPixels.Length; i++)
            {
                dstPixels[i] = operation(srcPixels[i]);
            }

            result.Pixels = dstPixels;
        }

        return result;
    }
}
