using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public enum DatamoshDirection
{
    Horizontal,
    Vertical
}

public class DatamoshSmearImageEffect : FilterImageEffect
{
    public override string Name => "Datamosh smear";
    public override bool HasParameters => true;

    public DatamoshDirection Direction { get; set; } = DatamoshDirection.Horizontal;
    public float SmearAmount { get; set; } = 58f;
    public float Corruption { get; set; } = 36f;
    public int BlockSize { get; set; } = 12;
    public float Drift { get; set; } = 24f;
    public float ChannelSplit { get; set; } = 25f;
    public int Seed { get; set; } = 9011;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        float smear = Math.Clamp(SmearAmount, 0f, 100f) / 100f;
        float corruption = Math.Clamp(Corruption, 0f, 100f) / 100f;
        int blockSize = Math.Clamp(BlockSize, 4, 64);
        float drift = Math.Clamp(Drift, -100f, 100f);
        float channelSplit = Math.Clamp(ChannelSplit, 0f, 100f) / 100f * 7f;

        if (smear <= 0.0001f && corruption <= 0.0001f && Math.Abs(drift) <= 0.0001f)
        {
            return source.Copy();
        }

        int width = source.Width;
        int height = source.Height;
        SKColor[] srcPixels = source.Pixels;
        SKColor[] dstPixels = new SKColor[srcPixels.Length];

        float smearDistance = smear * blockSize * 5.5f;

        if (Direction == DatamoshDirection.Horizontal)
        {
            Parallel.For(0, height, y =>
            {
                int row = y * width;
                float rowNoise = (ProceduralEffectHelper.Hash01(y / Math.Max(1, blockSize), 7, Seed) * 2f) - 1f;
                float baseShift = (MathF.Sin((y * 0.09f) + 0.4f) + (rowNoise * 1.15f)) * drift * 0.34f;
                float carry = 0f;

                for (int blockX = 0; blockX < width; blockX += blockSize)
                {
                    int blockIndex = blockX / blockSize;
                    float trigger = ProceduralEffectHelper.Hash01(blockIndex, y / Math.Max(1, blockSize), Seed ^ 313);

                    if (trigger > 1f - (corruption * 0.78f))
                    {
                        float sign = (ProceduralEffectHelper.Hash01(blockIndex, y, Seed ^ 743) * 2f) - 1f;
                        carry += sign * smearDistance * (0.35f + trigger);
                    }
                    else
                    {
                        carry *= 0.82f;
                    }

                    int x1 = Math.Min(width, blockX + blockSize);
                    for (int x = blockX; x < x1; x++)
                    {
                        float fx = x - carry - baseShift;
                        SKColor sr = AnalogEffectHelper.Sample(srcPixels, width, height, fx + channelSplit, y);
                        SKColor sg = AnalogEffectHelper.Sample(srcPixels, width, height, fx, y);
                        SKColor sb = AnalogEffectHelper.Sample(srcPixels, width, height, fx - channelSplit, y);

                        dstPixels[row + x] = new SKColor(sr.Red, sg.Green, sb.Blue, sg.Alpha);
                    }
                }
            });
        }
        else
        {
            Parallel.For(0, width, x =>
            {
                float columnNoise = (ProceduralEffectHelper.Hash01(x / Math.Max(1, blockSize), 11, Seed) * 2f) - 1f;
                float baseShift = (MathF.Cos((x * 0.07f) + 1.1f) + (columnNoise * 1.20f)) * drift * 0.34f;
                float carry = 0f;

                for (int blockY = 0; blockY < height; blockY += blockSize)
                {
                    int blockIndex = blockY / blockSize;
                    float trigger = ProceduralEffectHelper.Hash01(x / Math.Max(1, blockSize), blockIndex, Seed ^ 919);

                    if (trigger > 1f - (corruption * 0.78f))
                    {
                        float sign = (ProceduralEffectHelper.Hash01(x, blockIndex, Seed ^ 1597) * 2f) - 1f;
                        carry += sign * smearDistance * (0.35f + trigger);
                    }
                    else
                    {
                        carry *= 0.82f;
                    }

                    int y1 = Math.Min(height, blockY + blockSize);
                    for (int y = blockY; y < y1; y++)
                    {
                        float fy = y - carry - baseShift;
                        SKColor sr = AnalogEffectHelper.Sample(srcPixels, width, height, x, fy + channelSplit);
                        SKColor sg = AnalogEffectHelper.Sample(srcPixels, width, height, x, fy);
                        SKColor sb = AnalogEffectHelper.Sample(srcPixels, width, height, x, fy - channelSplit);

                        dstPixels[(y * width) + x] = new SKColor(sr.Red, sg.Green, sb.Blue, sg.Alpha);
                    }
                }
            });
        }

        return AnalogEffectHelper.CreateBitmap(source, dstPixels);
    }
}
