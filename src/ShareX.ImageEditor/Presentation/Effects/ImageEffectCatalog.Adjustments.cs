using ShareX.ImageEditor.Core.ImageEffects;
using ShareX.ImageEditor.Core.ImageEffects.Adjustments;

namespace ShareX.ImageEditor.Presentation.Effects;

public static partial class ImageEffectCatalog
{
    private static IReadOnlyList<EffectDefinition> BuildAdjustmentDefinitions()
    {
        return
        [
            BespokeEffect<SelectiveColorImageEffect>("selective_color", ImageEffectCategory.Adjustments, "selective_color")
        ];
    }
}
