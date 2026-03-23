using ShareX.ImageEditor.Core.ImageEffects;
using ShareX.ImageEditor.Core.ImageEffects.Manipulations;
using SkiaSharp;

namespace ShareX.ImageEditor.Presentation.Effects;

public static partial class ImageEffectCatalog
{
    private static IReadOnlyList<EffectDefinition> BuildManipulationDefinitions()
    {
        return
        [
            Effect<SkewImageEffect>(
                "skew", ImageEffectCategory.Manipulations,
                FloatSlider<SkewImageEffect>("horizontally", "Horizontal", -100, 100, 0, (e, v) => e.Horizontally = v),
                FloatSlider<SkewImageEffect>("vertically", "Vertical", -100, 100, 0, (e, v) => e.Vertically = v),
                BoolParameter<SkewImageEffect>("auto_resize", "Auto resize", true, (e, v) => e.AutoResize = v)),
            Effect<PinchBulgeImageEffect>(
                "pinch_bulge", ImageEffectCategory.Manipulations,
                FloatSlider<PinchBulgeImageEffect>("strength", "Strength", -100, 100, 35, (e, v) => e.Strength = v),
                FloatSlider<PinchBulgeImageEffect>("radius_percentage", "Radius", 1, 100, 50, (e, v) => e.RadiusPercentage = v),
                FloatSlider<PinchBulgeImageEffect>("center_x_percentage", "Center X", 0, 100, 50, (e, v) => e.CenterXPercentage = v),
                FloatSlider<PinchBulgeImageEffect>("center_y_percentage", "Center Y", 0, 100, 50, (e, v) => e.CenterYPercentage = v)),
            Effect<TwirlImageEffect>(
                "twirl", ImageEffectCategory.Manipulations,
                FloatSlider<TwirlImageEffect>("angle", "Angle", -360, 360, 90, (e, v) => e.Angle = v),
                FloatSlider<TwirlImageEffect>("radius_percentage", "Radius", 1, 100, 50, (e, v) => e.RadiusPercentage = v),
                FloatSlider<TwirlImageEffect>("center_x_percentage", "Center X", 0, 100, 50, (e, v) => e.CenterXPercentage = v),
                FloatSlider<TwirlImageEffect>("center_y_percentage", "Center Y", 0, 100, 50, (e, v) => e.CenterYPercentage = v)),
            Effect<RoundedCornersImageEffect>("rounded_corners", ImageEffectCategory.Manipulations, IntSlider<RoundedCornersImageEffect>("corner_radius", "Corner radius", 1, 500, 20, (e, v) => e.CornerRadius = v)),
            Effect<ScaleImageEffect>(
                "scale", ImageEffectCategory.Manipulations,
                FloatSlider<ScaleImageEffect>("width_percentage", "Width %", 1, 500, 100, (e, v) => e.WidthPercentage = v),
                FloatSlider<ScaleImageEffect>("height_percentage", "Height %", 0, 500, 0, (e, v) => e.HeightPercentage = v)),
            Effect<FlipImageEffect>(
                "flip", ImageEffectCategory.Manipulations,
                BoolParameter<FlipImageEffect>("horizontally", "Flip horizontally", true, (e, v) => e.Horizontally = v),
                BoolParameter<FlipImageEffect>("vertically", "Flip vertically", false, (e, v) => e.Vertically = v)),
            Effect<Rotate3DImageEffect>(
                "rotate_3d", ImageEffectCategory.Manipulations,
                FloatSlider<Rotate3DImageEffect>("rotate_x", "Rotate X", -180, 180, 0, (e, v) => e.RotateX = v),
                FloatSlider<Rotate3DImageEffect>("rotate_y", "Rotate Y", -180, 180, 0, (e, v) => e.RotateY = v),
                FloatSlider<Rotate3DImageEffect>("rotate_z", "Rotate Z", -180, 180, 0, (e, v) => e.RotateZ = v),
                BoolParameter<Rotate3DImageEffect>("auto_resize", "Auto resize", true, (e, v) => e.AutoResize = v)),
            Effect<Rotate3DBoxImageEffect>(
                "rotate_3d_box", ImageEffectCategory.Manipulations,
                FloatSlider<Rotate3DBoxImageEffect>("depth", "Depth", 0, 500, 75, (e, v) => e.Depth = v),
                FloatSlider<Rotate3DBoxImageEffect>("rotate_x", "Rotate X", -180, 180, 0, (e, v) => e.RotateX = v),
                FloatSlider<Rotate3DBoxImageEffect>("rotate_y", "Rotate Y", -180, 180, 0, (e, v) => e.RotateY = v),
                FloatSlider<Rotate3DBoxImageEffect>("rotate_z", "Rotate Z", -180, 180, 0, (e, v) => e.RotateZ = v),
                BoolParameter<Rotate3DBoxImageEffect>("auto_resize", "Auto resize", true, (e, v) => e.AutoResize = v)),
            Effect("auto_crop_image", ImageEffectCategory.Manipulations, static () => new AutoCropImageEffect(SKColors.Transparent, 0), IntSlider<AutoCropImageEffect>("tolerance", "Tolerance", 0, 255, 0, (_, _) => { })),
            Effect<DisplacementMapImageEffect>(
                "displacement_map", ImageEffectCategory.Manipulations,
                FloatSlider<DisplacementMapImageEffect>("amount_x", "Amount X", -200, 200, 20, (e, v) => e.AmountX = v),
                FloatSlider<DisplacementMapImageEffect>("amount_y", "Amount Y", -200, 200, 20, (e, v) => e.AmountY = v)),
            BespokeEffect<PerspectiveWarpImageEffect>("perspective_warp", ImageEffectCategory.Manipulations, "perspective_warp"),
            BespokeEffect<ResizeImageEffect>("resize_image", ImageEffectCategory.Manipulations, "resize_image"),
            BespokeEffect<ResizeImageEffect>("resize_canvas", ImageEffectCategory.Manipulations, "resize_canvas"),
            BespokeEffect<ResizeImageEffect>("crop_image", ImageEffectCategory.Manipulations, "crop_image")
        ];
    }
}
