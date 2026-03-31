using ShareX.ImageEditor.Core.ImageEffects;

namespace ShareX.ImageEditor.Presentation.Effects;

internal static class DiscoveredEffectPresentationAdapter
{
    public static EffectDefinition CreateDefinition(ImageEffectBase effect)
    {
        Type effectType = effect.GetType();

        return new EffectDefinition(
            effect.Id,
            effect.Name,
            effect.BrowserLabel,
            effect.IconKey,
            effect.Description,
            effect.Category,
            () => (ImageEffect)Activator.CreateInstance(effectType)!,
            [],
            effect.Parameters,
            applyImmediately: effect.ExecutionMode == EffectExecutionMode.Immediate);
    }
}