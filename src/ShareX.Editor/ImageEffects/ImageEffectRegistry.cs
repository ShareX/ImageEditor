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
            // Manipulations - Rotate
            ManipulationsRotateImageEffect.Clockwise90,
            ManipulationsRotateImageEffect.CounterClockwise90,
            ManipulationsRotateImageEffect.Rotate180,
            
            // Manipulations - Flip
            ManipulationsFlipImageEffect.Horizontal,
            ManipulationsFlipImageEffect.Vertical,
            
            // Manipulations - Resize (parameterized)
            new ManipulationsResizeImageEffect(), 
            new ManipulationsAutoCropImageEffect(),
            
            // Adjustments
            new AdjustmentsBrightnessImageEffect(),
            new AdjustmentsContrastImageEffect(),
            new AdjustmentsHueImageEffect(),
            new AdjustmentsSaturationImageEffect(),
            new AdjustmentsGammaImageEffect(),
            new AdjustmentsAlphaImageEffect(),
            new AdjustmentsColorizeImageEffect(),
            new AdjustmentsSelectiveColorImageEffect(),
            new AdjustmentsReplaceColorImageEffect(),
            
            // Filters (parameterless)
            new FiltersBlurImageEffect(),
            new FiltersPixelateImageEffect(),
            new FiltersSharpenImageEffect(),
            
            // Adjustments - Color Filters
            new AdjustmentsInvertImageEffect(),
            new AdjustmentsGrayscaleImageEffect(),
            new AdjustmentsBlackAndWhiteImageEffect(),
            new AdjustmentsSepiaImageEffect(),
            new AdjustmentsPolaroidImageEffect()
        };

        Effects = effects.AsReadOnly();
    }

    public static IEnumerable<ImageEffect> GetByCategory(ImageEffectCategory category)
    {
        return Effects.Where(e => e.Category == category);
    }
}
