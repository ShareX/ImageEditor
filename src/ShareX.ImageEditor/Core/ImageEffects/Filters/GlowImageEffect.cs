using SkiaSharp;

namespace ShareX.ImageEditor.ImageEffects.Filters;

public class GlowImageEffect : ImageEffect
{
    public int Size { get; set; }
    public float Strength { get; set; }
    public SKColor Color { get; set; }
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }

    public override string Name => "Glow";

    public override ImageEffectCategory Category => ImageEffectCategory.Filters;

    public GlowImageEffect(int size, float strength, SKColor color, int offsetX, int offsetY)
    {
        Size = size;
        Strength = strength;
        Color = color;
        OffsetX = offsetX;
        OffsetY = offsetY;
    }

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        // Compute one-sided canvas expansion based on offset direction:
        // Size is used as padding for the blur.
        int pad = Size;

        int expandLeft   = Math.Max(0, -OffsetX) + pad;
        int expandRight  = Math.Max(0,  OffsetX) + pad;
        int expandTop    = Math.Max(0, -OffsetY) + pad;
        int expandBottom = Math.Max(0,  OffsetY) + pad;

        int newWidth  = source.Width  + expandLeft + expandRight;
        int newHeight = source.Height + expandTop  + expandBottom;

        SKBitmap result = new SKBitmap(newWidth, newHeight);
        using SKCanvas canvas = new SKCanvas(result);
        canvas.Clear(SKColors.Transparent);

        int imageX = expandLeft;
        int imageY = expandTop;
        int glowX = imageX + OffsetX;
        int glowY = imageY + OffsetY;

        SKColor glowColor = Color.WithAlpha((byte)(255 * Strength / 100f));

        using SKPaint glowPaint = new SKPaint
        {
            ColorFilter = SKColorFilter.CreateBlendMode(glowColor, SKBlendMode.SrcIn),
            ImageFilter = SKImageFilter.CreateBlur(Size, Size)
        };

        // Draw glow
        canvas.DrawBitmap(source, glowX, glowY, glowPaint);
        // Draw original
        canvas.DrawBitmap(source, imageX, imageY);

        return result;
    }
}

