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
using ShareX.ImageEditor.Core.ImageEffects.Filters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Presentation.Filters;

public static class FilterCatalog
{
    private static readonly IReadOnlyList<FilterDefinition> _definitions =
    [
        new FilterDefinition(
            id: "blur",
            name: "Blur",
            browserLabel: "Blur...",
            icon: LucideIcons.Focus,
            description: "Applies a blur effect.",
            createEffect: static () => new BlurImageEffect(),
            parameters:
            [
                new SliderFilterParameterDefinition(
                    key: "radius",
                    label: "Radius",
                    minimum: 1,
                    maximum: 200,
                    defaultValue: 10,
                    tickFrequency: 1,
                    isSnapToTickEnabled: true,
                    valueStringFormat: "{}{0:0}",
                    applyValue: static (effect, value) => ((BlurImageEffect)effect).Radius = (int)value)
            ]),
        new FilterDefinition(
            id: "gaussian_blur",
            name: "Gaussian blur",
            browserLabel: "Gaussian blur...",
            icon: LucideIcons.CircleGauge,
            description: "Applies a Gaussian blur effect.",
            createEffect: static () => new GaussianBlurImageEffect(),
            parameters:
            [
                new SliderFilterParameterDefinition(
                    key: "radius",
                    label: "Radius",
                    minimum: 1,
                    maximum: 200,
                    defaultValue: 15,
                    tickFrequency: 1,
                    isSnapToTickEnabled: true,
                    valueStringFormat: "{}{0:0}",
                    applyValue: static (effect, value) => ((GaussianBlurImageEffect)effect).Radius = (int)value)
            ]),
        new FilterDefinition(
            id: "glow",
            name: "Glow",
            browserLabel: "Glow...",
            icon: LucideIcons.Lightbulb,
            description: "Applies a glowing effect.",
            createEffect: static () => new GlowImageEffect(20, 80f, SKColors.White, 0, 0, autoResize: true),
            parameters:
            [
                new SliderFilterParameterDefinition(
                    key: "size",
                    label: "Size",
                    minimum: 1,
                    maximum: 100,
                    defaultValue: 20,
                    tickFrequency: 1,
                    isSnapToTickEnabled: false,
                    valueStringFormat: "{}{0:0}",
                    applyValue: static (effect, value) => ((GlowImageEffect)effect).Size = (int)value),
                new SliderFilterParameterDefinition(
                    key: "strength",
                    label: "Strength",
                    minimum: 1,
                    maximum: 100,
                    defaultValue: 80,
                    tickFrequency: 1,
                    isSnapToTickEnabled: false,
                    valueStringFormat: "{}{0:0}%",
                    applyValue: static (effect, value) => ((GlowImageEffect)effect).Strength = (float)value),
                new SliderFilterParameterDefinition(
                    key: "offset_x",
                    label: "Offset X",
                    minimum: -100,
                    maximum: 100,
                    defaultValue: 0,
                    tickFrequency: 1,
                    isSnapToTickEnabled: false,
                    valueStringFormat: "{}{0:0}",
                    applyValue: static (effect, value) => ((GlowImageEffect)effect).OffsetX = (int)value),
                new SliderFilterParameterDefinition(
                    key: "offset_y",
                    label: "Offset Y",
                    minimum: -100,
                    maximum: 100,
                    defaultValue: 0,
                    tickFrequency: 1,
                    isSnapToTickEnabled: false,
                    valueStringFormat: "{}{0:0}",
                    applyValue: static (effect, value) => ((GlowImageEffect)effect).OffsetY = (int)value),
                new ColorFilterParameterDefinition(
                    key: "color",
                    label: "Color",
                    defaultValue: Colors.White,
                    applyValue: static (effect, value) => ((GlowImageEffect)effect).Color = ToSkColor(value)),
                new CheckboxFilterParameterDefinition(
                    key: "auto_resize",
                    label: "Auto resize",
                    defaultValue: true,
                    applyValue: static (effect, value) => ((GlowImageEffect)effect).AutoResize = value)
            ]),
        new FilterDefinition(
            id: "dithering",
            name: "Dithering",
            browserLabel: "Dithering...",
            icon: LucideIcons.DotSquare,
            description: "Reduces palette with Floyd-Steinberg or Bayer dot diffusion.",
            createEffect: static () => new DitheringImageEffect(),
            parameters:
            [
                new EnumFilterParameterDefinition(
                    key: "method",
                    label: "Method",
                    options:
                    [
                        new FilterOptionDefinition("Floyd-Steinberg", DitheringMethod.FloydSteinberg),
                        new FilterOptionDefinition("Bayer 4x4", DitheringMethod.Bayer4x4)
                    ],
                    defaultIndex: 0,
                    applyValue: static (effect, value) => ((DitheringImageEffect)effect).Method = value is DitheringMethod method ? method : DitheringMethod.FloydSteinberg),
                new EnumFilterParameterDefinition(
                    key: "palette",
                    label: "Palette",
                    options:
                    [
                        new FilterOptionDefinition("1-bit B&W", DitheringPalette.OneBitBW),
                        new FilterOptionDefinition("Web-safe 216", DitheringPalette.WebSafe216),
                        new FilterOptionDefinition("RGB332", DitheringPalette.RGB332),
                        new FilterOptionDefinition("Grayscale (4 levels)", DitheringPalette.Grayscale4)
                    ],
                    defaultIndex: 0,
                    applyValue: static (effect, value) => ((DitheringImageEffect)effect).Palette = value is DitheringPalette palette ? palette : DitheringPalette.OneBitBW),
                new CheckboxFilterParameterDefinition(
                    key: "serpentine",
                    label: "Serpentine scan (Floyd-Steinberg)",
                    defaultValue: true,
                    applyValue: static (effect, value) => ((DitheringImageEffect)effect).Serpentine = value),
                new SliderFilterParameterDefinition(
                    key: "strength",
                    label: "Strength (%)",
                    minimum: 0,
                    maximum: 100,
                    defaultValue: 100,
                    tickFrequency: 1,
                    isSnapToTickEnabled: true,
                    valueStringFormat: "{}{0:0}",
                    applyValue: static (effect, value) => ((DitheringImageEffect)effect).Strength = (float)value)
            ]),
        new FilterDefinition(
            id: "lens_blur",
            name: "Lens blur (bokeh)",
            browserLabel: "Lens blur (bokeh)...",
            icon: LucideIcons.Target,
            description: "Simulates circular aperture blur with weighted highlight bloom.",
            createEffect: static () => new LensBlurImageEffect(),
            parameters:
            [
                new SliderFilterParameterDefinition(
                    key: "radius",
                    label: "Radius (px)",
                    minimum: 1,
                    maximum: 15,
                    defaultValue: 8,
                    tickFrequency: 1,
                    isSnapToTickEnabled: true,
                    valueStringFormat: "{}{0:0}",
                    applyValue: static (effect, value) => ((LensBlurImageEffect)effect).Radius = (int)Math.Round(value)),
                new SliderFilterParameterDefinition(
                    key: "threshold",
                    label: "Highlight threshold (%)",
                    minimum: 0,
                    maximum: 100,
                    defaultValue: 70,
                    tickFrequency: 1,
                    isSnapToTickEnabled: true,
                    valueStringFormat: "{}{0:0}",
                    applyValue: static (effect, value) => ((LensBlurImageEffect)effect).HighlightThreshold = (float)value),
                new SliderFilterParameterDefinition(
                    key: "boost",
                    label: "Highlight boost (%)",
                    minimum: 0,
                    maximum: 200,
                    defaultValue: 85,
                    tickFrequency: 1,
                    isSnapToTickEnabled: true,
                    valueStringFormat: "{}{0:0}",
                    applyValue: static (effect, value) => ((LensBlurImageEffect)effect).HighlightBoost = (float)value)
            ]),
        new FilterDefinition(
            id: "vignette",
            name: "Vignette",
            browserLabel: "Vignette...",
            icon: LucideIcons.CircleDashed,
            description: "Applies a vignette effect.",
            createEffect: static () => new VignetteImageEffect(),
            parameters:
            [
                new SliderFilterParameterDefinition(
                    key: "strength",
                    label: "Strength",
                    minimum: 0,
                    maximum: 1,
                    defaultValue: 0.5,
                    tickFrequency: 0.01,
                    isSnapToTickEnabled: false,
                    valueStringFormat: "{}{0:0.##}",
                    applyValue: static (effect, value) => ((VignetteImageEffect)effect).Strength = (float)value),
                new SliderFilterParameterDefinition(
                    key: "radius",
                    label: "Radius",
                    minimum: 0.05,
                    maximum: 1,
                    defaultValue: 0.75,
                    tickFrequency: 0.01,
                    isSnapToTickEnabled: false,
                    valueStringFormat: "{}{0:0.##}",
                    applyValue: static (effect, value) => ((VignetteImageEffect)effect).Radius = (float)value)
            ])
    ];

    private static readonly IReadOnlyDictionary<string, FilterDefinition> _definitionsById =
        _definitions.ToDictionary(definition => definition.Id, StringComparer.OrdinalIgnoreCase);

    public static IReadOnlyList<FilterDefinition> Definitions => _definitions;

    public static bool TryGetDefinition(string id, out FilterDefinition? definition)
    {
        return _definitionsById.TryGetValue(id, out definition);
    }

    private static SKColor ToSkColor(Color color) => new(color.R, color.G, color.B, color.A);
}
