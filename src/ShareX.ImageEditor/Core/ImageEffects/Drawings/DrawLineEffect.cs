using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using SkiaSharp;
using ShareX.ImageEditor.Presentation.Theming;

namespace ShareX.ImageEditor.Core.ImageEffects.Drawings;

public sealed class DrawLineEffect : ImageEffectBase
{
    public override string Id => "draw_line";
    public override string Name => "Line";
    public override ImageEffectCategory Category => ImageEffectCategory.Drawings;
    public override string IconKey => LucideIcons.minus;
    public override string Description => "Draws a line on the image.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntNumeric<DrawLineEffect>("start_x", "Start X", -10000, 10000, 0, (e, v) => e.StartPoint = new SKPointI(v, e.StartPoint.Y)),
        EffectParameters.IntNumeric<DrawLineEffect>("start_y", "Start Y", -10000, 10000, 0, (e, v) => e.StartPoint = new SKPointI(e.StartPoint.X, v)),
        EffectParameters.IntNumeric<DrawLineEffect>("end_x", "End X", -10000, 10000, 200, (e, v) => e.EndPoint = new SKPointI(v, e.EndPoint.Y)),
        EffectParameters.IntNumeric<DrawLineEffect>("end_y", "End Y", -10000, 10000, 0, (e, v) => e.EndPoint = new SKPointI(e.EndPoint.X, v)),
        EffectParameters.Color<DrawLineEffect>("color", "Color", new SKColor(255, 255, 255, 255), (e, v) => e.Color = v),
        EffectParameters.FloatSlider<DrawLineEffect>("thickness", "Thickness", 0.5, 100, 4, (e, v) => e.Thickness = v)
    ];

    public SKPointI StartPoint { get; set; } = new(0, 0);

    public SKPointI EndPoint { get; set; } = new(200, 0);

    public SKColor Color { get; set; } = new SKColor(255, 255, 255, 255);

    public float Thickness { get; set; } = 4f;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (Thickness <= 0 || Color.Alpha == 0)
        {
            return source.Copy();
        }

        SKBitmap result = source.Copy();
        using SKCanvas canvas = new(result);
        using SKPaint paint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            Color = Color,
            StrokeWidth = Thickness,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };

        canvas.DrawLine(StartPoint.X, StartPoint.Y, EndPoint.X, EndPoint.Y, paint);
        return result;
    }
}
