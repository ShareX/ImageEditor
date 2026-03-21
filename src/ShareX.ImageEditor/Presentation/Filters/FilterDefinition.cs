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

namespace ShareX.ImageEditor.Presentation.Filters;

public sealed class FilterDefinition
{
    public string Id { get; }

    public string Name { get; }

    public string BrowserLabel { get; }

    public string Icon { get; }

    public string Description { get; }

    public Func<ImageEffect> CreateEffect { get; }

    public IReadOnlyList<FilterParameterDefinition> Parameters { get; }

    public FilterDefinition(
        string id,
        string name,
        string browserLabel,
        string icon,
        string description,
        Func<ImageEffect> createEffect,
        IReadOnlyList<FilterParameterDefinition> parameters)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        BrowserLabel = browserLabel ?? throw new ArgumentNullException(nameof(browserLabel));
        Icon = icon ?? throw new ArgumentNullException(nameof(icon));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        CreateEffect = createEffect ?? throw new ArgumentNullException(nameof(createEffect));
        Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
    }

    public ImageEffect CreateConfiguredEffect(IEnumerable<FilterParameterState> parameterStates)
    {
        ImageEffect effect = CreateEffect();

        foreach (FilterParameterState parameterState in parameterStates)
        {
            parameterState.Definition.ApplyValue(effect, parameterState.GetValue());
        }

        return effect;
    }
}
