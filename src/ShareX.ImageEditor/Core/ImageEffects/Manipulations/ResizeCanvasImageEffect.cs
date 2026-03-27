using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public sealed class ResizeCanvasImageEffect : ImageEffectBase
{
    public override string Id => "resize_canvas";
    public override string Name => "Resize canvas";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => "Maximize";
    public override string Description => "Resizes the canvas.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntNumeric<ResizeCanvasImageEffect>("top", "Top", -10000, 10000, 0, (e, v) => e.Top = v),
        EffectParameters.IntNumeric<ResizeCanvasImageEffect>("right", "Right", -10000, 10000, 0, (e, v) => e.Right = v),
        EffectParameters.IntNumeric<ResizeCanvasImageEffect>("bottom", "Bottom", -10000, 10000, 0, (e, v) => e.Bottom = v),
        EffectParameters.IntNumeric<ResizeCanvasImageEffect>("left", "Left", -10000, 10000, 0, (e, v) => e.Left = v),
        EffectParameters.Color<ResizeCanvasImageEffect>("background_color", "Background color", SKColors.Transparent, (e, v) => e.BackgroundColor = v)
    ];

    public int Top { get; set; }
    public int Right { get; set; }
    public int Bottom { get; set; }
    public int Left { get; set; }
    public SKColor BackgroundColor { get; set; } = SKColors.Transparent;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int newWidth = source.Width + Left + Right;
        int newHeight = source.Height + Top + Bottom;

        if (newWidth <= 0 || newHeight <= 0)
        {
            return source.Copy();
        }

        SKBitmap result = new SKBitmap(newWidth, newHeight, source.ColorType, source.AlphaType);
        using SKCanvas canvas = new SKCanvas(result);
        canvas.Clear(BackgroundColor);
        canvas.DrawBitmap(source, Left, Top);
        return result;
    }
}
