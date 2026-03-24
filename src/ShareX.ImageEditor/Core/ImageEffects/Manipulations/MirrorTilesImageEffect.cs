using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Manipulations;

public class MirrorTilesImageEffect : ImageEffect
{
    public override string Name => "Mirror tiles";
    public override ImageEffectCategory Category => ImageEffectCategory.Manipulations;
    public override bool HasParameters => true;

    public int Columns { get; set; } = 3;
    public int Rows { get; set; } = 3;
    public bool MirrorAlternateColumns { get; set; } = true;
    public bool MirrorAlternateRows { get; set; } = true;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int columns = Math.Clamp(Columns, 1, 16);
        int rows = Math.Clamp(Rows, 1, 16);

        if (columns == 1 && rows == 1 && !MirrorAlternateColumns && !MirrorAlternateRows)
        {
            return source.Copy();
        }

        int width = source.Width;
        int height = source.Height;
        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];

        Parallel.For(0, height, y =>
        {
            int row = y * width;
            float tiledY = (((y + 0.5f) / height) * rows);
            int tileY = Math.Min(rows - 1, Math.Max(0, (int)MathF.Floor(tiledY)));
            float localY = tiledY - tileY;
            if (tileY == rows - 1)
            {
                localY = Math.Min(1f, localY);
            }

            if (MirrorAlternateRows && (tileY & 1) == 1)
            {
                localY = 1f - localY;
            }

            float sampleY = localY * Math.Max(0, height - 1);

            for (int x = 0; x < width; x++)
            {
                float tiledX = (((x + 0.5f) / width) * columns);
                int tileX = Math.Min(columns - 1, Math.Max(0, (int)MathF.Floor(tiledX)));
                float localX = tiledX - tileX;
                if (tileX == columns - 1)
                {
                    localX = Math.Min(1f, localX);
                }

                if (MirrorAlternateColumns && (tileX & 1) == 1)
                {
                    localX = 1f - localX;
                }

                float sampleX = localX * Math.Max(0, width - 1);
                dstPixels[row + x] = DistortionEffectHelper.SampleClamped(srcPixels, width, height, sampleX, sampleY);
            }
        });

        return DistortionEffectHelper.CreateBitmap(source, width, height, dstPixels);
    }
}
