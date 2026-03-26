using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Adjustments;

public sealed class GammaImageEffect : AdjustmentImageEffectBase
{
    public override string Id => "gamma";
    public override string Name => "Gamma";
    public override string IconKey => "Gauge";
    public override string Description => "Adjusts the gamma level.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.FloatSlider<GammaImageEffect>("amount", "Amount", 0.1, 5, 1, (effect, value) => effect.Amount = value, tickFrequency: 0.1, isSnapToTickEnabled: false, valueStringFormat: "{}{0:0.0}")
    ];

    public float Amount { get; set; } = 1f;

    private float _cachedAmount = float.NaN;
    private byte[]? _cachedTable;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (_cachedTable is null || _cachedAmount != Amount)
        {
            _cachedAmount = Amount;
            _cachedTable = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                float val = i / 255f;
                float corrected = (float)Math.Pow(val, 1.0 / Amount);
                _cachedTable[i] = (byte)(Math.Max(0, Math.Min(1, corrected)) * 255);
            }
        }

        using var filter = SKColorFilter.CreateTable(null, _cachedTable, _cachedTable, _cachedTable);
        return ApplyColorFilter(source, filter);
    }
}

