using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class SliceImageEffect : ImageEffectBase
{
    public override string Id => "slice";
    public override string Name => "Slice";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.slice;
    public override string Description => "Slices the image.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<SliceImageEffect>("min_height", "Min Height", 1, 100, 10, (effect, value) => effect.MinHeight = value, isSnapToTickEnabled: false),
        EffectParameters.IntSlider<SliceImageEffect>("max_height", "Max Height", 1, 200, 100, (effect, value) => effect.MaxHeight = value, isSnapToTickEnabled: false),
        EffectParameters.IntSlider<SliceImageEffect>("min_shift", "Min Shift", 0, 100, 0, (effect, value) => effect.MinShift = value, isSnapToTickEnabled: false),
        EffectParameters.IntSlider<SliceImageEffect>("max_shift", "Max Shift", 0, 100, 10, (effect, value) => effect.MaxShift = value, isSnapToTickEnabled: false)
    ];

    public int MinHeight { get; set; } = 10;
    public int MaxHeight { get; set; } = 100;
    public int MinShift { get; set; }
    public int MaxShift { get; set; } = 10;

    public SliceImageEffect()
    {
    }

    public SliceImageEffect(int minHeight, int maxHeight, int minShift, int maxShift)
    {
        MinHeight = minHeight;
        MaxHeight = maxHeight;
        MinShift = minShift;
        MaxShift = maxShift;
    }

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (MinHeight <= 0 && MaxHeight <= 0) return source.Copy();

        int minSliceHeight = Math.Max(1, Math.Min(MinHeight, MaxHeight));
        int maxSliceHeight = Math.Max(minSliceHeight, Math.Max(MinHeight, MaxHeight));

        int minSliceShift = Math.Min(Math.Abs(MinShift), Math.Abs(MaxShift));
        int maxSliceShift = Math.Max(Math.Abs(MinShift), Math.Abs(MaxShift));

        Random rand = Random.Shared;
        int maxAbsShift = maxSliceShift;
        int newWidth = source.Width + maxAbsShift * 2;

        SKBitmap result = new(newWidth, source.Height);
        using SKCanvas canvas = new(result);
        canvas.Clear(SKColors.Transparent);

        int y = 0;
        while (y < source.Height)
        {
            int sliceHeight = rand.Next(minSliceHeight, maxSliceHeight + 1);
            sliceHeight = Math.Min(sliceHeight, source.Height - y);

            int shift = rand.Next(2) == 0
                ? rand.Next(-maxSliceShift, -minSliceShift + 1)
                : rand.Next(minSliceShift, maxSliceShift + 1);

            SKRect srcRect = new(0, y, source.Width, y + sliceHeight);
            SKRect dstRect = new(maxAbsShift + shift, y, maxAbsShift + shift + source.Width, y + sliceHeight);

            canvas.DrawBitmap(source, srcRect, dstRect);
            y += sliceHeight;
        }

        return result;
    }
}
