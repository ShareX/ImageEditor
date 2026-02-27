using SkiaSharp;


namespace ShareX.ImageEditor.ImageEffects.Adjustments;

public class BlackAndWhiteImageEffect : ImageEffect
{
    public override string Name => "Black and White";
    public override string IconKey => "IconAdjust";
    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        // Pass 1: luminance grayscale (ITU-R BT.709 coefficients)
        float[] grayscale = {
            0.2126f, 0.7152f, 0.0722f, 0, 0,
            0.2126f, 0.7152f, 0.0722f, 0, 0,
            0.2126f, 0.7152f, 0.0722f, 0, 0,
            0,       0,       0,       1, 0
        };
        using var step1 = ApplyColorMatrix(source, grayscale);

        // Pass 2: hard threshold (< 128 → 0, ≥ 128 → 255); alpha forced to 255
        byte[] table = new byte[256];
        for (int i = 0; i < 256; i++) table[i] = i < 128 ? (byte)0 : (byte)255;
        byte[] alphaTable = new byte[256];
        for (int i = 0; i < 256; i++) alphaTable[i] = 255;
        using var filter = SKColorFilter.CreateTable(alphaTable, table, table, table);
        return ApplyColorFilter(step1, filter);
    }
}

