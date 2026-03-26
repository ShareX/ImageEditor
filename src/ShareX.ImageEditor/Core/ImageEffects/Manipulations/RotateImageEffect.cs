using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public sealed class RotateImageEffect : ImageEffectBase
{
    public override string Id => "rotate_custom_angle";
    public override string Name => "Rotate";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => "RotateCw";
    public override string Description => "Rotates the image by a custom angle.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<RotateImageEffect>("angle", "Angle", -360f, 360f, 0f, (e, v) => e.Angle = v),
        EffectParameters.Bool<RotateImageEffect>("auto_resize", "Auto resize", true, (e, v) => e.AutoResize = v)
    ];

    public float Angle { get; set; }
    public bool AutoResize { get; set; } = true;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        if (Angle % 90 == 0 && AutoResize)
        {
            return RotateOrthogonal(source, (int)Angle);
        }

        return AutoResize ? RotateArbitrary(source, Angle) : RotateClipped(source, Angle);
    }

    private SKBitmap RotateClipped(SKBitmap source, float angle)
    {
        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using (SKCanvas canvas = new SKCanvas(result))
        {
            canvas.Clear(SKColors.Transparent);
            canvas.Translate(source.Width / 2f, source.Height / 2f);
            canvas.RotateDegrees(angle);
            canvas.Translate(-source.Width / 2f, -source.Height / 2f);
            canvas.DrawBitmap(source, 0, 0);
        }
        return result;
    }

    private SKBitmap RotateOrthogonal(SKBitmap source, int angle)
    {
        angle = angle % 360;
        if (angle < 0) angle += 360;

        int width = source.Width;
        int height = source.Height;

        if (angle == 90 || angle == 270)
        {
            (width, height) = (height, width);
        }

        SKBitmap result = new SKBitmap(width, height, source.ColorType, source.AlphaType);
        using (SKCanvas canvas = new SKCanvas(result))
        {
            canvas.Clear(SKColors.Transparent);

            if (angle == 90)
            {
                canvas.Translate(width, 0);
                canvas.RotateDegrees(90);
            }
            else if (angle == 180)
            {
                canvas.Translate(width, height);
                canvas.RotateDegrees(180);
            }
            else if (angle == 270)
            {
                canvas.Translate(0, height);
                canvas.RotateDegrees(270);
            }

            canvas.DrawBitmap(source, 0, 0);
        }
        return result;
    }

    private SKBitmap RotateArbitrary(SKBitmap source, float angle)
    {
        SKMatrix matrix = SKMatrix.CreateRotationDegrees(angle, source.Width / 2f, source.Height / 2f);
        SKRect rect = new SKRect(0, 0, source.Width, source.Height);
        SKRect mapped = matrix.MapRect(rect);

        int newWidth = (int)Math.Ceiling(mapped.Width);
        int newHeight = (int)Math.Ceiling(mapped.Height);

        SKBitmap result = new SKBitmap(newWidth, newHeight, source.ColorType, source.AlphaType);
        using (SKCanvas canvas = new SKCanvas(result))
        {
            canvas.Clear(SKColors.Transparent);
            canvas.Translate(newWidth / 2f, newHeight / 2f);
            canvas.RotateDegrees(angle);
            canvas.Translate(-source.Width / 2f, -source.Height / 2f);
            canvas.DrawBitmap(source, 0, 0);
        }
        return result;
    }

    public static RotateImageEffect Clockwise90 => new() { Angle = 90 };
    public static RotateImageEffect CounterClockwise90 => new() { Angle = -90 };
    public static RotateImageEffect Rotate180 => new() { Angle = 180 };
    public static RotateImageEffect Custom(float angle, bool autoResize = true) => new() { Angle = angle, AutoResize = autoResize };
}

