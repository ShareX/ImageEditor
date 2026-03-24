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
using ShareX.ImageEditor.Core.ImageEffects;
using ShareX.ImageEditor.Core.ImageEffects.Helpers;

namespace ShareX.ImageEditor.Presentation.Effects;

public static partial class ImageEffectCatalog
{
    private static IReadOnlyList<EffectDefinition> BuildFilterDefinitions()
    {
        return
        [
            Effect<AddNoiseImageEffect>(
                "add_noise",
                ImageEffectCategory.Filters,
                FloatSlider<AddNoiseImageEffect>("amount", "Amount", 0, 100, 8, (effect, value) => effect.Amount = value)),

            Effect<AnimeSpeedLinesImageEffect>(
                "anime_speed_lines",
                ImageEffectCategory.Filters,
                FloatSlider<AnimeSpeedLinesImageEffect>("density", "Density (%)", 10, 100, 70, (effect, value) => effect.Density = value),
                FloatSlider<AnimeSpeedLinesImageEffect>("strength", "Strength (%)", 0, 100, 65, (effect, value) => effect.Strength = value),
                FloatSlider<AnimeSpeedLinesImageEffect>("focus_radius", "Focus radius (%)", 0, 80, 18, (effect, value) => effect.FocusRadius = value),
                FloatSlider<AnimeSpeedLinesImageEffect>("center_x", "Center X (%)", 0, 100, 50, (effect, value) => effect.CenterX = value),
                FloatSlider<AnimeSpeedLinesImageEffect>("center_y", "Center Y (%)", 0, 100, 50, (effect, value) => effect.CenterY = value),
                FloatSlider<AnimeSpeedLinesImageEffect>("contrast", "Contrast (%)", 0, 100, 35, (effect, value) => effect.Contrast = value)),

            Effect<ASCIIArtImageEffect>(
                "ascii_art",
                ImageEffectCategory.Filters,
                IntSlider<ASCIIArtImageEffect>("cell_size", "Character size (px)", 4, 24, 8, (effect, value) => effect.CellSize = value),
                FloatSlider<ASCIIArtImageEffect>("contrast", "Contrast (%)", 50, 200, 110, (effect, value) => effect.Contrast = value),
                BoolParameter<ASCIIArtImageEffect>("invert", "Invert luminance", false, (effect, value) => effect.Invert = value),
                BoolParameter<ASCIIArtImageEffect>("dark_background", "Dark background", true, (effect, value) => effect.DarkBackground = value),
                BoolParameter<ASCIIArtImageEffect>("use_source_color", "Use source colors", true, (effect, value) => effect.UseSourceColor = value),
                TextParameter<ASCIIArtImageEffect>("character_set", "Character set", "@%#*+=-:. ", (effect, value) => effect.CharacterSet = value)),

            Effect<BevelImageEffect>(
                "bevel",
                ImageEffectCategory.Filters,
                IntSlider<BevelImageEffect>("size", "Size", 1, 40, 10, (effect, value) => effect.Size = value),
                FloatSlider<BevelImageEffect>("strength", "Strength (%)", 0, 100, 70, (effect, value) => effect.Strength = value),
                FloatSlider<BevelImageEffect>("light_angle", "Light angle", 0, 360, 225, (effect, value) => effect.LightAngle = value, valueStringFormat: "{}{0:0}°"),
                ColorParameter<BevelImageEffect>("highlight_color", "Highlight color", Argb(217, 255, 255, 255), (effect, value) => effect.HighlightColor = ToSkColor(value)),
                ColorParameter<BevelImageEffect>("shadow_color", "Shadow color", Argb(176, 0, 0, 0), (effect, value) => effect.ShadowColor = ToSkColor(value))),

            Effect<BloodSplashImageEffect>(
                "blood_splash",
                ImageEffectCategory.Filters,
                FloatSlider<BloodSplashImageEffect>("splash_amount", "Splash amount (%)", 0, 100, 52, (effect, value) => effect.SplashAmount = value),
                FloatSlider<BloodSplashImageEffect>("drip_length", "Drip length (%)", 0, 100, 48, (effect, value) => effect.DripLength = value),
                FloatSlider<BloodSplashImageEffect>("spread", "Spread (%)", 0, 100, 42, (effect, value) => effect.Spread = value),
                FloatSlider<BloodSplashImageEffect>("darkness", "Darkness (%)", 0, 100, 58, (effect, value) => effect.Darkness = value),
                FloatSlider<BloodSplashImageEffect>("wet_shine", "Wet shine (%)", 0, 100, 30, (effect, value) => effect.WetShine = value)),

            Effect<BlockGlitchImageEffect>(
                "block_glitch",
                ImageEffectCategory.Filters,
                IntSlider<BlockGlitchImageEffect>("block_count", "Block count", 1, 240, 36, (effect, value) => effect.BlockCount = value),
                IntSlider<BlockGlitchImageEffect>("min_block_width", "Min block width (px)", 4, 400, 24, (effect, value) => effect.MinBlockWidth = value),
                IntSlider<BlockGlitchImageEffect>("max_block_width", "Max block width (px)", 4, 900, 200, (effect, value) => effect.MaxBlockWidth = value),
                IntSlider<BlockGlitchImageEffect>("min_block_height", "Min block height (px)", 2, 200, 6, (effect, value) => effect.MinBlockHeight = value),
                IntSlider<BlockGlitchImageEffect>("max_block_height", "Max block height (px)", 2, 500, 50, (effect, value) => effect.MaxBlockHeight = value),
                IntSlider<BlockGlitchImageEffect>("max_displacement", "Max displacement (px)", 0, 500, 50, (effect, value) => effect.MaxDisplacement = value),
                IntSlider<BlockGlitchImageEffect>("channel_shift", "Channel shift (px)", 0, 64, 4, (effect, value) => effect.ChannelShift = value),
                FloatSlider<BlockGlitchImageEffect>("noise_amount", "Noise (%)", 0, 100, 10, (effect, value) => effect.NoiseAmount = value)),

            Effect<BloomImageEffect>(
                "bloom",
                ImageEffectCategory.Filters,
                FloatSlider<BloomImageEffect>("threshold", "Threshold (%)", 0, 100, 65, (effect, value) => effect.Threshold = value),
                FloatSlider<BloomImageEffect>("soft_knee", "Soft knee (%)", 0, 100, 35, (effect, value) => effect.SoftKnee = value),
                FloatSlider<BloomImageEffect>("radius", "Radius (px)", 1, 100, 24, (effect, value) => effect.Radius = value),
                FloatSlider<BloomImageEffect>("intensity", "Intensity (%)", 0, 200, 85, (effect, value) => effect.Intensity = value)),

            Effect<BlueprintDrawingImageEffect>(
                "blueprint_drawing",
                ImageEffectCategory.Filters,
                FloatSlider<BlueprintDrawingImageEffect>("line_strength", "Line strength (%)", 0, 100, 75, (effect, value) => effect.LineStrength = value),
                FloatSlider<BlueprintDrawingImageEffect>("detail", "Detail (%)", 0, 100, 45, (effect, value) => effect.Detail = value),
                FloatSlider<BlueprintDrawingImageEffect>("grid_intensity", "Grid intensity (%)", 0, 100, 30, (effect, value) => effect.GridIntensity = value),
                FloatSlider<BlueprintDrawingImageEffect>("texture", "Paper texture (%)", 0, 100, 25, (effect, value) => effect.Texture = value),
                FloatSlider<BlueprintDrawingImageEffect>("glow", "Cyan glow (%)", 0, 100, 35, (effect, value) => effect.Glow = value)),

            Effect<BlurImageEffect>(
                "blur",
                ImageEffectCategory.Filters,
                IntSlider<BlurImageEffect>("radius", "Radius", 1, 200, 10, (effect, value) => effect.Radius = value)),

            Effect(
                "border",
                ImageEffectCategory.Filters,
                static () => new BorderImageEffect(ImageHelpers.BorderType.Outside, 5, ImageHelpers.DashStyle.Solid, ToSkColor(Colors.Black)),
                EnumParameter<BorderImageEffect, ImageHelpers.BorderType>(
                    "type",
                    "Type",
                    ImageHelpers.BorderType.Outside,
                    (effect, value) => effect.Type = value,
                    ("Outside", ImageHelpers.BorderType.Outside),
                    ("Inside", ImageHelpers.BorderType.Inside)),
                IntSlider<BorderImageEffect>("size", "Size", 1, 100, 5, (effect, value) => effect.Size = value),
                EnumParameter<BorderImageEffect, ImageHelpers.DashStyle>(
                    "dash_style",
                    "Dash style",
                    ImageHelpers.DashStyle.Solid,
                    (effect, value) => effect.DashStyle = value,
                    ("Solid", ImageHelpers.DashStyle.Solid),
                    ("Dash", ImageHelpers.DashStyle.Dash),
                    ("Dot", ImageHelpers.DashStyle.Dot),
                    ("Dash Dot", ImageHelpers.DashStyle.DashDot)),
                ColorParameter<BorderImageEffect>("color", "Color", Colors.Black, (effect, value) => effect.Color = ToSkColor(value))),

            Effect<CartoonStickerCutoutImageEffect>(
                "cartoon_sticker_cutout",
                ImageEffectCategory.Filters,
                IntSlider<CartoonStickerCutoutImageEffect>("color_levels", "Color levels", 2, 20, 8, (effect, value) => effect.ColorLevels = value),
                IntSlider<CartoonStickerCutoutImageEffect>("edge_threshold", "Edge threshold", 5, 120, 32, (effect, value) => effect.EdgeThreshold = value),
                FloatSlider<CartoonStickerCutoutImageEffect>("ink_strength", "Ink strength (%)", 0, 100, 85, (effect, value) => effect.InkStrength = value),
                IntSlider<CartoonStickerCutoutImageEffect>("sticker_border", "Sticker border (px)", 0, 10, 3, (effect, value) => effect.StickerBorder = value),
                FloatSlider<CartoonStickerCutoutImageEffect>("border_strength", "Border strength (%)", 0, 100, 90, (effect, value) => effect.BorderStrength = value)),

            Effect<ChromaticAberrationImageEffect>(
                "chromatic_aberration",
                ImageEffectCategory.Filters,
                FloatSlider<ChromaticAberrationImageEffect>("amount", "Amount (px)", 0, 40, 8, (effect, value) => effect.Amount = value),
                FloatSlider<ChromaticAberrationImageEffect>("coverage", "Coverage (%)", 0, 100, 80, (effect, value) => effect.EdgeStart = 1f - (value / 100f)),
                FloatSlider<ChromaticAberrationImageEffect>("strength", "Strength (%)", 0, 100, 75, (effect, value) => effect.Strength = value)),

            Effect<ClaymationTextureImageEffect>(
                "claymation_texture",
                ImageEffectCategory.Filters,
                IntSlider<ClaymationTextureImageEffect>("chunk_size", "Chunk size", 3, 18, 8, (effect, value) => effect.ChunkSize = value),
                FloatSlider<ClaymationTextureImageEffect>("smoothness", "Smoothness (%)", 0, 100, 55, (effect, value) => effect.Smoothness = value),
                FloatSlider<ClaymationTextureImageEffect>("relief", "Relief (%)", 0, 100, 35, (effect, value) => effect.Relief = value),
                FloatSlider<ClaymationTextureImageEffect>("saturation", "Saturation (%)", -100, 100, 20, (effect, value) => effect.Saturation = value),
                FloatSlider<ClaymationTextureImageEffect>("texture_grain", "Texture grain (%)", 0, 100, 30, (effect, value) => effect.TextureGrain = value)),

            Effect<ColorDepthImageEffect>(
                "color_depth",
                ImageEffectCategory.Filters,
                IntSlider<ColorDepthImageEffect>("bits_per_channel", "Bits per channel", 1, 8, 4, (effect, value) => effect.BitsPerChannel = value)),

            Effect<ConvolutionMatrixImageEffect>(
                "convolution_matrix",
                ImageEffectCategory.Filters,
                IntNumeric<ConvolutionMatrixImageEffect>("x0_y0", "X0Y0", -100, 100, 0, (effect, value) => effect.X0Y0 = value),
                IntNumeric<ConvolutionMatrixImageEffect>("x1_y0", "X1Y0", -100, 100, 0, (effect, value) => effect.X1Y0 = value),
                IntNumeric<ConvolutionMatrixImageEffect>("x2_y0", "X2Y0", -100, 100, 0, (effect, value) => effect.X2Y0 = value),
                IntNumeric<ConvolutionMatrixImageEffect>("x0_y1", "X0Y1", -100, 100, 0, (effect, value) => effect.X0Y1 = value),
                IntNumeric<ConvolutionMatrixImageEffect>("x1_y1", "X1Y1", -100, 100, 1, (effect, value) => effect.X1Y1 = value),
                IntNumeric<ConvolutionMatrixImageEffect>("x2_y1", "X2Y1", -100, 100, 0, (effect, value) => effect.X2Y1 = value),
                IntNumeric<ConvolutionMatrixImageEffect>("x0_y2", "X0Y2", -100, 100, 0, (effect, value) => effect.X0Y2 = value),
                IntNumeric<ConvolutionMatrixImageEffect>("x1_y2", "X1Y2", -100, 100, 0, (effect, value) => effect.X1Y2 = value),
                IntNumeric<ConvolutionMatrixImageEffect>("x2_y2", "X2Y2", -100, 100, 0, (effect, value) => effect.X2Y2 = value),
                DoubleNumeric<ConvolutionMatrixImageEffect>("factor", "Factor", 0.01, 1000, 1, (effect, value) => effect.Factor = value, increment: 0.1, formatString: "0.##"),
                IntNumeric<ConvolutionMatrixImageEffect>("offset", "Offset", -255, 255, 0, (effect, value) => effect.Offset = value)),

            Effect<CrosshatchImageEffect>(
                "crosshatch",
                ImageEffectCategory.Filters,
                IntSlider<CrosshatchImageEffect>("line_spacing", "Line spacing (px)", 4, 24, 8, (effect, value) => effect.LineSpacing = value),
                FloatSlider<CrosshatchImageEffect>("line_thickness", "Line thickness (px)", 0.5, 4, 1.2, (effect, value) => effect.LineThickness = value, tickFrequency: 0.1, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.0}"),
                FloatSlider<CrosshatchImageEffect>("contrast", "Contrast (%)", 50, 200, 120, (effect, value) => effect.Contrast = value),
                IntSlider<CrosshatchImageEffect>("layer_count", "Layers", 1, 6, 4, (effect, value) => effect.LayerCount = value)),

            Effect<CRTImageEffect>(
                "crt",
                ImageEffectCategory.Filters,
                FloatSlider<CRTImageEffect>("scanline_strength", "Scanlines (%)", 0, 100, 35, (effect, value) => effect.ScanlineStrength = value),
                IntSlider<CRTImageEffect>("rgb_shift", "RGB shift (px)", 0, 8, 1, (effect, value) => effect.RGBShift = value),
                FloatSlider<CRTImageEffect>("noise_amount", "Noise (%)", 0, 100, 8, (effect, value) => effect.NoiseAmount = value),
                FloatSlider<CRTImageEffect>("vignette_strength", "Vignette (%)", 0, 100, 18, (effect, value) => effect.VignetteStrength = value)),

            Effect<CrystalPrismImageEffect>(
                "crystal_prism",
                ImageEffectCategory.Filters,
                IntSlider<CrystalPrismImageEffect>("facet_size", "Facet size (px)", 6, 96, 24, (effect, value) => effect.FacetSize = value),
                FloatSlider<CrystalPrismImageEffect>("refraction", "Refraction", 0, 30, 8, (effect, value) => effect.Refraction = value),
                FloatSlider<CrystalPrismImageEffect>("dispersion", "Dispersion", 0, 20, 4, (effect, value) => effect.Dispersion = value),
                FloatSlider<CrystalPrismImageEffect>("sparkle", "Sparkle (%)", 0, 100, 25, (effect, value) => effect.Sparkle = value)),

            Effect<CrystalizeShardsImageEffect>(
                "crystalize_shards",
                ImageEffectCategory.Filters,
                IntSlider<CrystalizeShardsImageEffect>("shard_size", "Shard size (px)", 6, 80, 22, (effect, value) => effect.ShardSize = value),
                FloatSlider<CrystalizeShardsImageEffect>("jitter", "Jitter (%)", 0, 100, 65, (effect, value) => effect.Jitter = value),
                FloatSlider<CrystalizeShardsImageEffect>("edge_strength", "Edge strength (%)", 0, 100, 75, (effect, value) => effect.EdgeStrength = value),
                FloatSlider<CrystalizeShardsImageEffect>("shine", "Shine (%)", 0, 100, 30, (effect, value) => effect.Shine = value)),

            Effect<DitheringImageEffect>(
                "dithering",
                ImageEffectCategory.Filters,
                EnumParameter<DitheringImageEffect, DitheringMethod>(
                    "method",
                    "Method",
                    DitheringMethod.FloydSteinberg,
                    (effect, value) => effect.Method = value,
                    ("Floyd-Steinberg", DitheringMethod.FloydSteinberg),
                    ("Bayer 4x4", DitheringMethod.Bayer4x4)),
                EnumParameter<DitheringImageEffect, DitheringPalette>(
                    "palette",
                    "Palette",
                    DitheringPalette.OneBitBW,
                    (effect, value) => effect.Palette = value,
                    ("1-bit B&W", DitheringPalette.OneBitBW),
                    ("Web-safe 216", DitheringPalette.WebSafe216),
                    ("RGB332", DitheringPalette.RGB332),
                    ("Grayscale (4 levels)", DitheringPalette.Grayscale4)),
                BoolParameter<DitheringImageEffect>("serpentine", "Serpentine scan (Floyd-Steinberg)", true, (effect, value) => effect.Serpentine = value),
                FloatSlider<DitheringImageEffect>("strength", "Strength (%)", 0, 100, 100, (effect, value) => effect.Strength = value)),

            Effect<EdgeFeatherImageEffect>(
                "edge_feather",
                ImageEffectCategory.Filters,
                FloatSlider<EdgeFeatherImageEffect>("radius", "Radius (px)", 0, 100, 12, (effect, value) => effect.Radius = value, tickFrequency: 0.5, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.#}")),

            Effect<FrostedGlassIceEdgesImageEffect>(
                "frosted_glass_ice_edges",
                ImageEffectCategory.Filters,
                FloatSlider<FrostedGlassIceEdgesImageEffect>("distortion", "Distortion (px)", 0, 30, 10, (effect, value) => effect.Distortion = value),
                FloatSlider<FrostedGlassIceEdgesImageEffect>("blur", "Blur (%)", 0, 100, 35, (effect, value) => effect.Blur = value),
                IntSlider<FrostedGlassIceEdgesImageEffect>("edge_threshold", "Edge threshold", 1, 120, 28, (effect, value) => effect.EdgeThreshold = value),
                FloatSlider<FrostedGlassIceEdgesImageEffect>("ice_strength", "Ice strength (%)", 0, 100, 75, (effect, value) => effect.IceStrength = value),
                FloatSlider<FrostedGlassIceEdgesImageEffect>("tint", "Cool tint (%)", 0, 100, 35, (effect, value) => effect.Tint = value)),

            Effect<GaussianBlurImageEffect>(
                "gaussian_blur",
                ImageEffectCategory.Filters,
                IntSlider<GaussianBlurImageEffect>("radius", "Radius", 1, 200, 15, (effect, value) => effect.Radius = value)),

            Effect(
                "glow",
                ImageEffectCategory.Filters,
                static () => new GlowImageEffect(20, 80f, ToSkColor(Colors.White), 0, 0, autoResize: true),
                IntSlider<GlowImageEffect>("size", "Size", 1, 100, 20, (effect, value) => effect.Size = value, isSnapToTickEnabled: false),
                FloatSlider<GlowImageEffect>("strength", "Strength", 1, 100, 80, (effect, value) => effect.Strength = value, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0}%"),
                IntSlider<GlowImageEffect>("offset_x", "Offset X", -100, 100, 0, (effect, value) => effect.OffsetX = value, isSnapToTickEnabled: false),
                IntSlider<GlowImageEffect>("offset_y", "Offset Y", -100, 100, 0, (effect, value) => effect.OffsetY = value, isSnapToTickEnabled: false),
                ColorParameter<GlowImageEffect>("color", "Color", Colors.White, (effect, value) => effect.Color = ToSkColor(value)),
                BoolParameter<GlowImageEffect>("auto_resize", "Auto resize", true, (effect, value) => effect.AutoResize = value)),

            Effect(
                "inner_shadow",
                ImageEffectCategory.Filters,
                static () => new InnerShadowImageEffect
                {
                    Opacity = 70f,
                    Size = 20,
                    Color = ToSkColor(Colors.Black),
                    OffsetX = 0,
                    OffsetY = 0
                },
                FloatSlider<InnerShadowImageEffect>("opacity", "Opacity", 0, 100, 70, (effect, value) => effect.Opacity = value, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0}%"),
                IntSlider<InnerShadowImageEffect>("size", "Size", 0, 300, 20, (effect, value) => effect.Size = value, isSnapToTickEnabled: false),
                IntNumeric<InnerShadowImageEffect>("offset_x", "Offset X", -1000, 1000, 0, (effect, value) => effect.OffsetX = value),
                IntNumeric<InnerShadowImageEffect>("offset_y", "Offset Y", -1000, 1000, 0, (effect, value) => effect.OffsetY = value),
                ColorParameter<InnerShadowImageEffect>("color", "Color", Colors.Black, (effect, value) => effect.Color = ToSkColor(value))),

            Effect<HalationImageEffect>(
                "halation",
                ImageEffectCategory.Filters,
                FloatSlider<HalationImageEffect>("threshold", "Highlight threshold (%)", 0, 100, 72, (effect, value) => effect.Threshold = value),
                FloatSlider<HalationImageEffect>("radius", "Radius (px)", 1, 100, 18, (effect, value) => effect.Radius = value),
                FloatSlider<HalationImageEffect>("strength", "Strength (%)", 0, 150, 60, (effect, value) => effect.Strength = value),
                FloatSlider<HalationImageEffect>("warmth", "Warmth (%)", 0, 100, 70, (effect, value) => effect.Warmth = value)),

            Effect<HalftoneImageEffect>(
                "halftone",
                ImageEffectCategory.Filters,
                IntSlider<HalftoneImageEffect>("cell_size", "Cell size (px)", 3, 24, 8, (effect, value) => effect.CellSize = value),
                FloatSlider<HalftoneImageEffect>("dot_softness", "Dot softness (%)", 0, 100, 18, (effect, value) => effect.DotSoftness = value),
                FloatSlider<HalftoneImageEffect>("ink_strength", "Ink strength (%)", 0, 100, 90, (effect, value) => effect.InkStrength = value),
                FloatSlider<HalftoneImageEffect>("source_blend", "Source blend (%)", 0, 100, 20, (effect, value) => effect.SourceBlend = value),
                FloatSlider<HalftoneImageEffect>("angle_offset", "Angle offset (°)", -45, 45, 0, (effect, value) => effect.AngleOffset = value)),

            Effect<HeatHazeRefractionImageEffect>(
                "heat_haze_refraction",
                ImageEffectCategory.Filters,
                FloatSlider<HeatHazeRefractionImageEffect>("strength", "Strength (%)", 0, 100, 45, (effect, value) => effect.Strength = value),
                FloatSlider<HeatHazeRefractionImageEffect>("frequency", "Frequency (%)", 0, 100, 40, (effect, value) => effect.Frequency = value),
                FloatSlider<HeatHazeRefractionImageEffect>("blur_radius", "Blur radius (px)", 0, 30, 10, (effect, value) => effect.BlurRadius = value),
                FloatSlider<HeatHazeRefractionImageEffect>("offset", "Offset", -100, 100, 6, (effect, value) => effect.Offset = value),
                FloatSlider<HeatHazeRefractionImageEffect>("luminance_influence", "Luminance influence (%)", 0, 100, 55, (effect, value) => effect.LuminanceInfluence = value)),

            Effect<HologramScanImageEffect>(
                "hologram_scan",
                ImageEffectCategory.Filters,
                FloatSlider<HologramScanImageEffect>("scanline_strength", "Scanlines (%)", 0, 100, 45, (effect, value) => effect.ScanlineStrength = value),
                FloatSlider<HologramScanImageEffect>("glitch_amount", "Glitch amount (%)", 0, 100, 25, (effect, value) => effect.GlitchAmount = value),
                IntSlider<HologramScanImageEffect>("chroma_shift", "Chroma shift (px)", 0, 12, 2, (effect, value) => effect.ChromaShift = value),
                FloatSlider<HologramScanImageEffect>("glow_amount", "Glow (%)", 0, 100, 30, (effect, value) => effect.GlowAmount = value)),

            Effect<HolographicFoilShimmerImageEffect>(
                "holographic_foil_shimmer",
                ImageEffectCategory.Filters,
                FloatSlider<HolographicFoilShimmerImageEffect>("intensity", "Intensity (%)", 0, 100, 65, (effect, value) => effect.Intensity = value),
                FloatSlider<HolographicFoilShimmerImageEffect>("scale", "Pattern scale (%)", 40, 300, 120, (effect, value) => effect.Scale = value),
                FloatSlider<HolographicFoilShimmerImageEffect>("shift", "Hue shift (deg)", 0, 360, 0, (effect, value) => effect.Shift = value),
                FloatSlider<HolographicFoilShimmerImageEffect>("specular", "Specular (%)", 0, 100, 45, (effect, value) => effect.Specular = value),
                FloatSlider<HolographicFoilShimmerImageEffect>("grain", "Grain sparkle (%)", 0, 100, 20, (effect, value) => effect.Grain = value)),

            Effect<InkSplatterDripsImageEffect>(
                "ink_splatter_drips",
                ImageEffectCategory.Filters,
                FloatSlider<InkSplatterDripsImageEffect>("ink_amount", "Ink amount (%)", 0, 100, 55, (effect, value) => effect.InkAmount = value),
                FloatSlider<InkSplatterDripsImageEffect>("drip_length", "Drip length (%)", 0, 100, 45, (effect, value) => effect.DripLength = value),
                FloatSlider<InkSplatterDripsImageEffect>("spread", "Spread (%)", 0, 100, 35, (effect, value) => effect.Spread = value),
                FloatSlider<InkSplatterDripsImageEffect>("paper_fade", "Paper fade (%)", 0, 100, 20, (effect, value) => effect.PaperFade = value)),

            Effect<LensBlurImageEffect>(
                "lens_blur",
                ImageEffectCategory.Filters,
                IntSlider<LensBlurImageEffect>("radius", "Radius (px)", 1, 15, 8, (effect, value) => effect.Radius = value),
                FloatSlider<LensBlurImageEffect>("highlight_threshold", "Highlight threshold (%)", 0, 100, 70, (effect, value) => effect.HighlightThreshold = value),
                FloatSlider<LensBlurImageEffect>("highlight_boost", "Highlight boost (%)", 0, 200, 85, (effect, value) => effect.HighlightBoost = value)),

            Effect<LiquidGlassImageEffect>(
                "liquid_glass",
                ImageEffectCategory.Filters,
                FloatSlider<LiquidGlassImageEffect>("distortion", "Distortion (px)", 0, 35, 9, (effect, value) => effect.Distortion = value),
                FloatSlider<LiquidGlassImageEffect>("refraction", "Refraction (%)", 0, 100, 45, (effect, value) => effect.Refraction = value),
                IntSlider<LiquidGlassImageEffect>("chroma_shift", "Chroma shift (px)", 0, 8, 1, (effect, value) => effect.ChromaShift = value),
                FloatSlider<LiquidGlassImageEffect>("gloss", "Gloss (%)", 0, 100, 40, (effect, value) => effect.Gloss = value),
                FloatSlider<LiquidGlassImageEffect>("flow_scale", "Flow scale (%)", 40, 220, 100, (effect, value) => effect.FlowScale = value)),

            Effect<LuminanceContourLinesImageEffect>(
                "luminance_contour_lines",
                ImageEffectCategory.Filters,
                IntSlider<LuminanceContourLinesImageEffect>("levels", "Levels", 2, 64, 12, (effect, value) => effect.Levels = value),
                FloatSlider<LuminanceContourLinesImageEffect>("line_width", "Line width", 0, 200, 6, (effect, value) => effect.LineWidth = value),
                FloatSlider<LuminanceContourLinesImageEffect>("line_strength", "Line strength (%)", 0, 100, 65, (effect, value) => effect.LineStrength = value),
                FloatSlider<LuminanceContourLinesImageEffect>("background_strength", "Background strength (%)", 0, 100, 20, (effect, value) => effect.BackgroundStrength = value),
                FloatSlider<LuminanceContourLinesImageEffect>("threshold", "Threshold bias", 0, 255, 0, (effect, value) => effect.Threshold = value),
                BoolParameter<LuminanceContourLinesImageEffect>("invert", "Invert contour mask", false, (effect, value) => effect.Invert = value),
                ColorParameter<LuminanceContourLinesImageEffect>("line_color", "Line color", Colors.Black, (effect, value) => effect.LineColor = ToSkColor(value))),

            Effect<MatrixDigitalRainImageEffect>(
                "matrix_digital_rain",
                ImageEffectCategory.Filters,
                IntSlider<MatrixDigitalRainImageEffect>("cell_size", "Cell size (px)", 6, 24, 12, (effect, value) => effect.CellSize = value),
                FloatSlider<MatrixDigitalRainImageEffect>("density", "Character density (%)", 0, 100, 85, (effect, value) => effect.Density = value),
                IntSlider<MatrixDigitalRainImageEffect>("trail_length", "Trail length", 3, 50, 12, (effect, value) => effect.TrailLength = value),
                FloatSlider<MatrixDigitalRainImageEffect>("glow_amount", "Glow (%)", 0, 100, 40, (effect, value) => effect.GlowAmount = value),
                FloatSlider<MatrixDigitalRainImageEffect>("source_blend", "Source blend (%)", 0, 100, 22, (effect, value) => effect.SourceBlend = value),
                FloatSlider<MatrixDigitalRainImageEffect>("rain_offset", "Rain offset (%)", 0, 100, 0, (effect, value) => effect.RainOffset = value),
                FloatSlider<MatrixDigitalRainImageEffect>("luminance_influence", "Luminance influence (%)", 0, 100, 65, (effect, value) => effect.LuminanceInfluence = value),
                TextParameter<MatrixDigitalRainImageEffect>("character_set", "Character set", "01<>[]{}*+-/\\=#$%&", (effect, value) => effect.CharacterSet = value)),

            Effect<MedianFilterImageEffect>(
                "median_filter",
                ImageEffectCategory.Filters,
                IntSlider<MedianFilterImageEffect>("radius", "Radius", 1, 5, 1, (effect, value) => effect.Radius = value)),

            Effect<MosaicPolygonImageEffect>(
                "mosaic_polygon",
                ImageEffectCategory.Filters,
                IntSlider<MosaicPolygonImageEffect>("cell_size", "Cell size (px)", 8, 80, 24, (effect, value) => effect.CellSize = value),
                EnumParameter<MosaicPolygonImageEffect, MosaicPolygonShape>(
                    "shape",
                    "Shape",
                    MosaicPolygonShape.Hexagon,
                    (effect, value) => effect.Shape = value,
                    ("Hexagon", MosaicPolygonShape.Hexagon),
                    ("Triangle", MosaicPolygonShape.Triangle)),
                FloatSlider<MosaicPolygonImageEffect>("border_width", "Border width (px)", 0, 6, 1, (effect, value) => effect.BorderWidth = value, tickFrequency: 0.1, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.0}"),
                FloatSlider<MosaicPolygonImageEffect>("border_opacity", "Border opacity (%)", 0, 100, 45, (effect, value) => effect.BorderOpacity = value),
                FloatSlider<MosaicPolygonImageEffect>("randomness", "Randomness (%)", 0, 100, 18, (effect, value) => effect.Randomness = value)),

            Effect<MotionBlurImageEffect>(
                "motion_blur",
                ImageEffectCategory.Filters,
                IntSlider<MotionBlurImageEffect>("distance", "Distance", 1, 200, 12, (effect, value) => effect.Distance = value),
                FloatSlider<MotionBlurImageEffect>("angle", "Angle", -360, 360, 0, (effect, value) => effect.Angle = value)),

            Effect<NebulaStarfieldImageEffect>(
                "nebula_starfield",
                ImageEffectCategory.Filters,
                FloatSlider<NebulaStarfieldImageEffect>("intensity", "Intensity (%)", 0, 100, 70, (effect, value) => effect.Intensity = value),
                FloatSlider<NebulaStarfieldImageEffect>("scale", "Nebula scale (%)", 0, 100, 80, (effect, value) => effect.Scale = value),
                FloatSlider<NebulaStarfieldImageEffect>("hue_shift", "Hue shift", -180, 180, -15, (effect, value) => effect.HueShift = value),
                FloatSlider<NebulaStarfieldImageEffect>("star_density", "Star density (%)", 0, 100, 55, (effect, value) => effect.StarDensity = value),
                FloatSlider<NebulaStarfieldImageEffect>("star_size", "Star size", 0, 200, 10, (effect, value) => effect.StarSize = value),
                FloatSlider<NebulaStarfieldImageEffect>("twinkle", "Twinkle (%)", 0, 100, 40, (effect, value) => effect.Twinkle = value),
                FloatSlider<NebulaStarfieldImageEffect>("vignette_strength", "Vignette (%)", 0, 100, 18, (effect, value) => effect.VignetteStrength = value)),

            Effect<NeonEdgeGlowImageEffect>(
                "neon_edge_glow",
                ImageEffectCategory.Filters,
                FloatSlider<NeonEdgeGlowImageEffect>("edge_strength", "Edge strength", 0.1, 6, 2.2, (effect, value) => effect.EdgeStrength = value, tickFrequency: 0.1, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.0}"),
                IntSlider<NeonEdgeGlowImageEffect>("threshold", "Threshold", 0, 255, 36, (effect, value) => effect.Threshold = value),
                FloatSlider<NeonEdgeGlowImageEffect>("glow_radius", "Glow radius (px)", 0, 40, 8, (effect, value) => effect.GlowRadius = value),
                FloatSlider<NeonEdgeGlowImageEffect>("glow_intensity", "Glow intensity (%)", 0, 250, 120, (effect, value) => effect.GlowIntensity = value),
                FloatSlider<NeonEdgeGlowImageEffect>("base_dim", "Base dim (%)", 0, 100, 30, (effect, value) => effect.BaseDim = value),
                ColorParameter<NeonEdgeGlowImageEffect>("neon_color", "Neon color", Argb(255, 0, 240, 255), (effect, value) => effect.NeonColor = ToSkColor(value))),

            Effect<OilPaintImageEffect>(
                "oil_paint",
                ImageEffectCategory.Filters,
                IntSlider<OilPaintImageEffect>("radius", "Radius", 1, 6, 3, (effect, value) => effect.Radius = value),
                IntSlider<OilPaintImageEffect>("levels", "Levels", 8, 64, 24, (effect, value) => effect.Levels = value)),

            Effect<OldCameraFlashBurnImageEffect>(
                "old_camera_flash_burn",
                ImageEffectCategory.Filters,
                FloatSlider<OldCameraFlashBurnImageEffect>("flash_strength", "Flash strength (%)", 0, 100, 70, (effect, value) => effect.FlashStrength = value),
                FloatSlider<OldCameraFlashBurnImageEffect>("flash_radius", "Flash radius (%)", 20, 100, 68, (effect, value) => effect.FlashRadius = value),
                FloatSlider<OldCameraFlashBurnImageEffect>("edge_burn", "Edge burn (%)", 0, 100, 45, (effect, value) => effect.EdgeBurn = value),
                FloatSlider<OldCameraFlashBurnImageEffect>("warmth", "Warmth (%)", 0, 100, 35, (effect, value) => effect.Warmth = value),
                FloatSlider<OldCameraFlashBurnImageEffect>("grain", "Film grain (%)", 0, 100, 20, (effect, value) => effect.Grain = value),
                FloatSlider<OldCameraFlashBurnImageEffect>("center_x", "Flash center X (%)", 0, 100, 50, (effect, value) => effect.CenterX = value),
                FloatSlider<OldCameraFlashBurnImageEffect>("center_y", "Flash center Y (%)", 0, 100, 50, (effect, value) => effect.CenterY = value)),

            Effect(
                "outline",
                ImageEffectCategory.Filters,
                static () => new OutlineImageEffect(3, 0, outlineOnly: false, ToSkColor(Colors.Black)),
                IntSlider<OutlineImageEffect>("size", "Size", 1, 50, 3, (effect, value) => effect.Size = value, isSnapToTickEnabled: false),
                IntSlider<OutlineImageEffect>("padding", "Padding", 0, 100, 0, (effect, value) => effect.Padding = value, isSnapToTickEnabled: false),
                BoolParameter<OutlineImageEffect>("outline_only", "Outline only", false, (effect, value) => effect.OutlineOnly = value),
                ColorParameter<OutlineImageEffect>("color", "Color", Colors.Black, (effect, value) => effect.Color = ToSkColor(value))),

            Effect<PaperStencilMaskImageEffect>(
                "paper_stencil_mask",
                ImageEffectCategory.Filters,
                FloatSlider<PaperStencilMaskImageEffect>("threshold", "Threshold", 0, 255, 140, (effect, value) => effect.Threshold = value),
                FloatSlider<PaperStencilMaskImageEffect>("feather_radius", "Feather radius", 0, 30, 8, (effect, value) => effect.FeatherRadius = value),
                FloatSlider<PaperStencilMaskImageEffect>("edge_strength", "Edge strength (%)", 0, 100, 70, (effect, value) => effect.EdgeStrength = value),
                FloatSlider<PaperStencilMaskImageEffect>("background_dim", "Background dim (%)", 0, 100, 35, (effect, value) => effect.BackgroundDim = value),
                BoolParameter<PaperStencilMaskImageEffect>("invert_mask", "Invert mask", false, (effect, value) => effect.InvertMask = value),
                ColorParameter<PaperStencilMaskImageEffect>("stencil_color", "Stencil color", Argb(220, 0, 0, 0), (effect, value) => effect.StencilColor = ToSkColor(value))),

            Effect<PencilSketchImageEffect>(
                "pencil_sketch",
                ImageEffectCategory.Filters,
                IntSlider<PencilSketchImageEffect>("blur_radius", "Blur radius (px)", 1, 24, 8, (effect, value) => effect.BlurRadius = value),
                FloatSlider<PencilSketchImageEffect>("edge_strength", "Edge strength (%)", 0, 160, 65, (effect, value) => effect.EdgeStrength = value),
                FloatSlider<PencilSketchImageEffect>("pencil_darkness", "Pencil darkness (%)", 0, 100, 70, (effect, value) => effect.PencilDarkness = value),
                FloatSlider<PencilSketchImageEffect>("paper_brightness", "Paper brightness (%)", 40, 130, 100, (effect, value) => effect.PaperBrightness = value)),

            Effect<PixelateImageEffect>(
                "pixelate",
                ImageEffectCategory.Filters,
                IntSlider<PixelateImageEffect>("size", "Size", 2, 200, 10, (effect, value) => effect.Size = value, valueStringFormat: "{}{0:0} px")),

            Effect<PixelSortingImageEffect>(
                "pixel_sorting",
                ImageEffectCategory.Filters,
                EnumParameter<PixelSortingImageEffect, PixelSortDirection>(
                    "direction",
                    "Direction",
                    PixelSortDirection.Vertical,
                    (effect, value) => effect.Direction = value,
                    ("Horizontal", PixelSortDirection.Horizontal),
                    ("Vertical", PixelSortDirection.Vertical)),
                EnumParameter<PixelSortingImageEffect, PixelSortMetric>(
                    "metric",
                    "Metric",
                    PixelSortMetric.Brightness,
                    (effect, value) => effect.Metric = value,
                    ("Brightness", PixelSortMetric.Brightness),
                    ("Hue", PixelSortMetric.Hue)),
                FloatSlider<PixelSortingImageEffect>("threshold_low", "Threshold low (%)", 0, 100, 12, (effect, value) => effect.ThresholdLow = value),
                FloatSlider<PixelSortingImageEffect>("threshold_high", "Threshold high (%)", 0, 100, 85, (effect, value) => effect.ThresholdHigh = value),
                IntSlider<PixelSortingImageEffect>("min_span_length", "Minimum span (px)", 2, 256, 8, (effect, value) => effect.MinSpanLength = value),
                IntSlider<PixelSortingImageEffect>("max_span_length", "Maximum span (px)", 2, 512, 120, (effect, value) => effect.MaxSpanLength = value),
                FloatSlider<PixelSortingImageEffect>("sort_probability", "Sort probability (%)", 0, 100, 85, (effect, value) => effect.SortProbability = value)),

            Effect<PointillismImageEffect>(
                "pointillism",
                ImageEffectCategory.Filters,
                IntSlider<PointillismImageEffect>("dot_size", "Dot size (px)", 2, 24, 7, (effect, value) => effect.DotSize = value),
                FloatSlider<PointillismImageEffect>("density", "Density (%)", 10, 100, 72, (effect, value) => effect.Density = value),
                FloatSlider<PointillismImageEffect>("jitter", "Jitter (%)", 0, 100, 65, (effect, value) => effect.Jitter = value),
                FloatSlider<PointillismImageEffect>("color_boost", "Color boost (%)", 0, 100, 20, (effect, value) => effect.ColorBoost = value),
                FloatSlider<PointillismImageEffect>("background_mix", "Background mix (%)", 0, 100, 20, (effect, value) => effect.BackgroundMix = value)),

            Effect<RainyWindowImageEffect>(
                "rainy_window",
                ImageEffectCategory.Filters,
                FloatSlider<RainyWindowImageEffect>("distortion", "Glass distortion", 0, 30, 8, (effect, value) => effect.Distortion = value),
                FloatSlider<RainyWindowImageEffect>("streak_density", "Streak density (%)", 0, 100, 45, (effect, value) => effect.StreakDensity = value),
                FloatSlider<RainyWindowImageEffect>("mist_amount", "Mist (%)", 0, 100, 25, (effect, value) => effect.MistAmount = value),
                FloatSlider<RainyWindowImageEffect>("droplet_amount", "Droplets (%)", 0, 100, 35, (effect, value) => effect.DropletAmount = value)),

            Effect(
                "reflection",
                ImageEffectCategory.Filters,
                static () => new ReflectionImageEffect(20, 255, 0, 0, skew: false, skewSize: 25),
                IntSlider<ReflectionImageEffect>("percentage", "Percentage", 1, 100, 20, (effect, value) => effect.Percentage = value, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0}%"),
                IntSlider<ReflectionImageEffect>("max_alpha", "Max Alpha", 0, 255, 255, (effect, value) => effect.MaxAlpha = value, isSnapToTickEnabled: false),
                IntSlider<ReflectionImageEffect>("min_alpha", "Min Alpha", 0, 255, 0, (effect, value) => effect.MinAlpha = value, isSnapToTickEnabled: false),
                IntSlider<ReflectionImageEffect>("offset", "Offset", 0, 100, 0, (effect, value) => effect.Offset = value, isSnapToTickEnabled: false),
                BoolParameter<ReflectionImageEffect>("skew", "Skew", false, (effect, value) => effect.Skew = value),
                IntSlider<ReflectionImageEffect>("skew_size", "Skew Size", 1, 100, 25, (effect, value) => effect.SkewSize = value, isSnapToTickEnabled: false)),

            Effect<RemoveBackgroundImageEffect>(
                "remove_background",
                ImageEffectCategory.Filters,
                FloatSlider<RemoveBackgroundImageEffect>("sensitivity", "Sensitivity (%)", 0, 100, 60, (effect, value) => effect.Sensitivity = value),
                FloatSlider<RemoveBackgroundImageEffect>("center_protection", "Center protection (%)", 0, 100, 65, (effect, value) => effect.CenterProtection = value),
                FloatSlider<RemoveBackgroundImageEffect>("edge_feather", "Edge feather (px)", 0, 24, 4, (effect, value) => effect.EdgeFeather = value, tickFrequency: 0.5, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.#}")),

            Effect<RGBSplitImageEffect>(
                "rgb_split",
                ImageEffectCategory.Filters,
                IntNumeric<RGBSplitImageEffect>("offset_red_x", "Red offset X", -1000, 1000, -5, (effect, value) => effect.OffsetRedX = value),
                IntNumeric<RGBSplitImageEffect>("offset_red_y", "Red offset Y", -1000, 1000, 0, (effect, value) => effect.OffsetRedY = value),
                IntNumeric<RGBSplitImageEffect>("offset_green_x", "Green offset X", -1000, 1000, 0, (effect, value) => effect.OffsetGreenX = value),
                IntNumeric<RGBSplitImageEffect>("offset_green_y", "Green offset Y", -1000, 1000, 0, (effect, value) => effect.OffsetGreenY = value),
                IntNumeric<RGBSplitImageEffect>("offset_blue_x", "Blue offset X", -1000, 1000, 5, (effect, value) => effect.OffsetBlueX = value),
                IntNumeric<RGBSplitImageEffect>("offset_blue_y", "Blue offset Y", -1000, 1000, 0, (effect, value) => effect.OffsetBlueY = value)),

            Effect<RisoPrintImageEffect>(
                "riso_print",
                ImageEffectCategory.Filters,
                FloatSlider<RisoPrintImageEffect>("ink_strength", "Ink strength (%)", 0, 100, 70, (effect, value) => effect.InkStrength = value),
                FloatSlider<RisoPrintImageEffect>("paper_fade", "Paper fade (%)", 0, 100, 25, (effect, value) => effect.PaperFade = value),
                FloatSlider<RisoPrintImageEffect>("offset", "Registration offset", -100, 100, 3, (effect, value) => effect.Offset = value),
                FloatSlider<RisoPrintImageEffect>("dot_scale", "Dot scale (%)", 0, 100, 18, (effect, value) => effect.DotScale = value),
                FloatSlider<RisoPrintImageEffect>("ink_noise", "Ink noise (%)", 0, 100, 35, (effect, value) => effect.InkNoise = value),
                ColorParameter<RisoPrintImageEffect>("ink_color_a", "Primary ink", Argb(255, 220, 70, 70), (effect, value) => effect.InkColorA = ToSkColor(value)),
                ColorParameter<RisoPrintImageEffect>("ink_color_b", "Secondary ink", Argb(255, 70, 200, 210), (effect, value) => effect.InkColorB = ToSkColor(value))),

            Effect(
                "shadow",
                ImageEffectCategory.Filters,
                static () => new ShadowImageEffect(80f, 20, ToSkColor(Colors.Black), 5, 5, autoResize: true),
                FloatSlider<ShadowImageEffect>("opacity", "Opacity", 0, 100, 80, (effect, value) => effect.Opacity = value, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0}%"),
                IntSlider<ShadowImageEffect>("size", "Size", 0, 100, 20, (effect, value) => effect.Size = value, isSnapToTickEnabled: false),
                IntNumeric<ShadowImageEffect>("offset_x", "Offset X", -1000, 1000, 5, (effect, value) => effect.OffsetX = value),
                IntNumeric<ShadowImageEffect>("offset_y", "Offset Y", -1000, 1000, 5, (effect, value) => effect.OffsetY = value),
                ColorParameter<ShadowImageEffect>("color", "Color", Colors.Black, (effect, value) => effect.Color = ToSkColor(value)),
                BoolParameter<ShadowImageEffect>("auto_resize", "Auto resize", true, (effect, value) => effect.AutoResize = value)),

            Effect<SharpenImageEffect>(
                "sharpen",
                ImageEffectCategory.Filters,
                IntSlider<SharpenImageEffect>("strength", "Strength", 0, 100, 50, (effect, value) => effect.Strength = value, valueStringFormat: "{}{0:0}%")),

            Effect(
                "slice",
                ImageEffectCategory.Filters,
                static () => new SliceImageEffect(10, 100, 0, 10),
                IntSlider<SliceImageEffect>("min_height", "Min Height", 1, 100, 10, (effect, value) => effect.MinHeight = value, isSnapToTickEnabled: false),
                IntSlider<SliceImageEffect>("max_height", "Max Height", 1, 200, 100, (effect, value) => effect.MaxHeight = value, isSnapToTickEnabled: false),
                IntSlider<SliceImageEffect>("min_shift", "Min Shift", 0, 100, 0, (effect, value) => effect.MinShift = value, isSnapToTickEnabled: false),
                IntSlider<SliceImageEffect>("max_shift", "Max Shift", 0, 100, 10, (effect, value) => effect.MaxShift = value, isSnapToTickEnabled: false)),

            Effect<SnowfallDepthFogImageEffect>(
                "snowfall_depth_fog",
                ImageEffectCategory.Filters,
                FloatSlider<SnowfallDepthFogImageEffect>("snow_amount", "Snow amount (%)", 0, 100, 55, (effect, value) => effect.SnowAmount = value),
                FloatSlider<SnowfallDepthFogImageEffect>("flake_size", "Flake size", 1, 14, 5, (effect, value) => effect.FlakeSize = value),
                FloatSlider<SnowfallDepthFogImageEffect>("wind", "Wind", -100, 100, 20, (effect, value) => effect.Wind = value),
                FloatSlider<SnowfallDepthFogImageEffect>("fog_amount", "Fog amount (%)", 0, 100, 40, (effect, value) => effect.FogAmount = value),
                FloatSlider<SnowfallDepthFogImageEffect>("fog_height", "Fog height (%)", 10, 100, 65, (effect, value) => effect.FogHeight = value)),

            Effect<SobelEdgeImageEffect>(
                "sobel_edge",
                ImageEffectCategory.Filters,
                FloatSlider<SobelEdgeImageEffect>("strength", "Strength", 0.1, 5, 1.2, (effect, value) => effect.Strength = value, tickFrequency: 0.1, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.#}"),
                IntSlider<SobelEdgeImageEffect>("threshold", "Threshold", 0, 255, 20, (effect, value) => effect.Threshold = value)),

            Effect<SpinBlurImageEffect>(
                "spin_blur",
                ImageEffectCategory.Filters,
                FloatSlider<SpinBlurImageEffect>("angle", "Angle (deg)", 0, 180, 20, (effect, value) => effect.Angle = value),
                IntSlider<SpinBlurImageEffect>("samples", "Samples", 4, 64, 24, (effect, value) => effect.Samples = value),
                FloatSlider<SpinBlurImageEffect>("center_x", "Center X (%)", 0, 100, 50, (effect, value) => effect.CenterX = value),
                FloatSlider<SpinBlurImageEffect>("center_y", "Center Y (%)", 0, 100, 50, (effect, value) => effect.CenterY = value)),

            Effect<StainedGlassImageEffect>(
                "stained_glass",
                ImageEffectCategory.Filters,
                IntSlider<StainedGlassImageEffect>("tile_size", "Tile size (px)", 6, 120, 22, (effect, value) => effect.TileSize = value),
                FloatSlider<StainedGlassImageEffect>("irregularity", "Irregularity (%)", 0, 100, 55, (effect, value) => effect.Irregularity = value),
                FloatSlider<StainedGlassImageEffect>("lead_width", "Lead width (px)", 0, 12, 1.8, (effect, value) => effect.LeadWidth = value, tickFrequency: 0.1, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.0}"),
                FloatSlider<StainedGlassImageEffect>("lead_opacity", "Lead opacity (%)", 0, 100, 85, (effect, value) => effect.LeadOpacity = value),
                FloatSlider<StainedGlassImageEffect>("color_boost", "Color boost (%)", 0, 100, 20, (effect, value) => effect.ColorBoost = value)),

            Effect<SurfaceBlurImageEffect>(
                "surface_blur",
                ImageEffectCategory.Filters,
                IntSlider<SurfaceBlurImageEffect>("radius", "Radius (px)", 1, 8, 3, (effect, value) => effect.Radius = value),
                IntSlider<SurfaceBlurImageEffect>("threshold", "Edge threshold", 1, 100, 24, (effect, value) => effect.Threshold = value)),

            Effect<ThermalVisionImageEffect>(
                "thermal_vision",
                ImageEffectCategory.Filters,
                IntSlider<ThermalVisionImageEffect>("levels", "Levels", 3, 16, 8, (effect, value) => effect.Levels = value),
                FloatSlider<ThermalVisionImageEffect>("contrast", "Contrast (%)", 50, 200, 135, (effect, value) => effect.Contrast = value),
                FloatSlider<ThermalVisionImageEffect>("glow", "Glow (%)", 0, 100, 28, (effect, value) => effect.Glow = value),
                FloatSlider<ThermalVisionImageEffect>("blend", "Blend (%)", 0, 100, 100, (effect, value) => effect.Blend = value),
                BoolParameter<ThermalVisionImageEffect>("invert", "Invert heatmap", false, (effect, value) => effect.Invert = value)),

            Effect<TiltShiftImageEffect>(
                "tilt_shift",
                ImageEffectCategory.Filters,
                EnumParameter<TiltShiftImageEffect, TiltShiftMode>(
                    "mode",
                    "Mode",
                    TiltShiftMode.Linear,
                    (effect, value) => effect.Mode = value,
                    ("Linear", TiltShiftMode.Linear),
                    ("Radial", TiltShiftMode.Radial)),
                FloatSlider<TiltShiftImageEffect>("blur_radius", "Blur radius (px)", 0, 30, 12, (effect, value) => effect.BlurRadius = value),
                FloatSlider<TiltShiftImageEffect>("focus_size", "Focus size (%)", 5, 90, 30, (effect, value) => effect.FocusSize = value),
                FloatSlider<TiltShiftImageEffect>("focus_position_x", "Focus X (%)", 0, 100, 50, (effect, value) => effect.FocusPositionX = value),
                FloatSlider<TiltShiftImageEffect>("focus_position_y", "Focus Y (%)", 0, 100, 50, (effect, value) => effect.FocusPositionY = value),
                FloatSlider<TiltShiftImageEffect>("falloff", "Falloff (%)", 1, 60, 24, (effect, value) => effect.Falloff = value),
                FloatSlider<TiltShiftImageEffect>("saturation_boost", "Saturation boost (%)", 0, 100, 35, (effect, value) => effect.SaturationBoost = value)),

            Effect(
                "torn_edge",
                ImageEffectCategory.Filters,
                static () => new TornEdgeImageEffect(20, 20, top: true, right: true, bottom: true, left: true, curved: false),
                IntSlider<TornEdgeImageEffect>("depth", "Depth", 1, 100, 20, (effect, value) => effect.Depth = value, isSnapToTickEnabled: false),
                IntSlider<TornEdgeImageEffect>("range", "Range", 1, 100, 20, (effect, value) => effect.Range = value, isSnapToTickEnabled: false),
                BoolParameter<TornEdgeImageEffect>("top", "Top", true, (effect, value) => effect.Top = value),
                BoolParameter<TornEdgeImageEffect>("right", "Right", true, (effect, value) => effect.Right = value),
                BoolParameter<TornEdgeImageEffect>("bottom", "Bottom", true, (effect, value) => effect.Bottom = value),
                BoolParameter<TornEdgeImageEffect>("left", "Left", true, (effect, value) => effect.Left = value),
                BoolParameter<TornEdgeImageEffect>("curved", "Curved edges", false, (effect, value) => effect.Curved = value)),

            Effect<UnsharpMaskImageEffect>(
                "unsharp_mask",
                ImageEffectCategory.Filters,
                FloatSlider<UnsharpMaskImageEffect>("radius", "Radius", 1, 100, 5, (effect, value) => effect.Radius = value, tickFrequency: 0.5, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.#}"),
                FloatSlider<UnsharpMaskImageEffect>("amount", "Amount (%)", 0, 500, 150, (effect, value) => effect.Amount = value, valueStringFormat: "{}{0:0}%"),
                IntSlider<UnsharpMaskImageEffect>("threshold", "Threshold", 0, 255, 0, (effect, value) => effect.Threshold = value)),

            Effect<VignetteImageEffect>(
                "vignette",
                ImageEffectCategory.Filters,
                FloatSlider<VignetteImageEffect>("strength", "Strength", 0, 1, 0.5, (effect, value) => effect.Strength = value, tickFrequency: 0.01, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.##}"),
                FloatSlider<VignetteImageEffect>("radius", "Radius", 0.05, 1, 0.75, (effect, value) => effect.Radius = value, tickFrequency: 0.01, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.##}")),

            Effect<VintagePrintDamageImageEffect>(
                "vintage_print_damage",
                ImageEffectCategory.Filters,
                FloatSlider<VintagePrintDamageImageEffect>("sepia_amount", "Sepia tone (%)", 0, 100, 65, (effect, value) => effect.SepiaAmount = value),
                FloatSlider<VintagePrintDamageImageEffect>("fade_amount", "Fade (%)", 0, 100, 30, (effect, value) => effect.FadeAmount = value),
                FloatSlider<VintagePrintDamageImageEffect>("grain_amount", "Grain (%)", 0, 100, 25, (effect, value) => effect.GrainAmount = value),
                FloatSlider<VintagePrintDamageImageEffect>("scratch_amount", "Scratches (%)", 0, 100, 25, (effect, value) => effect.ScratchAmount = value),
                FloatSlider<VintagePrintDamageImageEffect>("dust_amount", "Dust (%)", 0, 100, 20, (effect, value) => effect.DustAmount = value)),

            Effect<WatercolorKuwaharaImageEffect>(
                "watercolor_kuwahara",
                ImageEffectCategory.Filters,
                IntSlider<WatercolorKuwaharaImageEffect>("radius", "Radius (px)", 2, 10, 4, (effect, value) => effect.Radius = value),
                FloatSlider<WatercolorKuwaharaImageEffect>("saturation_boost", "Saturation boost (%)", 0, 100, 20, (effect, value) => effect.SaturationBoost = value),
                FloatSlider<WatercolorKuwaharaImageEffect>("detail_blend", "Detail blend (%)", 0, 100, 18, (effect, value) => effect.DetailBlend = value)),

            Effect<WaveEdgeImageEffect>(
                "wave_edge",
                ImageEffectCategory.Filters,
                IntSlider<WaveEdgeImageEffect>("depth", "Depth", 1, 100, 15, (effect, value) => effect.Depth = value, isSnapToTickEnabled: false),
                IntSlider<WaveEdgeImageEffect>("range", "Range", 1, 100, 20, (effect, value) => effect.Range = value, isSnapToTickEnabled: false),
                BoolParameter<WaveEdgeImageEffect>("top", "Top", true, (effect, value) => effect.Top = value),
                BoolParameter<WaveEdgeImageEffect>("right", "Right", true, (effect, value) => effect.Right = value),
                BoolParameter<WaveEdgeImageEffect>("bottom", "Bottom", true, (effect, value) => effect.Bottom = value),
                BoolParameter<WaveEdgeImageEffect>("left", "Left", true, (effect, value) => effect.Left = value)),

            Effect<ZoomBlurImageEffect>(
                "zoom_blur",
                ImageEffectCategory.Filters,
                FloatSlider<ZoomBlurImageEffect>("strength", "Strength (%)", 0, 100, 35, (effect, value) => effect.Strength = value),
                IntSlider<ZoomBlurImageEffect>("samples", "Samples", 4, 64, 24, (effect, value) => effect.Samples = value),
                FloatSlider<ZoomBlurImageEffect>("center_x", "Center X (%)", 0, 100, 50, (effect, value) => effect.CenterX = value),
                FloatSlider<ZoomBlurImageEffect>("center_y", "Center Y (%)", 0, 100, 50, (effect, value) => effect.CenterY = value)),

            Effect<Anaglyph3DImageEffect>(
                "anaglyph_3d",
                ImageEffectCategory.Filters,
                EnumParameter<Anaglyph3DImageEffect, AnaglyphMode>(
                    "mode",
                    "Mode",
                    AnaglyphMode.RedCyan,
                    (effect, value) => effect.Mode = value,
                    ("Red / cyan", AnaglyphMode.RedCyan),
                    ("Green / magenta", AnaglyphMode.GreenMagenta),
                    ("Amber / blue", AnaglyphMode.AmberBlue)),
                IntSlider<Anaglyph3DImageEffect>("separation_x", "Separation X", -64, 64, 10, (effect, value) => effect.SeparationX = value),
                IntSlider<Anaglyph3DImageEffect>("separation_y", "Separation Y", -64, 64, 0, (effect, value) => effect.SeparationY = value),
                FloatSlider<Anaglyph3DImageEffect>("ghost_reduction", "Ghost reduction (%)", 0, 100, 45, (effect, value) => effect.GhostReduction = value)),
            Effect<CloudsImageEffect>(
                "clouds",
                ImageEffectCategory.Filters,
                FloatSlider<CloudsImageEffect>("coverage", "Coverage (%)", 0, 100, 55, (effect, value) => effect.Coverage = value),
                FloatSlider<CloudsImageEffect>("scale", "Scale (%)", 0, 100, 72, (effect, value) => effect.Scale = value),
                FloatSlider<CloudsImageEffect>("height_bias", "Height bias (%)", 0, 100, 62, (effect, value) => effect.HeightBias = value),
                FloatSlider<CloudsImageEffect>("softness", "Softness (%)", 0, 100, 64, (effect, value) => effect.Softness = value),
                FloatSlider<CloudsImageEffect>("sunlight", "Sunlight (%)", 0, 100, 38, (effect, value) => effect.Sunlight = value)),
            Effect<CyanotypeImageEffect>(
                "cyanotype",
                ImageEffectCategory.Filters,
                FloatSlider<CyanotypeImageEffect>("contrast", "Contrast (%)", 50, 200, 118, (effect, value) => effect.Contrast = value),
                FloatSlider<CyanotypeImageEffect>("paper_texture", "Paper texture (%)", 0, 100, 34, (effect, value) => effect.PaperTexture = value),
                FloatSlider<CyanotypeImageEffect>("stain", "Stain (%)", 0, 100, 28, (effect, value) => effect.Stain = value),
                FloatSlider<CyanotypeImageEffect>("grain", "Grain (%)", 0, 100, 12, (effect, value) => effect.Grain = value),
                FloatSlider<CyanotypeImageEffect>("vignette", "Vignette (%)", 0, 100, 20, (effect, value) => effect.Vignette = value),
                IntNumeric<CyanotypeImageEffect>("seed", "Seed", 0, int.MaxValue, 1842, (effect, value) => effect.Seed = value)),
            Effect<DatamoshSmearImageEffect>(
                "datamosh_smear",
                ImageEffectCategory.Filters,
                EnumParameter<DatamoshSmearImageEffect, DatamoshDirection>(
                    "direction", "Direction", DatamoshDirection.Horizontal, (effect, value) => effect.Direction = value,
                    ("Horizontal", DatamoshDirection.Horizontal), ("Vertical", DatamoshDirection.Vertical)),
                FloatSlider<DatamoshSmearImageEffect>("smear_amount", "Smear amount (%)", 0, 100, 58, (effect, value) => effect.SmearAmount = value),
                FloatSlider<DatamoshSmearImageEffect>("corruption", "Corruption (%)", 0, 100, 36, (effect, value) => effect.Corruption = value),
                IntSlider<DatamoshSmearImageEffect>("block_size", "Block size", 2, 64, 12, (effect, value) => effect.BlockSize = value),
                FloatSlider<DatamoshSmearImageEffect>("drift", "Drift (%)", -100, 100, 24, (effect, value) => effect.Drift = value),
                FloatSlider<DatamoshSmearImageEffect>("channel_split", "Channel split (%)", 0, 100, 25, (effect, value) => effect.ChannelSplit = value),
                IntNumeric<DatamoshSmearImageEffect>("seed", "Seed", 0, int.MaxValue, 9011, (effect, value) => effect.Seed = value)),
            Effect<DisposableCameraImageEffect>(
                "disposable_camera",
                ImageEffectCategory.Filters,
                FloatSlider<DisposableCameraImageEffect>("flash", "Flash (%)", 0, 100, 55, (effect, value) => effect.Flash = value),
                FloatSlider<DisposableCameraImageEffect>("softness", "Softness (%)", 0, 100, 8, (effect, value) => effect.Softness = value),
                FloatSlider<DisposableCameraImageEffect>("grain", "Grain (%)", 0, 100, 24, (effect, value) => effect.Grain = value),
                FloatSlider<DisposableCameraImageEffect>("vignette", "Vignette (%)", 0, 100, 34, (effect, value) => effect.Vignette = value),
                FloatSlider<DisposableCameraImageEffect>("warmth", "Warmth (%)", 0, 100, 42, (effect, value) => effect.Warmth = value),
                IntNumeric<DisposableCameraImageEffect>("seed", "Seed", 0, int.MaxValue, 1996, (effect, value) => effect.Seed = value)),
            Effect<DoubleExposureImageEffect>(
                "double_exposure",
                ImageEffectCategory.Filters,
                FloatSlider<DoubleExposureImageEffect>("blend_amount", "Blend amount (%)", 0, 100, 62, (effect, value) => effect.BlendAmount = value),
                IntSlider<DoubleExposureImageEffect>("offset_x", "Offset X", -300, 300, 22, (effect, value) => effect.OffsetX = value),
                IntSlider<DoubleExposureImageEffect>("offset_y", "Offset Y", -300, 300, -14, (effect, value) => effect.OffsetY = value),
                FloatSlider<DoubleExposureImageEffect>("ghost_blur", "Ghost blur (px)", 0, 40, 8, (effect, value) => effect.GhostBlur = value),
                FloatSlider<DoubleExposureImageEffect>("highlight_bias", "Highlight bias (%)", 0, 100, 55, (effect, value) => effect.HighlightBias = value),
                FloatSlider<DoubleExposureImageEffect>("contrast", "Contrast (%)", 50, 200, 112, (effect, value) => effect.Contrast = value)),
            Effect<InfraredFalseColorImageEffect>(
                "infrared_false_color",
                ImageEffectCategory.Filters,
                FloatSlider<InfraredFalseColorImageEffect>("intensity", "Intensity (%)", 0, 100, 68, (effect, value) => effect.Intensity = value),
                FloatSlider<InfraredFalseColorImageEffect>("foliage_shift", "Foliage shift (%)", 0, 100, 78, (effect, value) => effect.FoliageShift = value),
                FloatSlider<InfraredFalseColorImageEffect>("sky_darkening", "Sky darkening (%)", 0, 100, 44, (effect, value) => effect.SkyDarkening = value),
                FloatSlider<InfraredFalseColorImageEffect>("glow", "Glow (%)", 0, 100, 20, (effect, value) => effect.Glow = value),
                FloatSlider<InfraredFalseColorImageEffect>("contrast", "Contrast (%)", 50, 200, 118, (effect, value) => effect.Contrast = value)),
            Effect<JpegArtifactImageEffect>(
                "jpeg_artifact",
                ImageEffectCategory.Filters,
                FloatSlider<JpegArtifactImageEffect>("compression", "Compression (%)", 0, 100, 62, (effect, value) => effect.Compression = value),
                IntSlider<JpegArtifactImageEffect>("block_size", "Block size", 4, 32, 8, (effect, value) => effect.BlockSize = value),
                FloatSlider<JpegArtifactImageEffect>("chroma_bleed", "Chroma bleed (%)", 0, 100, 45, (effect, value) => effect.ChromaBleed = value),
                FloatSlider<JpegArtifactImageEffect>("ringing", "Ringing (%)", 0, 100, 28, (effect, value) => effect.Ringing = value)),
            Effect<SoftDiffusionImageEffect>(
                "soft_diffusion",
                ImageEffectCategory.Filters,
                FloatSlider<SoftDiffusionImageEffect>("amount", "Amount (%)", 0, 100, 52, (effect, value) => effect.Amount = value),
                FloatSlider<SoftDiffusionImageEffect>("radius", "Radius (px)", 0, 50, 12, (effect, value) => effect.Radius = value),
                FloatSlider<SoftDiffusionImageEffect>("highlight_bloom", "Highlight bloom (%)", 0, 100, 40, (effect, value) => effect.HighlightBloom = value),
                FloatSlider<SoftDiffusionImageEffect>("contrast_softening", "Contrast softening (%)", 0, 100, 28, (effect, value) => effect.ContrastSoftening = value),
                FloatSlider<SoftDiffusionImageEffect>("warmth", "Warmth (%)", 0, 100, 22, (effect, value) => effect.Warmth = value)),
            Effect<StarFilterImageEffect>(
                "star_filter",
                ImageEffectCategory.Filters,
                FloatSlider<StarFilterImageEffect>("threshold", "Threshold (%)", 0, 100, 74, (effect, value) => effect.Threshold = value),
                FloatSlider<StarFilterImageEffect>("length", "Length (%)", 0, 100, 40, (effect, value) => effect.Length = value),
                FloatSlider<StarFilterImageEffect>("strength", "Strength (%)", 0, 100, 58, (effect, value) => effect.Strength = value),
                FloatSlider<StarFilterImageEffect>("rotation", "Rotation (°)", -180, 180, 0, (effect, value) => effect.Rotation = value),
                FloatSlider<StarFilterImageEffect>("warmth", "Warmth (%)", 0, 100, 48, (effect, value) => effect.Warmth = value)),
            Effect<WetPlateCollodionImageEffect>(
                "wet_plate_collodion",
                ImageEffectCategory.Filters,
                FloatSlider<WetPlateCollodionImageEffect>("contrast", "Contrast (%)", 50, 200, 132, (effect, value) => effect.Contrast = value),
                FloatSlider<WetPlateCollodionImageEffect>("plate_stains", "Plate stains (%)", 0, 100, 34, (effect, value) => effect.PlateStains = value),
                FloatSlider<WetPlateCollodionImageEffect>("scratches", "Scratches (%)", 0, 100, 24, (effect, value) => effect.Scratches = value),
                FloatSlider<WetPlateCollodionImageEffect>("silvering", "Silvering (%)", 0, 100, 36, (effect, value) => effect.Silvering = value),
                FloatSlider<WetPlateCollodionImageEffect>("vignette", "Vignette (%)", 0, 100, 58, (effect, value) => effect.Vignette = value),
                IntNumeric<WetPlateCollodionImageEffect>("seed", "Seed", 0, int.MaxValue, 1855, (effect, value) => effect.Seed = value)),

            // --- Migrated from bespoke dialogs (IEIP0003 Phase 4) ---

            Effect<AnamorphicLensFlareImageEffect>(
                "anamorphic_lens_flare",
                ImageEffectCategory.Filters,
                FloatSlider<AnamorphicLensFlareImageEffect>("intensity", "Intensity (%)", 0, 100, 60, (effect, value) => effect.Intensity = value),
                FloatSlider<AnamorphicLensFlareImageEffect>("threshold", "Threshold (%)", 0, 100, 72, (effect, value) => effect.Threshold = value),
                FloatSlider<AnamorphicLensFlareImageEffect>("streak_length", "Streak length (%)", 0, 100, 55, (effect, value) => effect.StreakLength = value),
                FloatSlider<AnamorphicLensFlareImageEffect>("warmth", "Warmth (%)", 0, 100, 45, (effect, value) => effect.Warmth = value),
                FloatSlider<AnamorphicLensFlareImageEffect>("ghosting", "Ghosting (%)", 0, 100, 35, (effect, value) => effect.Ghosting = value)),

            Effect<EtchedGlassImageEffect>(
                "etched_glass",
                ImageEffectCategory.Filters,
                FloatSlider<EtchedGlassImageEffect>("frost", "Frost (%)", 0, 100, 48, (effect, value) => effect.Frost = value),
                FloatSlider<EtchedGlassImageEffect>("engrave", "Engrave (%)", 0, 100, 68, (effect, value) => effect.Engrave = value),
                FloatSlider<EtchedGlassImageEffect>("refraction", "Refraction (%)", 0, 100, 18, (effect, value) => effect.Refraction = value),
                FloatSlider<EtchedGlassImageEffect>("highlight", "Highlight (%)", 0, 100, 42, (effect, value) => effect.Highlight = value),
                FloatSlider<EtchedGlassImageEffect>("background_fade", "Background fade (%)", 0, 100, 38, (effect, value) => effect.BackgroundFade = value)),

            Effect<LiquidMercuryImageEffect>(
                "liquid_mercury",
                ImageEffectCategory.Filters,
                FloatSlider<LiquidMercuryImageEffect>("reflection", "Reflection (%)", 0, 100, 78, (effect, value) => effect.Reflection = value),
                FloatSlider<LiquidMercuryImageEffect>("ripple", "Ripple (%)", 0, 100, 42, (effect, value) => effect.Ripple = value),
                FloatSlider<LiquidMercuryImageEffect>("shine", "Shine (%)", 0, 100, 82, (effect, value) => effect.Shine = value),
                FloatSlider<LiquidMercuryImageEffect>("fluidity", "Fluidity (%)", 0, 100, 55, (effect, value) => effect.Fluidity = value),
                FloatSlider<LiquidMercuryImageEffect>("depth", "Depth (%)", 0, 100, 65, (effect, value) => effect.Depth = value)),

            Effect<NightVisionImageEffect>(
                "night_vision",
                ImageEffectCategory.Filters,
                FloatSlider<NightVisionImageEffect>("intensity", "Intensity (%)", 0, 100, 78, (effect, value) => effect.Intensity = value),
                FloatSlider<NightVisionImageEffect>("glow", "Glow (%)", 0, 100, 42, (effect, value) => effect.Glow = value),
                FloatSlider<NightVisionImageEffect>("noise", "Noise (%)", 0, 100, 18, (effect, value) => effect.Noise = value),
                FloatSlider<NightVisionImageEffect>("vignette", "Vignette (%)", 0, 100, 45, (effect, value) => effect.Vignette = value),
                FloatSlider<NightVisionImageEffect>("scanlines", "Scanlines (%)", 0, 100, 35, (effect, value) => effect.Scanlines = value)),

            Effect<OilSlickInterferenceImageEffect>(
                "oil_slick_interference",
                ImageEffectCategory.Filters,
                FloatSlider<OilSlickInterferenceImageEffect>("intensity", "Intensity (%)", 0, 100, 58, (effect, value) => effect.Intensity = value),
                FloatSlider<OilSlickInterferenceImageEffect>("scale", "Scale (%)", 0, 100, 70, (effect, value) => effect.Scale = value),
                FloatSlider<OilSlickInterferenceImageEffect>("darkness", "Darkness (%)", 0, 100, 45, (effect, value) => effect.Darkness = value),
                FloatSlider<OilSlickInterferenceImageEffect>("gloss", "Gloss (%)", 0, 100, 60, (effect, value) => effect.Gloss = value),
                FloatSlider<OilSlickInterferenceImageEffect>("shift", "Hue shift", 0, 360, 0, (effect, value) => effect.Shift = value, valueStringFormat: "{}{0:0}°")),

            Effect<PlasmaEnergyArcsImageEffect>(
                "plasma_energy_arcs",
                ImageEffectCategory.Filters,
                FloatSlider<PlasmaEnergyArcsImageEffect>("energy", "Energy (%)", 0, 100, 65, (effect, value) => effect.Energy = value),
                FloatSlider<PlasmaEnergyArcsImageEffect>("arc_density", "Arc density (%)", 0, 100, 46, (effect, value) => effect.ArcDensity = value),
                FloatSlider<PlasmaEnergyArcsImageEffect>("glow", "Glow (%)", 0, 100, 70, (effect, value) => effect.Glow = value),
                FloatSlider<PlasmaEnergyArcsImageEffect>("turbulence", "Turbulence (%)", 0, 100, 52, (effect, value) => effect.Turbulence = value),
                FloatSlider<PlasmaEnergyArcsImageEffect>("thickness", "Thickness (%)", 0, 100, 38, (effect, value) => effect.Thickness = value)),

            Effect<RustCorrosionImageEffect>(
                "rust_corrosion",
                ImageEffectCategory.Filters,
                FloatSlider<RustCorrosionImageEffect>("corrosion", "Corrosion (%)", 0, 100, 58, (effect, value) => effect.Corrosion = value),
                FloatSlider<RustCorrosionImageEffect>("pitting", "Pitting (%)", 0, 100, 44, (effect, value) => effect.Pitting = value),
                FloatSlider<RustCorrosionImageEffect>("streaks", "Streaks (%)", 0, 100, 36, (effect, value) => effect.Streaks = value),
                FloatSlider<RustCorrosionImageEffect>("dirt", "Dirt (%)", 0, 100, 26, (effect, value) => effect.Dirt = value),
                FloatSlider<RustCorrosionImageEffect>("edge_wear", "Edge wear (%)", 0, 100, 52, (effect, value) => effect.EdgeWear = value)),

            Effect<SmokeOverlayImageEffect>(
                "smoke_overlay",
                ImageEffectCategory.Filters,
                FloatSlider<SmokeOverlayImageEffect>("density", "Density (%)", 0, 100, 42, (effect, value) => effect.Density = value),
                FloatSlider<SmokeOverlayImageEffect>("scale", "Scale (%)", 0, 100, 68, (effect, value) => effect.Scale = value),
                FloatSlider<SmokeOverlayImageEffect>("drift", "Drift", -100, 100, 18, (effect, value) => effect.Drift = value),
                FloatSlider<SmokeOverlayImageEffect>("softness", "Softness (%)", 0, 100, 55, (effect, value) => effect.Softness = value),
                FloatSlider<SmokeOverlayImageEffect>("contrast", "Contrast (%)", 0, 100, 48, (effect, value) => effect.Contrast = value)),

            Effect<VhsTapeDamageImageEffect>(
                "vhs_tape_damage",
                ImageEffectCategory.Filters,
                FloatSlider<VhsTapeDamageImageEffect>("distortion", "Distortion (%)", 0, 100, 55, (effect, value) => effect.Distortion = value),
                FloatSlider<VhsTapeDamageImageEffect>("noise", "Noise (%)", 0, 100, 28, (effect, value) => effect.Noise = value),
                FloatSlider<VhsTapeDamageImageEffect>("color_bleed", "Color bleed (%)", 0, 100, 30, (effect, value) => effect.ColorBleed = value),
                FloatSlider<VhsTapeDamageImageEffect>("tracking", "Tracking (%)", 0, 100, 24, (effect, value) => effect.Tracking = value),
                FloatSlider<VhsTapeDamageImageEffect>("scanlines", "Scanlines (%)", 0, 100, 48, (effect, value) => effect.Scanlines = value)),

            Effect<XRayScanImageEffect>(
                "x_ray_scan",
                ImageEffectCategory.Filters,
                FloatSlider<XRayScanImageEffect>("contrast", "Contrast (%)", 0, 100, 70, (effect, value) => effect.Contrast = value),
                FloatSlider<XRayScanImageEffect>("glow", "Glow (%)", 0, 100, 60, (effect, value) => effect.Glow = value),
                FloatSlider<XRayScanImageEffect>("edge_boost", "Edge boost (%)", 0, 100, 68, (effect, value) => effect.EdgeBoost = value),
                FloatSlider<XRayScanImageEffect>("scanlines", "Scanlines (%)", 0, 100, 40, (effect, value) => effect.Scanlines = value),
                FloatSlider<XRayScanImageEffect>("noise", "Noise (%)", 0, 100, 14, (effect, value) => effect.Noise = value))
        ];
    }
}
