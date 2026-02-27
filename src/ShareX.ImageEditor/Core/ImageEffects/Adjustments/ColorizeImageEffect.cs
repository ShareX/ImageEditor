using SkiaSharp;


namespace ShareX.ImageEditor.ImageEffects.Adjustments;

public class ColorizeImageEffect : ImageEffect
{
    public override string Name => "Colorize";
    public override string IconKey => "IconTint";
    public SKColor Color { get; set; } = SKColors.Red; // Default
    public float Strength { get; set; } = 50f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        float strength = Math.Clamp(Strength, 0, 100);
        if (strength <= 0) return source.Copy();

        var grayscaleMatrix = new float[] {
            0.2126f, 0.7152f, 0.0722f, 0, 0,
            0.2126f, 0.7152f, 0.0722f, 0, 0,
            0.2126f, 0.7152f, 0.0722f, 0, 0,
            0,       0,       0,       1, 0
        };
        using var grayscale = SKColorFilter.CreateColorMatrix(grayscaleMatrix);
        using var tint = SKColorFilter.CreateBlendMode(Color, SKBlendMode.Modulate);
        using var composed = SKColorFilter.CreateCompose(tint, grayscale);

        if (strength >= 100)
            return ApplyColorFilter(source, composed);

        // Partial strength: blend colorized result over original at reduced opacity
        using var colorized = ApplyColorFilter(source, composed);
        var result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        using (var canvas = new SKCanvas(result))
        {
            canvas.Clear(SKColors.Transparent);
            canvas.DrawBitmap(source, 0, 0);
            using var blendPaint = new SKPaint
            {
                Color = new SKColor(255, 255, 255, (byte)(255 * strength / 100f))
            };
            canvas.DrawBitmap(colorized, 0, 0, blendPaint);
        }
        return result;
    }
}

