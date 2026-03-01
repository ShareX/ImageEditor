using ShareX.ImageEditor.ImageEffects.Adjustments;
using ShareX.ImageEditor.ImageEffects.Drawings;
using ShareX.ImageEditor.ImageEffects.Filters;
using ShareX.ImageEditor.ImageEffects.Manipulations;

namespace ShareX.ImageEditor.ImageEffects;

public static class ImageEffectRegistry
{
    public static IReadOnlyList<ImageEffect> Effects { get; }

    static ImageEffectRegistry()
    {
        var effects = new List<ImageEffect>
        {
            // Manipulations - Rotate
            RotateImageEffect.Clockwise90,
            RotateImageEffect.CounterClockwise90,
            RotateImageEffect.Rotate180,
            
            // Manipulations - Flip
            FlipImageEffect.Horizontal,
            FlipImageEffect.Vertical,
            new FlipImageEffect(),
            
            // Manipulations - Resize (parameterized)
            new ResizeImageEffect(),
            new ScaleImageEffect(),
            new PerspectiveWarpImageEffect(),
            new PinchBulgeImageEffect(),
            new TwirlImageEffect(),
            new DisplacementMapImageEffect(),
            new AutoCropImageEffect(),
            new Rotate3DBoxImageEffect(),
            
            // Adjustments
            new BrightnessImageEffect(),
            new AutoContrastImageEffect(),
            new ContrastImageEffect(),
            new ExposureImageEffect(),
            new HueImageEffect(),
            new LevelsImageEffect(),
            new PosterizeImageEffect(),
            new SaturationImageEffect(),
            new SolarizeImageEffect(),
            new ShadowsHighlightsImageEffect(),
            new TemperatureTintImageEffect(),
            new ThresholdImageEffect(),
            new GammaImageEffect(),
            new AlphaImageEffect(),
            new ColorMatrixImageEffect(),
            new ColorizeImageEffect(),
            new SelectiveColorImageEffect(),
            new ReplaceColorImageEffect(),
            new VibranceImageEffect(),
            
            // Filters
            new AddNoiseImageEffect(),
            new ColorDepthImageEffect(),
            new ConvolutionMatrixImageEffect(),
            new EdgeDetectImageEffect(),
            new EmbossImageEffect(),
            new GaussianBlurImageEffect(),
            new MeanRemovalImageEffect(),
            new MedianFilterImageEffect(),
            new MotionBlurImageEffect(),
            new OilPaintImageEffect(),
            new RGBSplitImageEffect(),
            new SobelEdgeImageEffect(),
            new SmoothImageEffect(),
            new BlurImageEffect(),
            new PixelateImageEffect(),
            new SharpenImageEffect(),
            new UnsharpMaskImageEffect(),
            new VignetteImageEffect(),
            new WaveEdgeImageEffect(),
            
            // Adjustments - Color Filters
            new InvertImageEffect(),
            new GrayscaleImageEffect(),
            new BlackAndWhiteImageEffect(),
            new SepiaImageEffect(),
            new PolaroidImageEffect(),

            // Drawings
            new DrawBackgroundEffect(),
            new DrawBackgroundImageEffect(),
            new DrawCheckerboardEffect(),
            new DrawImageEffect(),
            new DrawParticlesEffect(),
            new DrawTextEffect()
        };

        Effects = effects.AsReadOnly();
    }

    public static IEnumerable<ImageEffect> GetByCategory(ImageEffectCategory category)
    {
        return Effects.Where(e => e.Category == category);
    }
}
