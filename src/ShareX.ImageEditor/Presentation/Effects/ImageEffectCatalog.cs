#region License Information (GPL v3)

/*
    ShareX.ImageEditor - The UI-agnostic Editor library for ShareX
    Copyright (c) 2007-2026 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using ShareX.ImageEditor.Core.ImageEffects;

namespace ShareX.ImageEditor.Presentation.Effects;

public static class ImageEffectCatalog
{
    private static readonly IReadOnlyList<EffectDefinition> _definitions = DiscoveredEffectRegistry.Definitions;

    private static readonly IReadOnlyDictionary<string, EffectDefinition> _definitionsById =
        _definitions.ToDictionary(definition => definition.Id, StringComparer.OrdinalIgnoreCase);

    private static readonly IReadOnlyDictionary<ImageEffectCategory, IReadOnlyList<EffectDefinition>> _definitionsByCategory =
        _definitions
            .GroupBy(definition => definition.Category)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<EffectDefinition>)group.ToArray());

    public static IReadOnlyList<EffectDefinition> Definitions => _definitions;

    public static bool TryGetDefinition(string id, out EffectDefinition? definition)
    {
        return _definitionsById.TryGetValue(id, out definition);
    }

    /// <summary>
    /// Resolves browser label, icon, and description for an effect or operation id.
    /// </summary>
    public static bool TryGetBrowserPresentation(
        string id,
        out string browserLabel,
        out string icon,
        out string description)
    {
        if (TryGetDefinition(id, out EffectDefinition? definition) && definition != null)
        {
            browserLabel = definition.BrowserLabel;
            icon = definition.Icon;
            description = definition.Description;
            return true;
        }

        if (EditorOperationCatalog.TryGetDefinition(id, out EditorOperationDefinition? operation) && operation != null)
        {
            browserLabel = operation.BrowserLabel;
            icon = operation.Icon;
            description = operation.Description;
            return true;
        }

        browserLabel = string.Empty;
        icon = string.Empty;
        description = string.Empty;
        return false;
    }

    public static IReadOnlyList<EffectDefinition> GetByCategory(ImageEffectCategory category)
    {
        return _definitionsByCategory.TryGetValue(category, out var definitions) ? definitions : [];
    }
}
