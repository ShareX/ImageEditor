using SkiaSharp;
using ShareX.Editor.Helpers;

namespace ShareX.Editor.ImageEffects;

public abstract class FilterImageEffect : ImageEffect
{
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
}

public class InvertImageEffect : FilterImageEffect
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

public class BlackAndWhiteImageEffect : FilterImageEffect
{
    public override string Name => "Black and White";
    public override string IconKey => "IconAdjust";
    public override SKBitmap Apply(SKBitmap source) 
    {
        return ImageHelpers.ApplyPixelOperation(source, (color) =>
        {
            float lum = 0.2126f * color.Red + 0.7152f * color.Green + 0.0722f * color.Blue;
            return lum > 127 ? SKColors.White : SKColors.Black;
        });
    }
}

public class GrayscaleImageEffect : FilterImageEffect
{
    public override string Name => "Grayscale";
    public override string IconKey => "IconCloud";
    public override bool HasParameters => true;
    public float Strength { get; set; } = 100f;

    public override SKBitmap Apply(SKBitmap source) 
    {
        float strength = Strength;
        if (strength >= 100)
        {
            float[] matrix = {
                0.2126f, 0.7152f, 0.0722f, 0, 0,
                0.2126f, 0.7152f, 0.0722f, 0, 0,
                0.2126f, 0.7152f, 0.0722f, 0, 0,
                0,       0,       0,       1, 0
            };
            return ImageHelpers.ApplyColorMatrix(source, matrix);
        }
        else if (strength <= 0)
        {
            return source.Copy();
        }
        else
        {
            float s = strength / 100f;
            float invS = 1f - s;
            
            float[] matrix = {
                0.2126f * s + invS, 0.7152f * s,        0.0722f * s,        0, 0,
                0.2126f * s,        0.7152f * s + invS, 0.0722f * s,        0, 0,
                0.2126f * s,        0.7152f * s,        0.0722f * s + invS, 0, 0,
                0,                  0,                  0,                  1, 0
            };
            return ImageHelpers.ApplyColorMatrix(source, matrix);
        }
    }
}

public class SepiaImageEffect : FilterImageEffect
{
    public override string Name => "Sepia";
    public override string IconKey => "IconCoffee";
    public override bool HasParameters => true;
    public float Strength { get; set; } = 100f;

    public override SKBitmap Apply(SKBitmap source) 
    {
        float s = Math.Clamp(Strength / 100f, 0f, 1f);
        
        if (s <= 0) return source.Copy();
        
        // Sepia matrix
        float[] sepiaMatrix = {
            0.393f, 0.769f, 0.189f, 0, 0,
            0.349f, 0.686f, 0.168f, 0, 0,
            0.272f, 0.534f, 0.131f, 0, 0,
            0,      0,      0,      1, 0
        };
        
        // Identity matrix
        float[] identityMatrix = {
            1, 0, 0, 0, 0,
            0, 1, 0, 0, 0,
            0, 0, 1, 0, 0,
            0, 0, 0, 1, 0
        };
        
        // Interpolate between identity and sepia
        float[] matrix = new float[20];
        for (int i = 0; i < 20; i++)
        {
            matrix[i] = identityMatrix[i] * (1 - s) + sepiaMatrix[i] * s;
        }
        
        return ImageHelpers.ApplyColorMatrix(source, matrix);
    }
}

public class PolaroidImageEffect : FilterImageEffect
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

public class BlurImageEffect : FilterImageEffect
{
    public override string Name => "Blur";
    public override string IconKey => "IconCloud";
    public override bool HasParameters => true;
    public int Radius { get; set; } = 5;

    public override SKBitmap Apply(SKBitmap source) 
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (Radius <= 0) return source.Copy();

        return ImageHelpers.BoxBlur(source, Radius);
    }
}

public class PixelateImageEffect : FilterImageEffect
{
    public override string Name => "Pixelate";
    public override string IconKey => "IconGrid";
    public override bool HasParameters => true;
    public int Size { get; set; } = 10;

    public override SKBitmap Apply(SKBitmap source) 
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (Size <= 1) return source.Copy();

        return ImageHelpers.Pixelate(source, Size);
    }
}

public class SharpenImageEffect : FilterImageEffect
{
    public override string Name => "Sharpen";
    public override string IconKey => "IconMagic";
    public override bool HasParameters => true;
    public int Strength { get; set; } = 50;

    public override SKBitmap Apply(SKBitmap source) 
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (Strength <= 0) return source.Copy();

        return ImageHelpers.Sharpen(source, Strength / 100f);
    }
}
