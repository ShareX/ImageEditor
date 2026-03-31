using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public sealed class ResizeImageEffect : ImageEffectBase
{
    public override string Id => "resize_image";
    public override string Name => "Resize image";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override string IconKey => LucideIcons.scaling;
    public override string Description => "Resizes the image.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntNumeric<ResizeImageEffect>("width", "Width", 0, 10000, 0, (e, v) => e.Width = v),
        EffectParameters.IntNumeric<ResizeImageEffect>("height", "Height", 0, 10000, 0, (e, v) => e.Height = v),
        EffectParameters.Bool<ResizeImageEffect>("maintain_aspect_ratio", "Maintain aspect ratio", false, (e, v) => e.MaintainAspectRatio = v)
    ];

    public int Width { get; set; }
    public int Height { get; set; }
    public bool MaintainAspectRatio { get; set; }

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int width = Width > 0 ? Width : source.Width;
        int height = Height > 0 ? Height : source.Height;

        if (width <= 0) width = source.Width;
        if (height <= 0) height = source.Height;

        if (MaintainAspectRatio)
        {
            double sourceAspect = (double)source.Width / source.Height;
            double targetAspect = (double)width / height;

            if (sourceAspect > targetAspect)
            {
                height = (int)Math.Round(width / sourceAspect);
            }
            else
            {
                width = (int)Math.Round(height * sourceAspect);
            }
        }

        SKImageInfo info = new SKImageInfo(width, height, source.ColorType, source.AlphaType, source.ColorSpace);
        return source.Resize(info, SKFilterQuality.High);
    }
}

