using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public enum ChannelSwapMode
{
    RedGreen,
    RedBlue,
    GreenBlue,
    RotateRGB,
    RotateBGR
}

public sealed class ChannelSwapImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "channel_swap";
    public override string Name => "Channel swap";
    public override string IconKey => LucideIcons.shuffle;
    public override string Description => "Swaps color channels for creative color effects.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.Enum<ChannelSwapImageEffect, ChannelSwapMode>(
            "mode", "Swap mode", ChannelSwapMode.RedGreen, (e, v) => e.Mode = v,
            new (string, ChannelSwapMode)[]
            {
                ("Red \u2194 Green", ChannelSwapMode.RedGreen),
                ("Red \u2194 Blue", ChannelSwapMode.RedBlue),
                ("Green \u2194 Blue", ChannelSwapMode.GreenBlue),
                ("Rotate R\u2192G\u2192B", ChannelSwapMode.RotateRGB),
                ("Rotate B\u2192G\u2192R", ChannelSwapMode.RotateBGR)
            })
    ];

    public ChannelSwapMode Mode { get; set; } = ChannelSwapMode.RedGreen;

    public override SKBitmap Apply(SKBitmap source)
    {
        return ApplyPixelOperation(source, c => Mode switch
        {
            ChannelSwapMode.RedGreen => new SKColor(c.Green, c.Red, c.Blue, c.Alpha),
            ChannelSwapMode.RedBlue => new SKColor(c.Blue, c.Green, c.Red, c.Alpha),
            ChannelSwapMode.GreenBlue => new SKColor(c.Red, c.Blue, c.Green, c.Alpha),
            ChannelSwapMode.RotateRGB => new SKColor(c.Blue, c.Red, c.Green, c.Alpha),
            ChannelSwapMode.RotateBGR => new SKColor(c.Green, c.Blue, c.Red, c.Alpha),
            _ => c
        });
    }
}
