using ShareX.ImageEditor.Core.ImageEffects.Helpers;
using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class BloodyBorderImageEffect : ImageEffectBase
{
    public override string Id => "bloody_border";
    public override string Name => "Bloody border";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.skull;
    public override string Description => "Adds a dripping blood-style border to the edges of the image.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<BloodyBorderImageEffect>("drip_count", "Drip count", 5, 100, 30, (e, v) => e.DripCount = v),
        EffectParameters.IntSlider<BloodyBorderImageEffect>("min_length", "Min drip length", 5, 100, 15, (e, v) => e.MinLength = v),
        EffectParameters.IntSlider<BloodyBorderImageEffect>("max_length", "Max drip length", 20, 300, 80, (e, v) => e.MaxLength = v),
        EffectParameters.IntSlider<BloodyBorderImageEffect>("border_width", "Border width", 2, 40, 8, (e, v) => e.BorderWidth = v),
        EffectParameters.Color<BloodyBorderImageEffect>("color", "Color", new SKColor(139, 0, 0), (e, v) => e.Color = v),
        EffectParameters.FloatSlider<BloodyBorderImageEffect>("opacity", "Opacity", 0, 100, 90, (e, v) => e.Opacity = v),
        EffectParameters.Bool<BloodyBorderImageEffect>("top_edge", "Top edge", true, (e, v) => e.TopEdge = v),
        EffectParameters.Bool<BloodyBorderImageEffect>("bottom_edge", "Bottom edge", false, (e, v) => e.BottomEdge = v)
    ];

    public int DripCount { get; set; } = 30;
    public int MinLength { get; set; } = 15;
    public int MaxLength { get; set; } = 80;
    public int BorderWidth { get; set; } = 8;
    public SKColor Color { get; set; } = new SKColor(139, 0, 0);
    public float Opacity { get; set; } = 90f;
    public bool TopEdge { get; set; } = true;
    public bool BottomEdge { get; set; }

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        float opacity = Math.Clamp(Opacity, 0f, 100f) / 100f;
        if (opacity <= 0f || (!TopEdge && !BottomEdge)) return source.Copy();

        int dripCount = Math.Clamp(DripCount, 5, 100);
        int minLen = Math.Max(5, Math.Min(MinLength, MaxLength));
        int maxLen = Math.Max(minLen, MaxLength);
        int borderW = Math.Clamp(BorderWidth, 2, 40);

        byte alpha = (byte)(255 * opacity);
        SKColor bloodColor = Color.WithAlpha(alpha);

        SKBitmap result = source.Copy();
        using SKCanvas canvas = new(result);

        if (TopEdge)
        {
            DrawBloodyEdge(canvas, source.Width, borderW, dripCount, minLen, maxLen, bloodColor, false);
        }

        if (BottomEdge)
        {
            canvas.Save();
            canvas.Translate(0, source.Height);
            canvas.Scale(1, -1);
            DrawBloodyEdge(canvas, source.Width, borderW, dripCount, minLen, maxLen, bloodColor, true);
            canvas.Restore();
        }

        return result;
    }

    private static void DrawBloodyEdge(SKCanvas canvas, int width, int borderW, int dripCount, int minLen, int maxLen, SKColor color, bool isBottom)
    {
        // Draw the solid border strip along the edge
        using (SKPaint borderPaint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Color = color
        })
        {
            canvas.DrawRect(0, 0, width, borderW, borderPaint);
        }

        // Draw drips
        for (int i = 0; i < dripCount; i++)
        {
            float x = Random.Shared.Next(0, width);
            float dripLength = Random.Shared.Next(minLen, maxLen + 1);
            float dripWidth = borderW * (0.3f + Random.Shared.NextSingle() * 0.7f);

            // Darken or lighten slightly for variation
            float variation = 0.85f + Random.Shared.NextSingle() * 0.3f;
            SKColor dripColor = new(
                (byte)Math.Clamp(color.Red * variation, 0, 255),
                (byte)Math.Clamp(color.Green * variation, 0, 255),
                (byte)Math.Clamp(color.Blue * variation, 0, 255),
                color.Alpha);

            using SKPath dripPath = new();
            dripPath.MoveTo(x - dripWidth / 2, borderW * 0.5f);

            // Slightly wavy drip using cubic curves
            float midY = borderW + dripLength * 0.5f;
            float wobble = (Random.Shared.NextSingle() - 0.5f) * dripWidth * 2f;

            dripPath.CubicTo(
                x - dripWidth / 2 + wobble * 0.3f, midY * 0.5f,
                x + wobble, midY,
                x, borderW + dripLength);

            // Come back up the other side
            dripPath.CubicTo(
                x - wobble * 0.5f, midY,
                x + dripWidth / 2 - wobble * 0.3f, midY * 0.5f,
                x + dripWidth / 2, borderW * 0.5f);

            dripPath.Close();

            using SKPaint dripPaint = new()
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = dripColor
            };

            canvas.DrawPath(dripPath, dripPaint);

            // Add a small bulb at the tip of some drips
            if (Random.Shared.NextSingle() > 0.4f)
            {
                float bulbRadius = dripWidth * 0.6f;
                using SKPaint bulbPaint = new()
                {
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill,
                    Color = dripColor
                };
                canvas.DrawOval(x, borderW + dripLength, bulbRadius, bulbRadius * 1.2f, bulbPaint);
            }
        }
    }
}
