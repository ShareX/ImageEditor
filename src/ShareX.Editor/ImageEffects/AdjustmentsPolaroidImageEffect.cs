using SkiaSharp;
using ShareX.Editor.Helpers;

namespace ShareX.Editor.ImageEffects;

public class AdjustmentsPolaroidImageEffect : AdjustmentsImageEffect
{
    public override string Name => "Polaroid";
    public override string IconKey => "IconCameraRetro";
    public override SKBitmap Apply(SKBitmap source) 
    {
        float[] matrix = {
            1.438f, -0.062f, -0.062f, 0, 0,
            -0.122f, 1.378f, -0.122f, 0, 0,
            -0.016f, -0.016f, 1.483f, 0, 0,
            0,       0,       0,      1, 0
        };
        return ImageHelpers.ApplyColorMatrix(source, matrix);
    }
}
