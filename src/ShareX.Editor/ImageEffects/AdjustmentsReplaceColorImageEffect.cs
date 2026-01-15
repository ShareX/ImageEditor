using SkiaSharp;
using ShareX.Editor.Helpers;

namespace ShareX.Editor.ImageEffects;

public class AdjustmentsReplaceColorImageEffect : AdjustmentsImageEffect
{
    public override string Name => "Replace Color";
    public override string IconKey => "IconSync";
    public SKColor TargetColor { get; set; }
    public SKColor ReplaceColor { get; set; }
    public float Tolerance { get; set; }

    public override SKBitmap Apply(SKBitmap source) 
    {
         int tol = (int)(Tolerance * 2.55f);
         return ApplyPixelOperation(source, (c) =>
         {
             if (ImageHelpers.ColorsMatch(c, TargetColor, tol))
             {
                 return ReplaceColor;
             }
             return c;
         });
    }
}
