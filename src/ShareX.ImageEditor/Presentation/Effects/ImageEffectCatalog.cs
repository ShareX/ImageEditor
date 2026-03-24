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

namespace ShareX.ImageEditor.Presentation.Effects;

public static partial class ImageEffectCatalog
{
    private sealed record EffectPresentationMetadata(
        string BrowserLabel,
        string Icon,
        string Description);

    private static readonly IReadOnlyDictionary<string, EffectPresentationMetadata> _presentationById = BuildPresentationMetadata();
    private static readonly IReadOnlyList<EffectDefinition> _definitions = BuildAllDefinitions();

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

    public static IReadOnlyList<EffectDefinition> GetByCategory(ImageEffectCategory category)
    {
        return _definitionsByCategory.TryGetValue(category, out var definitions) ? definitions : [];
    }

    private static IReadOnlyList<EffectDefinition> BuildAllDefinitions()
    {
        var all = new List<EffectDefinition>();
        all.AddRange(BuildFilterDefinitions());
        all.AddRange(BuildAdjustmentDefinitions());
        all.AddRange(BuildManipulationDefinitions());
        all.AddRange(BuildDrawingDefinitions());
        return all.AsReadOnly();
    }

    // --- Shared helper builders ---

    private static EffectDefinition Effect<TEffect>(
        string id,
        ImageEffectCategory category,
        params EffectParameterDefinition[] parameters)
        where TEffect : ImageEffect, new()
    {
        return Effect(id, category, static () => new TEffect(), parameters);
    }

    private static EffectDefinition Effect(
        string id,
        ImageEffectCategory category,
        Func<ImageEffect> createEffect,
        params EffectParameterDefinition[] parameters)
    {
        EffectPresentationMetadata metadata = GetMetadata(id);
        return new EffectDefinition(
            id,
            metadata.BrowserLabel,
            metadata.Icon,
            metadata.Description,
            category,
            createEffect,
            parameters);
    }

    private static EffectDefinition ImmediateEffect<TEffect>(
        string id,
        ImageEffectCategory category)
        where TEffect : ImageEffect, new()
    {
        EffectPresentationMetadata metadata = GetMetadata(id);
        return new EffectDefinition(
            id,
            metadata.BrowserLabel,
            metadata.Icon,
            metadata.Description,
            category,
            static () => new TEffect(),
            [],
            applyImmediately: true);
    }

    private static EffectDefinition BespokeEffect<TEffect>(
        string id,
        ImageEffectCategory category,
        string customEditorKey)
        where TEffect : ImageEffect, new()
    {
        EffectPresentationMetadata metadata = GetMetadata(id);
        return new EffectDefinition(
            id,
            metadata.BrowserLabel,
            metadata.Icon,
            metadata.Description,
            category,
            static () => new TEffect(),
            [],
            customEditorKey: customEditorKey);
    }

    private static SliderParameterDefinition IntSlider<TEffect>(
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
        return new SliderParameterDefinition(
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

    private static SliderParameterDefinition FloatSlider<TEffect>(
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
        return new SliderParameterDefinition(
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

    private static SliderParameterDefinition DoubleSlider<TEffect>(
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
        return new SliderParameterDefinition(
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

    private static CheckboxParameterDefinition BoolParameter<TEffect>(
        string key,
        string label,
        bool defaultValue,
        Action<TEffect, bool> applyValue)
        where TEffect : ImageEffect
    {
        return new CheckboxParameterDefinition(
            key,
            label,
            defaultValue,
            (effect, value) => applyValue((TEffect)effect, value));
    }

    private static EnumParameterDefinition EnumParameter<TEffect, TEnum>(
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

        return new EnumParameterDefinition(
            key,
            label,
            options.Select(option => new EffectOptionDefinition(option.Label, option.Value!)).ToArray(),
            defaultIndex,
            (effect, value) => applyValue((TEffect)effect, value is TEnum typedValue ? typedValue : defaultValue));
    }

    private static ColorParameterDefinition ColorParameter<TEffect>(
        string key,
        string label,
        Color defaultValue,
        Action<TEffect, Color> applyValue)
        where TEffect : ImageEffect
    {
        return new ColorParameterDefinition(
            key,
            label,
            defaultValue,
            (effect, value) => applyValue((TEffect)effect, value));
    }

    private static NumericParameterDefinition IntNumeric<TEffect>(
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
        return new NumericParameterDefinition(
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

    private static NumericParameterDefinition DoubleNumeric<TEffect>(
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
        return new NumericParameterDefinition(
            key,
            label,
            (decimal)minimum,
            (decimal)maximum,
            (decimal)defaultValue,
            (decimal)increment,
            formatString,
            (effect, value) => applyValue((TEffect)effect, (double)value));
    }

    private static TextParameterDefinition TextParameter<TEffect>(
        string key,
        string label,
        string defaultValue,
        Action<TEffect, string> applyValue)
        where TEffect : ImageEffect
    {
        return new TextParameterDefinition(
            key,
            label,
            defaultValue,
            (effect, value) => applyValue((TEffect)effect, value));
    }

    private static FilePathParameterDefinition FilePathParameter<TEffect>(
        string key,
        string label,
        string defaultValue,
        Action<TEffect, string> applyValue,
        string? fileFilter = null)
        where TEffect : ImageEffect
    {
        return new FilePathParameterDefinition(
            key,
            label,
            defaultValue,
            (effect, value) => applyValue((TEffect)effect, value),
            fileFilter);
    }

    private static EffectPresentationMetadata GetMetadata(string id)
    {
        if (_presentationById.TryGetValue(id, out EffectPresentationMetadata? metadata))
        {
            return metadata;
        }
        return BuildFallbackMetadata(id);
    }

    private static Color Argb(byte alpha, byte red, byte green, byte blue) => Color.FromArgb(alpha, red, green, blue);

    private static SKColor ToSkColor(Color color) => new(color.R, color.G, color.B, color.A);

    private static EffectPresentationMetadata BuildFallbackMetadata(string id)
    {
        string browserLabel = string.Join(
            " ",
            id.Split('_', StringSplitOptions.RemoveEmptyEntries)
                .Select(FormatPresentationToken))
            + "...";

        return new EffectPresentationMetadata(
            browserLabel,
            string.Empty,
            $"Applies the {browserLabel[..^3]} effect.");
    }

    /// <summary>
    /// Title-cases a snake_case segment for display, preserving common acronyms (3D, RGB, …).
    /// </summary>
    private static string FormatPresentationToken(string segment)
    {
        if (string.IsNullOrEmpty(segment))
        {
            return segment;
        }

        return segment.ToLowerInvariant() switch
        {
            "2d" => "2D",
            "3d" => "3D",
            "rgb" => "RGB",
            "cmyk" => "CMYK",
            "vhs" => "VHS",
            "dvd" => "DVD",
            "lcd" => "LCD",
            "crt" => "CRT",
            "ui" => "UI",
            "ux" => "UX",
            "url" => "URL",
            "uri" => "URI",
            "api" => "API",
            "css" => "CSS",
            "html" => "HTML",
            "sql" => "SQL",
            "jpg" or "jpeg" => "JPEG",
            "png" => "PNG",
            "gif" => "GIF",
            "webp" => "WebP",
            "avif" => "AVIF",
            "ascii" => "ASCII",
            _ => char.ToUpperInvariant(segment[0]) + segment[1..].ToLowerInvariant()
        };
    }
}
