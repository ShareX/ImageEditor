using System.Collections.Generic;
using System.Linq;

namespace ShareX.Editor.ImageEffects;

public static class ImageEffectRegistry
{
    public static IReadOnlyList<ImageEffect> Effects { get; }

    static ImageEffectRegistry()
    {
        var effects = new List<ImageEffect>
        {
            // Rotate
            RotateImageEffect.Clockwise90,
            RotateImageEffect.CounterClockwise90,
            RotateImageEffect.Rotate180,
            
            // Flip
            FlipImageEffect.Horizontal,
            FlipImageEffect.Vertical,
            
            // Resize (Placeholders for menu, actual logic requires parameters)
            new ResizeImageEffect(), 
            new AutoCropImageEffect(),
            
            // Adjustments
            new AdjustmentBrightnessImageEffect(),
            new AdjustmentContrastImageEffect(),
            new AdjustmentHueImageEffect(),
            new AdjustmentSaturationImageEffect(),
            new AdjustmentGammaImageEffect(),
            new AdjustmentAlphaImageEffect(),
            new AdjustmentColorizeImageEffect(),
            new AdjustmentSelectiveColorImageEffect(),
            new AdjustmentReplaceColorImageEffect(),
            
            // Filters
            new InvertImageEffect(),
            new GrayscaleImageEffect(),
            new BlackAndWhiteImageEffect(),
            new SepiaImageEffect(),
            new PolaroidImageEffect()
        };

        Effects = effects.AsReadOnly();
    }

    public static IEnumerable<ImageEffect> GetByCategory(ImageEffectCategory category)
    {
        return Effects.Where(e => e.Category == category);
    }
}
