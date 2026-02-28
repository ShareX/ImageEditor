using SkiaSharp;

namespace ShareX.ImageEditor.ImageEffects.Filters;

public class RGBSplitImageEffect : ImageEffect
{
    public override string Name => "RGB split";
    public override string IconKey => "IconAdjust";
    public override bool HasParameters => true;

    public int OffsetRedX { get; set; } = -5;
    public int OffsetRedY { get; set; }

    public int OffsetGreenX { get; set; }
    public int OffsetGreenY { get; set; }

    public int OffsetBlueX { get; set; } = 5;
    public int OffsetBlueY { get; set; }

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        SKBitmap result = new SKBitmap(source.Width, source.Height, source.ColorType, source.AlphaType);
        int right = source.Width - 1;
        int bottom = source.Height - 1;

        for (int y = 0; y < source.Height; y++)
        {
            for (int x = 0; x < source.Width; x++)
            {
                SKColor colorR = source.GetPixel(
                    Clamp(x - OffsetRedX, 0, right),
                    Clamp(y - OffsetRedY, 0, bottom));
                SKColor colorG = source.GetPixel(
                    Clamp(x - OffsetGreenX, 0, right),
                    Clamp(y - OffsetGreenY, 0, bottom));
                SKColor colorB = source.GetPixel(
                    Clamp(x - OffsetBlueX, 0, right),
                    Clamp(y - OffsetBlueY, 0, bottom));

                byte red = (byte)(colorR.Red * colorR.Alpha / 255);
                byte green = (byte)(colorG.Green * colorG.Alpha / 255);
                byte blue = (byte)(colorB.Blue * colorB.Alpha / 255);
                byte alpha = (byte)((colorR.Alpha + colorG.Alpha + colorB.Alpha) / 3);

                result.SetPixel(x, y, new SKColor(red, green, blue, alpha));
            }
        }

        return result;
    }

    private static int Clamp(int value, int min, int max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}
