using ShareX.ImageEditor.Presentation.Theming;
using System.Reflection;

namespace ShareX.ImageEditor.Presentation.Effects;

internal static class EffectIconResolver
{
    private static readonly IReadOnlyDictionary<string, string> _icons = typeof(LucideIcons)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(field => field.FieldType == typeof(string))
        .ToDictionary(field => field.Name, field => (string)(field.GetValue(null) ?? string.Empty), StringComparer.OrdinalIgnoreCase);

    public static string Resolve(string? iconKey)
    {
        if (string.IsNullOrWhiteSpace(iconKey))
        {
            return string.Empty;
        }

        return _icons.TryGetValue(iconKey, out string? icon) ? icon : iconKey;
    }
}
