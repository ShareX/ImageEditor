using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public sealed class CropImageEffect : ImageEffectBase
{
    public override string Id => "crop_image";
    public override string Name => "Crop image";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => "Crop";
    public override string Description => "Crops the image.";
    public override string? EditorKey => "crop_image";

    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int cropX = Math.Clamp(X, 0, source.Width);
        int cropY = Math.Clamp(Y, 0, source.Height);
        int cropW = Math.Clamp(Width, 0, source.Width - cropX);
        int cropH = Math.Clamp(Height, 0, source.Height - cropY);

        if (cropW <= 0 || cropH <= 0)
        {
            return source.Copy();
        }

        SKBitmap result = new SKBitmap(cropW, cropH, source.ColorType, source.AlphaType);
        using SKCanvas canvas = new SKCanvas(result);
        canvas.Clear(SKColors.Transparent);
        canvas.DrawBitmap(source, new SKRect(cropX, cropY, cropX + cropW, cropY + cropH), new SKRect(0, 0, cropW, cropH));
        return result;
    }
}
