using SkiaSharp;
using ShareX.Editor.Helpers;

namespace ShareX.Editor.ImageEffects;

public class AdjustmentsInvertImageEffect : AdjustmentsImageEffect
{
    public override string Name => "Invert";
    public override string IconKey => "IconExchangeAlt";
    public override SKBitmap Apply(SKBitmap source) 
    {
        float[] matrix = {
            -1,  0,  0, 0, 1,
             0, -1,  0, 0, 1,
             0,  0, -1, 0, 1,
             0,  0,  0, 1, 0
        };
        return ImageHelpers.ApplyColorMatrix(source, matrix);
    }
}
