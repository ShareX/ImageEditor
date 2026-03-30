using ShareX.ImageEditor.Core.ImageEffects.Drawings;
using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class FakeCursorImageEffect : ImageEffectBase
{
    public override string Id => "fake_cursor";
    public override string Name => "Fake cursor";
    public override ImageEffectCategory Category => ImageEffectCategory.Drawings;
    public override string IconKey => LucideIcons.mouse_pointer;
    public override string Description => "Draws a fake mouse cursor on the image.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.Enum<FakeCursorImageEffect, DrawingPlacement>(
            "placement", "Placement", DrawingPlacement.MiddleCenter, (e, v) => e.Placement = v,
            new (string Label, DrawingPlacement Value)[]
            {
                ("Top left", DrawingPlacement.TopLeft),
                ("Top center", DrawingPlacement.TopCenter),
                ("Top right", DrawingPlacement.TopRight),
                ("Middle left", DrawingPlacement.MiddleLeft),
                ("Middle center", DrawingPlacement.MiddleCenter),
                ("Middle right", DrawingPlacement.MiddleRight),
                ("Bottom left", DrawingPlacement.BottomLeft),
                ("Bottom center", DrawingPlacement.BottomCenter),
                ("Bottom right", DrawingPlacement.BottomRight)
            }),
        EffectParameters.IntNumeric<FakeCursorImageEffect>("offset_x", "Offset X", -10000, 10000, 0, (e, v) => e.Offset = new SKPointI(v, e.Offset.Y)),
        EffectParameters.IntNumeric<FakeCursorImageEffect>("offset_y", "Offset Y", -10000, 10000, 0, (e, v) => e.Offset = new SKPointI(e.Offset.X, v)),
        EffectParameters.FloatSlider<FakeCursorImageEffect>("scale", "Scale", 0.5, 5, 1, (e, v) => e.Scale = v),
        EffectParameters.Color<FakeCursorImageEffect>("fill_color", "Fill color", SKColors.White, (e, v) => e.FillColor = v),
        EffectParameters.Color<FakeCursorImageEffect>("border_color", "Border color", SKColors.Black, (e, v) => e.BorderColor = v)
    ];

    public DrawingPlacement Placement { get; set; } = DrawingPlacement.MiddleCenter;
    public SKPointI Offset { get; set; } = new(0, 0);
    public float Scale { get; set; } = 1f;
    public SKColor FillColor { get; set; } = SKColors.White;
    public SKColor BorderColor { get; set; } = SKColors.Black;

    // Standard Windows arrow cursor shape (normalized to ~21x25 base)
    private static readonly SKPoint[] CursorOutline =
    [
        new(0, 0),
        new(0, 20.4f),
        new(5.7f, 15.6f),
        new(9.6f, 23.4f),
        new(12.6f, 22.2f),
        new(8.7f, 14.4f),
        new(15.3f, 14.4f),
    ];

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        float scale = Math.Clamp(Scale, 0.5f, 5f);

        // Compute cursor size for placement
        float cursorWidth = 15.3f * scale;
        float cursorHeight = 23.4f * scale;
        SKSizeI cursorSize = new((int)MathF.Ceiling(cursorWidth), (int)MathF.Ceiling(cursorHeight));

        SKPointI position = DrawingEffectHelpers.GetPosition(
            Placement,
            Offset,
            new SKSizeI(source.Width, source.Height),
            cursorSize);

        SKBitmap result = source.Copy();
        using SKCanvas canvas = new(result);

        canvas.Save();
        canvas.Translate(position.X, position.Y);
        canvas.Scale(scale);

        using SKPath cursorPath = new();
        cursorPath.MoveTo(CursorOutline[0]);
        for (int i = 1; i < CursorOutline.Length; i++)
        {
            cursorPath.LineTo(CursorOutline[i]);
        }
        cursorPath.Close();

        // Draw border/outline
        using (SKPaint borderPaint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            Color = BorderColor,
            StrokeWidth = 1.5f,
            StrokeJoin = SKStrokeJoin.Round
        })
        {
            canvas.DrawPath(cursorPath, borderPaint);
        }

        // Draw fill
        using (SKPaint fillPaint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Color = FillColor
        })
        {
            canvas.DrawPath(cursorPath, fillPaint);
        }

        canvas.Restore();
        return result;
    }
}
