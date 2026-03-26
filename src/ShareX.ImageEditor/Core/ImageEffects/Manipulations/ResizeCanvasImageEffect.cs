using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public sealed class ResizeCanvasImageEffect : ImageEffectBase
{
    public override string Id => "resize_canvas";
    public override string Name => "Resize canvas";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => "Maximize";
    public override string Description => "Resizes the canvas.";
    public override string? EditorKey => "resize_canvas";

    public int Width { get; set; }
    public int Height { get; set; }
    public int OffsetX { get; set; }
    public int OffsetY { get; set; }

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int w = Width > 0 ? Width : source.Width;
        int h = Height > 0 ? Height : source.Height;

        SKBitmap result = new SKBitmap(w, h, source.ColorType, source.AlphaType);
        using SKCanvas canvas = new SKCanvas(result);
        canvas.Clear(SKColors.Transparent);
        canvas.DrawBitmap(source, OffsetX, OffsetY);
        return result;
    }
}
