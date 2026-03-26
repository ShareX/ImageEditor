using ShareX.ImageEditor.Core.ImageEffects;

namespace ShareX.ImageEditor.Presentation.Effects;

internal static class DiscoveredEffectRegistry
{
    private static readonly Lazy<IReadOnlyList<EffectDefinition>> _definitions = new(BuildDefinitions);

    public static IReadOnlyList<EffectDefinition> Definitions => _definitions.Value;

    private static IReadOnlyList<EffectDefinition> BuildDefinitions()
    {
        List<EffectDefinition> definitions = [];

        foreach (Type type in typeof(ImageEffectBase).Assembly.GetTypes()
                     .Where(type => type is { IsAbstract: false, IsClass: true } && typeof(ImageEffectBase).IsAssignableFrom(type)))
        {
            if (type.GetConstructor(Type.EmptyTypes) == null)
            {
                continue;
            }

            if (Activator.CreateInstance(type) is not ImageEffectBase effect)
            {
                continue;
            }

            definitions.Add(DiscoveredEffectPresentationAdapter.CreateDefinition(effect));
        }

        return definitions
            .OrderBy(definition => definition.Category)
            .ThenBy(definition => definition.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
}
