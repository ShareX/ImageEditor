using ShareX.ImageEditor.Core.ImageEffects;
using ShareX.ImageEditor.Core.ImageEffects.Filters;
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
            Effect<CylinderWrapImageEffect>(
                "cylinder_wrap", ImageEffectCategory.Manipulations,
                EnumParameter<CylinderWrapImageEffect, CylinderWrapOrientation>(
                    "orientation", "Orientation", CylinderWrapOrientation.Vertical, (e, v) => e.Orientation = v,
                    ("Vertical", CylinderWrapOrientation.Vertical), ("Horizontal", CylinderWrapOrientation.Horizontal)),
                FloatSlider<CylinderWrapImageEffect>("curvature", "Curvature (%)", 0, 100, 65, (e, v) => e.Curvature = v),
                FloatSlider<CylinderWrapImageEffect>("edge_shading", "Edge shading (%)", 0, 100, 35, (e, v) => e.EdgeShading = v)),
            Effect<FisheyeLensImageEffect>(
                "fisheye_lens", ImageEffectCategory.Manipulations,
                FloatSlider<FisheyeLensImageEffect>("strength", "Strength", -100, 100, 58, (e, v) => e.Strength = v),
                FloatSlider<FisheyeLensImageEffect>("radius_percentage", "Radius (%)", 1, 100, 100, (e, v) => e.RadiusPercentage = v),
                FloatSlider<FisheyeLensImageEffect>("center_x_percentage", "Center X (%)", 0, 100, 50, (e, v) => e.CenterXPercentage = v),
                FloatSlider<FisheyeLensImageEffect>("center_y_percentage", "Center Y (%)", 0, 100, 50, (e, v) => e.CenterYPercentage = v)),
            Effect<FoldCreaseWarpImageEffect>(
                "fold_crease_warp", ImageEffectCategory.Manipulations,
                EnumParameter<FoldCreaseWarpImageEffect, FoldCreaseOrientation>(
                    "orientation", "Orientation", FoldCreaseOrientation.Vertical, (e, v) => e.Orientation = v,
                    ("Vertical", FoldCreaseOrientation.Vertical), ("Horizontal", FoldCreaseOrientation.Horizontal)),
                IntSlider<FoldCreaseWarpImageEffect>("fold_count", "Fold count", 1, 12, 3, (e, v) => e.FoldCount = v),
                FloatSlider<FoldCreaseWarpImageEffect>("fold_depth", "Fold depth (%)", 0, 100, 30, (e, v) => e.FoldDepth = v),
                FloatSlider<FoldCreaseWarpImageEffect>("shadow_strength", "Shadow strength (%)", 0, 100, 40, (e, v) => e.ShadowStrength = v)),
            Effect<KaleidoscopeImageEffect>(
                "kaleidoscope", ImageEffectCategory.Manipulations,
                IntSlider<KaleidoscopeImageEffect>("segments", "Segments", 2, 32, 8, (e, v) => e.Segments = v),
                FloatSlider<KaleidoscopeImageEffect>("rotation", "Rotation (°)", -180, 180, 0, (e, v) => e.Rotation = v),
                FloatSlider<KaleidoscopeImageEffect>("zoom", "Zoom (%)", 10, 300, 100, (e, v) => e.Zoom = v),
                FloatSlider<KaleidoscopeImageEffect>("center_x_percentage", "Center X (%)", 0, 100, 50, (e, v) => e.CenterXPercentage = v),
                FloatSlider<KaleidoscopeImageEffect>("center_y_percentage", "Center Y (%)", 0, 100, 50, (e, v) => e.CenterYPercentage = v)),
            Effect<LiquifyPushSmudgeImageEffect>(
                "liquify_push_smudge", ImageEffectCategory.Manipulations,
                FloatSlider<LiquifyPushSmudgeImageEffect>("angle", "Angle (°)", -180, 180, 0, (e, v) => e.Angle = v),
                FloatSlider<LiquifyPushSmudgeImageEffect>("distance", "Distance", 0, 200, 40, (e, v) => e.Distance = v),
                FloatSlider<LiquifyPushSmudgeImageEffect>("radius_percentage", "Radius (%)", 1, 100, 25, (e, v) => e.RadiusPercentage = v),
                FloatSlider<LiquifyPushSmudgeImageEffect>("smudge", "Smudge (%)", 0, 100, 35, (e, v) => e.Smudge = v),
                FloatSlider<LiquifyPushSmudgeImageEffect>("center_x_percentage", "Center X (%)", 0, 100, 50, (e, v) => e.CenterXPercentage = v),
                FloatSlider<LiquifyPushSmudgeImageEffect>("center_y_percentage", "Center Y (%)", 0, 100, 50, (e, v) => e.CenterYPercentage = v)),
            Effect<MirrorTilesImageEffect>(
                "mirror_tiles", ImageEffectCategory.Manipulations,
                IntSlider<MirrorTilesImageEffect>("columns", "Columns", 1, 16, 3, (e, v) => e.Columns = v),
                IntSlider<MirrorTilesImageEffect>("rows", "Rows", 1, 16, 3, (e, v) => e.Rows = v),
                BoolParameter<MirrorTilesImageEffect>("mirror_alternate_columns", "Mirror alternate columns", true, (e, v) => e.MirrorAlternateColumns = v),
                BoolParameter<MirrorTilesImageEffect>("mirror_alternate_rows", "Mirror alternate rows", true, (e, v) => e.MirrorAlternateRows = v)),
            Effect<PageCurlImageEffect>(
                "page_curl", ImageEffectCategory.Manipulations,
                EnumParameter<PageCurlImageEffect, PageCurlCorner>(
                    "corner", "Corner", PageCurlCorner.BottomRight, (e, v) => e.Corner = v,
                    ("Top-left", PageCurlCorner.TopLeft), ("Top-right", PageCurlCorner.TopRight),
                    ("Bottom-left", PageCurlCorner.BottomLeft), ("Bottom-right", PageCurlCorner.BottomRight)),
                FloatSlider<PageCurlImageEffect>("curl_size", "Curl size (%)", 0, 100, 28, (e, v) => e.CurlSize = v),
                FloatSlider<PageCurlImageEffect>("curl_depth", "Curl depth (%)", 0, 100, 55, (e, v) => e.CurlDepth = v),
                FloatSlider<PageCurlImageEffect>("shadow_strength", "Shadow strength (%)", 0, 100, 60, (e, v) => e.ShadowStrength = v),
                ColorParameter<PageCurlImageEffect>("back_color", "Back color", Argb(255, 248, 244, 236), (e, v) => e.BackColor = ToSkColor(v))),
            Effect<PolarWarpImageEffect>(
                "polar_warp", ImageEffectCategory.Manipulations,
                EnumParameter<PolarWarpImageEffect, PolarWarpMode>(
                    "mode", "Mode", PolarWarpMode.CartesianToPolar, (e, v) => e.Mode = v,
                    ("Cartesian to polar", PolarWarpMode.CartesianToPolar), ("Polar to cartesian", PolarWarpMode.PolarToCartesian)),
                FloatSlider<PolarWarpImageEffect>("rotation", "Rotation (°)", -180, 180, 0, (e, v) => e.Rotation = v),
                FloatSlider<PolarWarpImageEffect>("radius_scale", "Radius scale (%)", 10, 200, 100, (e, v) => e.RadiusScale = v),
                FloatSlider<PolarWarpImageEffect>("center_x_percentage", "Center X (%)", 0, 100, 50, (e, v) => e.CenterXPercentage = v),
                FloatSlider<PolarWarpImageEffect>("center_y_percentage", "Center Y (%)", 0, 100, 50, (e, v) => e.CenterYPercentage = v)),
            Effect<RippleRefractionImageEffect>(
                "ripple_refraction", ImageEffectCategory.Manipulations,
                FloatSlider<RippleRefractionImageEffect>("amplitude", "Amplitude", 0, 100, 12, (e, v) => e.Amplitude = v),
                FloatSlider<RippleRefractionImageEffect>("wavelength", "Wavelength", 1, 200, 32, (e, v) => e.Wavelength = v),
                FloatSlider<RippleRefractionImageEffect>("phase", "Phase (°)", -180, 180, 0, (e, v) => e.Phase = v),
                FloatSlider<RippleRefractionImageEffect>("refraction", "Refraction (%)", 0, 100, 35, (e, v) => e.Refraction = v),
                FloatSlider<RippleRefractionImageEffect>("center_x_percentage", "Center X (%)", 0, 100, 50, (e, v) => e.CenterXPercentage = v),
                FloatSlider<RippleRefractionImageEffect>("center_y_percentage", "Center Y (%)", 0, 100, 50, (e, v) => e.CenterYPercentage = v)),
            Effect<RemoveBackgroundImageEffect>(
                "remove_background", ImageEffectCategory.Manipulations,
                FloatSlider<RemoveBackgroundImageEffect>("sensitivity", "Sensitivity (%)", 0, 100, 60, (e, v) => e.Sensitivity = v),
                FloatSlider<RemoveBackgroundImageEffect>("center_protection", "Center protection (%)", 0, 100, 65, (e, v) => e.CenterProtection = v),
                FloatSlider<RemoveBackgroundImageEffect>("edge_feather", "Edge feather (px)", 0, 24, 4, (e, v) => e.EdgeFeather = v, tickFrequency: 0.5, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.#}")),
            BespokeEffect<PerspectiveWarpImageEffect>("perspective_warp", ImageEffectCategory.Manipulations, "perspective_warp"),
            BespokeEffect<ResizeImageEffect>("resize_image", ImageEffectCategory.Manipulations, "resize_image"),
            BespokeEffect<ResizeImageEffect>("resize_canvas", ImageEffectCategory.Manipulations, "resize_canvas"),
            BespokeEffect<ResizeImageEffect>("crop_image", ImageEffectCategory.Manipulations, "crop_image")
        ];
    }
}
