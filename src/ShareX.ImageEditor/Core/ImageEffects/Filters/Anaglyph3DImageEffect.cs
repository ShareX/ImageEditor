using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public enum AnaglyphMode
{
    RedCyan,
    AmberBlue,
    GreenMagenta
}

public class Anaglyph3DImageEffect : FilterImageEffect
{
    public override string Name => "Anaglyph 3D";
    public override bool HasParameters => true;

    public AnaglyphMode Mode { get; set; } = AnaglyphMode.RedCyan;
    public int SeparationX { get; set; } = 10;
    public int SeparationY { get; set; }
    public float GhostReduction { get; set; } = 45f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        float ghostReduction = Math.Clamp(GhostReduction, 0f, 100f) / 100f;
        if (SeparationX == 0 && SeparationY == 0)
        {
            return source.Copy();
        }

        int width = source.Width;
        int height = source.Height;
        float halfX = SeparationX * 0.5f;
        float halfY = SeparationY * 0.5f;
        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];

        Parallel.For(0, height, y =>
        {
            int row = y * width;
            for (int x = 0; x < width; x++)
            {
                SKColor left = AnalogEffectHelper.Sample(srcPixels, width, height, x - halfX, y - halfY);
                SKColor right = AnalogEffectHelper.Sample(srcPixels, width, height, x + halfX, y + halfY);

                float leftGray = AnalogEffectHelper.Luminance01(left);
                float rightGray = AnalogEffectHelper.Luminance01(right);
                float r;
                float g;
                float b;

                switch (Mode)
                {
                    case AnaglyphMode.AmberBlue:
                        r = ProceduralEffectHelper.Lerp(left.Red / 255f, leftGray, ghostReduction * 0.85f);
                        g = ProceduralEffectHelper.Lerp(left.Green / 255f, leftGray, ghostReduction * 0.50f);
                        b = ProceduralEffectHelper.Lerp(right.Blue / 255f, rightGray, ghostReduction);
                        break;
                    case AnaglyphMode.GreenMagenta:
                        r = ProceduralEffectHelper.Lerp(right.Red / 255f, rightGray, ghostReduction * 0.85f);
                        g = ProceduralEffectHelper.Lerp(left.Green / 255f, leftGray, ghostReduction);
                        b = ProceduralEffectHelper.Lerp(right.Blue / 255f, rightGray, ghostReduction * 0.85f);
                        break;
                    default:
                        r = ProceduralEffectHelper.Lerp(left.Red / 255f, leftGray, ghostReduction);
                        g = ProceduralEffectHelper.Lerp(right.Green / 255f, rightGray, ghostReduction * 0.90f);
                        b = ProceduralEffectHelper.Lerp(right.Blue / 255f, rightGray, ghostReduction);
                        break;
                }

                dstPixels[row + x] = new SKColor(
                    ProceduralEffectHelper.ClampToByte(r * 255f),
                    ProceduralEffectHelper.ClampToByte(g * 255f),
                    ProceduralEffectHelper.ClampToByte(b * 255f),
                    (byte)((left.Alpha + right.Alpha) / 2));
            }
        });

        return AnalogEffectHelper.CreateBitmap(source, dstPixels);
    }
}
