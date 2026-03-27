using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public sealed class AutoCropImageEffect : ImageEffectBase
{
    public override string Id => "auto_crop_image";
    public override string Name => "Auto crop image";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => LucideIcons.scan;
    public override string Description => "Automatically crops the image using tolerance on edge pixels.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.Color<AutoCropImageEffect>("color", "Color", SKColors.Transparent, (e, v) => e.Color = v),
        EffectParameters.IntSlider<AutoCropImageEffect>("tolerance", "Tolerance", 0, 255, 0, (e, v) => e.Tolerance = v)
    ];

    private SKColor _color;
    private int _tolerance;

    // Exposed for schema-driven dialog parameter binding.
    public SKColor Color
    {
        get => _color;
        set => _color = value;
    }

    // Exposed for schema-driven dialog parameter binding.
    public int Tolerance
    {
        get => _tolerance;
        set => _tolerance = value;
    }

    public AutoCropImageEffect(SKColor color, int tolerance = 0)
    {
        _color = color;
        _tolerance = tolerance;
    }

    public AutoCropImageEffect()
    {
        _color = SKColors.Transparent;
        _tolerance = 0;
    }

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int width = source.Width;
        int height = source.Height;

        // Dialog/catalog definitions historically pass `Transparent` for the match color.
        // In that case, align behavior with `EditorCore.AutoCrop()` by using the source's top-left pixel.
        SKColor matchColor = _color == SKColors.Transparent && width > 0 && height > 0
            ? source.GetPixel(0, 0)
            : _color;

        int minX = width, minY = height, maxX = 0, maxY = 0;
        bool hasContent = false;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                SKColor pixel = source.GetPixel(x, y);
                if (!ImageHelpers.ColorsMatch(pixel, matchColor, _tolerance))
                {
                    hasContent = true;
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }
        }

        if (!hasContent)
        {
            return new SKBitmap(1, 1, source.ColorType, source.AlphaType);
        }

        int cropWidth = maxX - minX + 1;
        int cropHeight = maxY - minY + 1;

        return ImageHelpers.Crop(source, minX, minY, cropWidth, cropHeight);
    }
}

