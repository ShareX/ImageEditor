using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class ThresholdImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "threshold";
    public override string Name => "Threshold";
    public override string IconKey => "Binary";
    public override string Description => "Applies a contrast threshold.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<ThresholdImageEffect>("value", "Threshold", 0, 255, 128, (effect, value) => effect.Value = value)
    ];

    public int Value { get; set; } = 128;

    public override SKBitmap Apply(SKBitmap source)
    {
        int threshold = Math.Clamp(Value, 0, 255);

        return ApplyPixelOperation(source, c =>
        {
            int luma = ((c.Red * 77) + (c.Green * 150) + (c.Blue * 29)) >> 8;
            byte bw = (byte)(luma >= threshold ? 255 : 0);
            return new SKColor(bw, bw, bw, c.Alpha);
        });
    }
}

