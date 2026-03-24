using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public class KaleidoscopeImageEffect : ImageEffect
{
    public override string Name => "Kaleidoscope";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override bool HasParameters => true;

    public int Segments { get; set; } = 8;
    public float Rotation { get; set; } = 0f;
    public float Zoom { get; set; } = 100f;
    public float CenterXPercentage { get; set; } = 50f;
    public float CenterYPercentage { get; set; } = 50f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int segmentCount = Math.Clamp(Segments, 2, 24);
        float zoom = Math.Clamp(Zoom, 20f, 300f) / 100f;
        float rotation = Rotation * (MathF.PI / 180f);
        float centerX = DistortionEffectHelper.PercentageToX(source.Width, CenterXPercentage);
        float centerY = DistortionEffectHelper.PercentageToY(source.Height, CenterYPercentage);
        float segmentAngle = (MathF.PI * 2f) / segmentCount;

        int width = source.Width;
        int height = source.Height;
        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];

        Parallel.For(0, height, y =>
        {
            int row = y * width;
            for (int x = 0; x < width; x++)
            {
                float dx = (x - centerX) / zoom;
                float dy = (y - centerY) / zoom;
                float distance = MathF.Sqrt((dx * dx) + (dy * dy));
                float angle = DistortionEffectHelper.WrapAngle(MathF.Atan2(dy, dx) - rotation);

                int segmentIndex = (int)MathF.Floor(angle / segmentAngle);
                float localAngle = angle - (segmentIndex * segmentAngle);
                if ((segmentIndex & 1) == 1)
                {
                    localAngle = segmentAngle - localAngle;
                }

                float sampleAngle = localAngle + rotation;
                float sampleX = centerX + (MathF.Cos(sampleAngle) * distance);
                float sampleY = centerY + (MathF.Sin(sampleAngle) * distance);

                dstPixels[row + x] = DistortionEffectHelper.SampleClamped(srcPixels, width, height, sampleX, sampleY);
            }
        });

        return DistortionEffectHelper.CreateBitmap(source, width, height, dstPixels);
    }
}
