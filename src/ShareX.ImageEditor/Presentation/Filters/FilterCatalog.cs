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

using Avalonia.Media;
using ShareX.ImageEditor.Core.ImageEffects;
using SkiaSharp;

namespace ShareX.ImageEditor.Presentation.Filters;

public static partial class FilterCatalog
{
    private sealed record FilterPresentationMetadata(
        string BrowserLabel,
        string Icon,
        string Description,
        bool IncludeInFiltersCategory = true);

    private static readonly IReadOnlyDictionary<string, FilterPresentationMetadata> _presentationById = BuildPresentationMetadata();
    private static readonly IReadOnlyList<FilterDefinition> _definitions = BuildDefinitions();

    private static readonly IReadOnlyDictionary<string, FilterDefinition> _definitionsById =
        _definitions.ToDictionary(definition => definition.Id, StringComparer.OrdinalIgnoreCase);

    public static IReadOnlyList<FilterDefinition> Definitions => _definitions;

    public static bool TryGetDefinition(string id, out FilterDefinition? definition)
    {
        return _definitionsById.TryGetValue(id, out definition);
    }

    private static FilterDefinition Filter<TEffect>(string id, params FilterParameterDefinition[] parameters)
        where TEffect : ImageEffect, new()
    {
        return Filter(id, static () => new TEffect(), parameters);
    }

    private static FilterDefinition Filter(string id, Func<ImageEffect> createEffect, params FilterParameterDefinition[] parameters)
    {
        FilterPresentationMetadata metadata = GetMetadata(id);
        return new FilterDefinition(
            id,
            metadata.BrowserLabel,
            metadata.Icon,
            metadata.Description,
            createEffect,
            parameters,
            metadata.IncludeInFiltersCategory);
    }

    private static SliderFilterParameterDefinition IntSlider<TEffect>(
        string key,
        string label,
        int minimum,
        int maximum,
        int defaultValue,
        Action<TEffect, int> applyValue,
        int tickFrequency = 1,
        bool isSnapToTickEnabled = true,
        string valueStringFormat = "{}{0:0}")
        where TEffect : ImageEffect
    {
        return new SliderFilterParameterDefinition(
            key,
            label,
            minimum,
            maximum,
            defaultValue,
            tickFrequency,
            isSnapToTickEnabled,
            valueStringFormat,
            (effect, value) => applyValue((TEffect)effect, (int)Math.Round(value)));
    }

    private static SliderFilterParameterDefinition FloatSlider<TEffect>(
        string key,
        string label,
        double minimum,
        double maximum,
        double defaultValue,
        Action<TEffect, float> applyValue,
        double tickFrequency = 1,
        bool isSnapToTickEnabled = true,
        string valueStringFormat = "{}{0:0}")
        where TEffect : ImageEffect
    {
        return new SliderFilterParameterDefinition(
            key,
            label,
            minimum,
            maximum,
            defaultValue,
            tickFrequency,
            isSnapToTickEnabled,
            valueStringFormat,
            (effect, value) => applyValue((TEffect)effect, (float)value));
    }

    private static SliderFilterParameterDefinition DoubleSlider<TEffect>(
        string key,
        string label,
        double minimum,
        double maximum,
        double defaultValue,
        Action<TEffect, double> applyValue,
        double tickFrequency = 1,
        bool isSnapToTickEnabled = true,
        string valueStringFormat = "{}{0:0}")
        where TEffect : ImageEffect
    {
        return new SliderFilterParameterDefinition(
            key,
            label,
            minimum,
            maximum,
            defaultValue,
            tickFrequency,
            isSnapToTickEnabled,
            valueStringFormat,
            (effect, value) => applyValue((TEffect)effect, value));
    }

    private static CheckboxFilterParameterDefinition BoolParameter<TEffect>(
        string key,
        string label,
        bool defaultValue,
        Action<TEffect, bool> applyValue)
        where TEffect : ImageEffect
    {
        return new CheckboxFilterParameterDefinition(
            key,
            label,
            defaultValue,
            (effect, value) => applyValue((TEffect)effect, value));
    }

    private static EnumFilterParameterDefinition EnumParameter<TEffect, TEnum>(
        string key,
        string label,
        TEnum defaultValue,
        Action<TEffect, TEnum> applyValue,
        params (string Label, TEnum Value)[] options)
        where TEffect : ImageEffect
        where TEnum : notnull
    {
        int defaultIndex = Array.FindIndex(
            options,
            option => EqualityComparer<TEnum>.Default.Equals(option.Value, defaultValue));

        if (defaultIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(defaultValue), defaultValue, "Enum default value must be present in the options list.");
        }

        return new EnumFilterParameterDefinition(
            key,
            label,
            options.Select(option => new FilterOptionDefinition(option.Label, option.Value!)).ToArray(),
            defaultIndex,
            (effect, value) => applyValue((TEffect)effect, value is TEnum typedValue ? typedValue : defaultValue));
    }

    private static ColorFilterParameterDefinition ColorParameter<TEffect>(
        string key,
        string label,
        Color defaultValue,
        Action<TEffect, Color> applyValue)
        where TEffect : ImageEffect
    {
        return new ColorFilterParameterDefinition(
            key,
            label,
            defaultValue,
            (effect, value) => applyValue((TEffect)effect, value));
    }

    private static NumericFilterParameterDefinition IntNumeric<TEffect>(
        string key,
        string label,
        int minimum,
        int maximum,
        int defaultValue,
        Action<TEffect, int> applyValue,
        int increment = 1,
        string formatString = "0")
        where TEffect : ImageEffect
    {
        return new NumericFilterParameterDefinition(
            key,
            label,
            minimum,
            maximum,
            defaultValue,
            increment,
            formatString,
            (effect, value) => applyValue(
                (TEffect)effect,
                decimal.ToInt32(decimal.Round(value, 0, MidpointRounding.AwayFromZero))));
    }

    private static NumericFilterParameterDefinition DoubleNumeric<TEffect>(
        string key,
        string label,
        double minimum,
        double maximum,
        double defaultValue,
        Action<TEffect, double> applyValue,
        double increment = 1,
        string formatString = "0.##")
        where TEffect : ImageEffect
    {
        return new NumericFilterParameterDefinition(
            key,
            label,
            (decimal)minimum,
            (decimal)maximum,
            (decimal)defaultValue,
            (decimal)increment,
            formatString,
            (effect, value) => applyValue((TEffect)effect, (double)value));
    }

    private static TextFilterParameterDefinition TextParameter<TEffect>(
        string key,
        string label,
        string defaultValue,
        Action<TEffect, string> applyValue)
        where TEffect : ImageEffect
    {
        return new TextFilterParameterDefinition(
            key,
            label,
            defaultValue,
            (effect, value) => applyValue((TEffect)effect, value));
    }

    private static FilterPresentationMetadata GetMetadata(string id)
    {
        if (_presentationById.TryGetValue(id, out FilterPresentationMetadata? metadata))
        {
            return metadata;
        }

        throw new KeyNotFoundException($"No filter catalog metadata was found for '{id}'.");
    }

    private static Color Argb(byte alpha, byte red, byte green, byte blue) => Color.FromArgb(alpha, red, green, blue);

    private static SKColor ToSkColor(Color color) => new(color.R, color.G, color.B, color.A);
}
