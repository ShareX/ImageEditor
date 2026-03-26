using ShareX.ImageEditor.Core.ImageEffects;
using ShareX.ImageEditor.Core.ImageEffects.Manipulations;
using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Presentation.Effects;

internal static class EditorOperationCatalog
{
    private static readonly IReadOnlyList<EditorOperationDefinition> _definitions =
    [
        new(
            "auto_crop_image",
            "Auto crop image...",
            LucideIcons.Scan,
            "Automatically crops the image using tolerance on edge pixels.",
            ImageEffectCategory.Manipulations,
            EditorOperationKind.AutoCropImage,
            new EffectDefinition(
                "auto_crop_image",
                "Auto crop image",
                "Auto crop image...",
                LucideIcons.Scan,
                "Automatically crops the image using tolerance on edge pixels.",
                ImageEffectCategory.Manipulations,
                static () => new AutoCropImageEffect(SKColors.Transparent, 0),
                [],
                [
                    EffectParameters.IntSlider<AutoCropImageEffect>("tolerance", "Tolerance", 0, 255, 0, (effect, value) => effect.Tolerance = value)
                ])),
        new("crop_image", "Crop image...", LucideIcons.Crop, "Crops the image.", ImageEffectCategory.Manipulations, EditorOperationKind.CropImage),
        new("resize_image", "Resize image...", LucideIcons.ImageUpscale, "Resizes the image.", ImageEffectCategory.Manipulations, EditorOperationKind.ResizeImage),
        new("resize_canvas", "Resize canvas...", LucideIcons.Maximize, "Resizes the canvas.", ImageEffectCategory.Manipulations, EditorOperationKind.ResizeCanvas),
        new("rotate_90_clockwise", "Rotate 90° clockwise", LucideIcons.Redo2, "Rotates the image 90 degrees clockwise.", ImageEffectCategory.Manipulations, EditorOperationKind.Rotate90Clockwise),
        new("rotate_90_counter_clockwise", "Rotate 90° counter clockwise", LucideIcons.Undo2, "Rotates the image 90 degrees counter-clockwise.", ImageEffectCategory.Manipulations, EditorOperationKind.Rotate90CounterClockwise),
        new("rotate_180", "Rotate 180°", LucideIcons.RotateCwSquare, "Rotates the image by 180 degrees.", ImageEffectCategory.Manipulations, EditorOperationKind.Rotate180),
        new("rotate_custom_angle", "Rotate...", LucideIcons.RotateCw, "Rotates the image by a custom angle.", ImageEffectCategory.Manipulations, EditorOperationKind.RotateCustomAngle),
        new("flip_horizontal", "Flip horizontal", LucideIcons.FlipHorizontal, "Flips the image horizontally.", ImageEffectCategory.Manipulations, EditorOperationKind.FlipHorizontal),
        new("flip_vertical", "Flip vertical", LucideIcons.FlipVertical, "Flips the image vertically.", ImageEffectCategory.Manipulations, EditorOperationKind.FlipVertical)
    ];

    private static readonly IReadOnlyDictionary<string, EditorOperationDefinition> _definitionsById =
        _definitions.ToDictionary(definition => definition.Id, StringComparer.OrdinalIgnoreCase);

    private static readonly IReadOnlyDictionary<ImageEffectCategory, IReadOnlyList<EditorOperationDefinition>> _definitionsByCategory =
        _definitions
            .GroupBy(definition => definition.Category)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<EditorOperationDefinition>)group.ToArray());

    public static bool TryGetDefinition(string id, out EditorOperationDefinition? definition)
    {
        return _definitionsById.TryGetValue(id, out definition);
    }

    public static IReadOnlyList<EditorOperationDefinition> GetByCategory(ImageEffectCategory category)
    {
        return _definitionsByCategory.TryGetValue(category, out IReadOnlyList<EditorOperationDefinition>? definitions)
            ? definitions
            : [];
    }
}
