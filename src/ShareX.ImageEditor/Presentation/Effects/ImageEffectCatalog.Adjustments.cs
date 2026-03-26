using ShareX.ImageEditor.Core.ImageEffects;
using ShareX.ImageEditor.Core.ImageEffects.Adjustments;
using Avalonia.Media;
using SkiaSharp;

namespace ShareX.ImageEditor.Presentation.Effects;

public static partial class ImageEffectCatalog
{
    private static IReadOnlyList<EffectDefinition> BuildAdjustmentDefinitions()
    {
        return
        [
            Effect<HueImageEffect>("hue", ImageEffectCategory.Adjustments, FloatSlider<HueImageEffect>("amount", "Amount", -180, 180, 0, (e, v) => e.Amount = v)),
            Effect<SaturationImageEffect>("saturation", ImageEffectCategory.Adjustments, FloatSlider<SaturationImageEffect>("amount", "Amount", -100, 100, 0, (e, v) => e.Amount = v)),
            Effect<AlphaImageEffect>("alpha", ImageEffectCategory.Adjustments, FloatSlider<AlphaImageEffect>("amount", "Alpha", 0, 100, 100, (e, v) => e.Amount = v)),
            Effect<ExposureImageEffect>("exposure", ImageEffectCategory.Adjustments, FloatSlider<ExposureImageEffect>("amount", "Exposure", -100, 100, 0, (e, v) => e.Amount = v)),
            Effect<ThresholdImageEffect>("threshold", ImageEffectCategory.Adjustments, IntSlider<ThresholdImageEffect>("value", "Threshold", 0, 255, 128, (e, v) => e.Value = v)),
            Effect<PosterizeImageEffect>("posterize", ImageEffectCategory.Adjustments, IntSlider<PosterizeImageEffect>("levels", "Levels", 2, 64, 8, (e, v) => e.Levels = v)),
            Effect<SolarizeImageEffect>("solarize", ImageEffectCategory.Adjustments, IntSlider<SolarizeImageEffect>("threshold", "Threshold", 0, 255, 128, (e, v) => e.Threshold = v)),
            Effect<VibranceImageEffect>("vibrance", ImageEffectCategory.Adjustments, FloatSlider<VibranceImageEffect>("amount", "Amount", -100, 100, 0, (e, v) => e.Amount = v)),
            Effect<SepiaImageEffect>("sepia", ImageEffectCategory.Adjustments, FloatSlider<SepiaImageEffect>("strength", "Strength", 0, 100, 100, (e, v) => e.Strength = v)),
            Effect<GrayscaleImageEffect>("grayscale", ImageEffectCategory.Adjustments, FloatSlider<GrayscaleImageEffect>("strength", "Strength", 0, 100, 100, (e, v) => e.Strength = v)),
            Effect<ColorizeImageEffect>(
                "colorize", ImageEffectCategory.Adjustments,
                ColorParameter<ColorizeImageEffect>("color", "Color", Colors.Orange, (e, v) => e.Color = new SKColor(v.R, v.G, v.B, v.A)),
                FloatSlider<ColorizeImageEffect>("strength", "Strength", 0, 100, 50, (e, v) => e.Strength = v)),
            Effect<ShadowsHighlightsImageEffect>(
                "shadows_highlights", ImageEffectCategory.Adjustments,
                FloatSlider<ShadowsHighlightsImageEffect>("shadows", "Shadows", -100, 100, 0, (e, v) => e.Shadows = v),
                FloatSlider<ShadowsHighlightsImageEffect>("highlights", "Highlights", -100, 100, 0, (e, v) => e.Highlights = v)),
            Effect<TemperatureTintImageEffect>(
                "temperature_tint", ImageEffectCategory.Adjustments,
                FloatSlider<TemperatureTintImageEffect>("temperature", "Temperature", -100, 100, 0, (e, v) => e.Temperature = v),
                FloatSlider<TemperatureTintImageEffect>("tint", "Tint", -100, 100, 0, (e, v) => e.Tint = v)),
            Effect<LevelsImageEffect>(
                "levels", ImageEffectCategory.Adjustments,
                IntSlider<LevelsImageEffect>("input_black", "Input black", 0, 255, 0, (e, v) => e.InputBlack = v),
                IntSlider<LevelsImageEffect>("input_white", "Input white", 0, 255, 255, (e, v) => e.InputWhite = v),
                FloatSlider<LevelsImageEffect>("gamma", "Gamma", 0.1, 5, 1, (e, v) => e.Gamma = v, tickFrequency: 0.1, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.0}"),
                IntSlider<LevelsImageEffect>("output_black", "Output black", 0, 255, 0, (e, v) => e.OutputBlack = v),
                IntSlider<LevelsImageEffect>("output_white", "Output white", 0, 255, 255, (e, v) => e.OutputWhite = v)),
            BespokeEffect<SelectiveColorImageEffect>("selective_color", ImageEffectCategory.Adjustments, "selective_color"),
            Effect<ReplaceColorImageEffect>(
                "replace_color", ImageEffectCategory.Adjustments,
                ColorParameter<ReplaceColorImageEffect>("target_color", "Target color", Colors.White, (e, v) => e.TargetColor = new SKColor(v.R, v.G, v.B, v.A)),
                ColorParameter<ReplaceColorImageEffect>("replace_color", "Replace color", Colors.Black, (e, v) => e.ReplaceColor = new SKColor(v.R, v.G, v.B, v.A)),
                FloatSlider<ReplaceColorImageEffect>("tolerance", "Tolerance", 0, 255, 40, (e, v) => e.Tolerance = v)),
            Effect<DuotoneGradientMapImageEffect>(
                "duotone_gradient_map", ImageEffectCategory.Adjustments,
                ColorParameter<DuotoneGradientMapImageEffect>("shadow_color", "Shadow color", Color.FromArgb(255, 24, 28, 78), (e, v) => e.ShadowColor = new SKColor(v.R, v.G, v.B, v.A)),
                ColorParameter<DuotoneGradientMapImageEffect>("midtone_color", "Midtone color", Color.FromArgb(255, 182, 60, 132), (e, v) => e.MidtoneColor = new SKColor(v.R, v.G, v.B, v.A)),
                ColorParameter<DuotoneGradientMapImageEffect>("highlight_color", "Highlight color", Color.FromArgb(255, 255, 224, 132), (e, v) => e.HighlightColor = new SKColor(v.R, v.G, v.B, v.A)),
                FloatSlider<DuotoneGradientMapImageEffect>("contrast", "Contrast", 50, 200, 110, (e, v) => e.Contrast = v),
                FloatSlider<DuotoneGradientMapImageEffect>("gamma", "Gamma", 0.1, 5, 1, (e, v) => e.Gamma = v, tickFrequency: 0.1, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.0}"),
                FloatSlider<DuotoneGradientMapImageEffect>("blend", "Blend", 0, 100, 100, (e, v) => e.Blend = v)),
            Effect<ColorMatrixImageEffect>(
                "color_matrix", ImageEffectCategory.Adjustments,
                DoubleNumeric<ColorMatrixImageEffect>("rr", "Rr", -5, 5, 1, (e, v) => e.Rr = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("rg", "Rg", -5, 5, 0, (e, v) => e.Rg = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("rb", "Rb", -5, 5, 0, (e, v) => e.Rb = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("ra", "Ra", -5, 5, 0, (e, v) => e.Ra = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("ro", "Ro", -255, 255, 0, (e, v) => e.Ro = (float)v, increment: 1),
                DoubleNumeric<ColorMatrixImageEffect>("gr", "Gr", -5, 5, 0, (e, v) => e.Gr = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("gg", "Gg", -5, 5, 1, (e, v) => e.Gg = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("gb", "Gb", -5, 5, 0, (e, v) => e.Gb = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("ga", "Ga", -5, 5, 0, (e, v) => e.Ga = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("go", "Go", -255, 255, 0, (e, v) => e.Go = (float)v, increment: 1),
                DoubleNumeric<ColorMatrixImageEffect>("br", "Br", -5, 5, 0, (e, v) => e.Br = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("bg", "Bg", -5, 5, 0, (e, v) => e.Bg = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("bb", "Bb", -5, 5, 1, (e, v) => e.Bb = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("ba", "Ba", -5, 5, 0, (e, v) => e.Ba = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("bo", "Bo", -255, 255, 0, (e, v) => e.Bo = (float)v, increment: 1),
                DoubleNumeric<ColorMatrixImageEffect>("ar", "Ar", -5, 5, 0, (e, v) => e.Ar = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("ag", "Ag", -5, 5, 0, (e, v) => e.Ag = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("ab", "Ab", -5, 5, 0, (e, v) => e.Ab = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("aa", "Aa", -5, 5, 1, (e, v) => e.Aa = (float)v, increment: 0.1),
                DoubleNumeric<ColorMatrixImageEffect>("ao", "Ao", -255, 255, 0, (e, v) => e.Ao = (float)v, increment: 1)),
            Effect<FilmEmulationImageEffect>(
                "film_emulation", ImageEffectCategory.Adjustments,
                EnumParameter<FilmEmulationImageEffect, FilmEmulationImageEffect.FilmEmulationPreset>(
                    "preset", "Preset", FilmEmulationImageEffect.FilmEmulationPreset.Classic, (e, v) => e.Preset = v,
                    ("Classic", FilmEmulationImageEffect.FilmEmulationPreset.Classic),
                    ("Warm", FilmEmulationImageEffect.FilmEmulationPreset.Warm),
                    ("Cool", FilmEmulationImageEffect.FilmEmulationPreset.Cool),
                    ("Faded", FilmEmulationImageEffect.FilmEmulationPreset.Faded),
                    ("Cross processed", FilmEmulationImageEffect.FilmEmulationPreset.CrossProcessed)),
                FloatSlider<FilmEmulationImageEffect>("tone_strength", "Tone strength", 0, 100, 65, (e, v) => e.ToneStrength = v),
                FloatSlider<FilmEmulationImageEffect>("grain_amount", "Grain amount", 0, 100, 12, (e, v) => e.GrainAmount = v),
                FloatSlider<FilmEmulationImageEffect>("fade_amount", "Fade amount", 0, 100, 10, (e, v) => e.FadeAmount = v),
                FloatSlider<FilmEmulationImageEffect>("contrast_amount", "Contrast amount", 50, 150, 110, (e, v) => e.ContrastAmount = v)),
            Effect<AutoContrastImageEffect>("auto_contrast", ImageEffectCategory.Adjustments, FloatSlider<AutoContrastImageEffect>("clip_percent", "Clip percent", 0, 20, 0.5, (e, v) => e.ClipPercent = v, tickFrequency: 0.1, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.0}"))
        ];
    }
}
