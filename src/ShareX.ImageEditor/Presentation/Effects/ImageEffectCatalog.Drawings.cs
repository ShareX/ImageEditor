using Avalonia.Media;
using ShareX.ImageEditor.Core.ImageEffects;
using ShareX.ImageEditor.Core.ImageEffects.Drawings;
using ShareX.ImageEditor.Core.ImageEffects.Filters;
using ShareX.ImageEditor.Core.ImageEffects.Helpers;

namespace ShareX.ImageEditor.Presentation.Effects;

public static partial class ImageEffectCatalog
{
    private static IReadOnlyList<EffectDefinition> BuildDrawingDefinitions()
    {
        return
        [
            Effect<DrawBackgroundEffect>(
                "draw_background", ImageEffectCategory.Drawings,
                ColorParameter<DrawBackgroundEffect>("color", "Color", Colors.Black, (e, v) => e.Color = ToSkColor(v))),
            Effect<DrawCheckerboardEffect>(
                "draw_checkerboard", ImageEffectCategory.Drawings,
                IntSlider<DrawCheckerboardEffect>("size", "Size", 1, 200, 10, (e, v) => e.Size = v),
                ColorParameter<DrawCheckerboardEffect>("color", "Color 1", Color.FromRgb(211, 211, 211), (e, v) => e.Color = ToSkColor(v)),
                ColorParameter<DrawCheckerboardEffect>("color2", "Color 2", Colors.White, (e, v) => e.Color2 = ToSkColor(v))),
            Effect<DrawBackgroundImageEffect>(
                "draw_background_image", ImageEffectCategory.Drawings,
                FilePathParameter<DrawBackgroundImageEffect>("image_file_path", "Image file", "", (e, v) => e.ImageFilePath = v, "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.webp"),
                BoolParameter<DrawBackgroundImageEffect>("center", "Center", true, (e, v) => e.Center = v),
                BoolParameter<DrawBackgroundImageEffect>("tile", "Tile", false, (e, v) => e.Tile = v)),
            Effect<DrawImageEffect>(
                "draw_image", ImageEffectCategory.Drawings,
                FilePathParameter<DrawImageEffect>("image_location", "Image file", "", (e, v) => e.ImageLocation = v, "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.webp"),
                EnumParameter<DrawImageEffect, DrawingPlacement>(
                    "placement", "Placement", DrawingPlacement.TopLeft, (e, v) => e.Placement = v,
                    ("Top left", DrawingPlacement.TopLeft), ("Top center", DrawingPlacement.TopCenter), ("Top right", DrawingPlacement.TopRight),
                    ("Middle left", DrawingPlacement.MiddleLeft), ("Middle center", DrawingPlacement.MiddleCenter), ("Middle right", DrawingPlacement.MiddleRight),
                    ("Bottom left", DrawingPlacement.BottomLeft), ("Bottom center", DrawingPlacement.BottomCenter), ("Bottom right", DrawingPlacement.BottomRight)),
                IntSlider<DrawImageEffect>("opacity", "Opacity", 0, 100, 100, (e, v) => e.Opacity = v),
                BoolParameter<DrawImageEffect>("tile", "Tile", false, (e, v) => e.Tile = v)),
            BespokeEffect<DrawLineEffect>("draw_line", ImageEffectCategory.Drawings, "draw_line"),
            BespokeEffect<DrawParticlesEffect>("draw_particles", ImageEffectCategory.Drawings, "draw_particles"),
            BespokeEffect<DrawShapeEffect>("draw_shape", ImageEffectCategory.Drawings, "draw_shape"),
            BespokeEffect<DrawTextEffect>("draw_text", ImageEffectCategory.Drawings, "draw_text"),
            BespokeEffect<TextWatermarkEffect>("text_watermark", ImageEffectCategory.Drawings, "text_watermark")
        ];
    }
}
