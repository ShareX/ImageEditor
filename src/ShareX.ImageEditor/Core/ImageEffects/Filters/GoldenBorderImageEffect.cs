using ShareX.ImageEditor.Core.ImageEffects.Parameters;
using ShareX.ImageEditor.Presentation.Theming;
using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Filters;

public sealed class GoldenBorderImageEffect : ImageEffectBase
{
    public override string Id => "golden_border";
    public override string Name => "Golden border";
    public override ImageEffectCategory Category => ImageEffectCategory.Filters;
    public override string IconKey => LucideIcons.crown;
    public override string Description => "Adds a luxurious golden picture frame border to the image.";
    public override IReadOnlyList<EffectParameter> Parameters =>
    [
        EffectParameters.IntSlider<GoldenBorderImageEffect>("size", "Size", 5, 80, 25, (e, v) => e.Size = v),
        EffectParameters.FloatSlider<GoldenBorderImageEffect>("bevel_strength", "Bevel strength", 0, 100, 60, (e, v) => e.BevelStrength = v),
        EffectParameters.Bool<GoldenBorderImageEffect>("inner_border", "Inner border", true, (e, v) => e.InnerBorder = v),
        EffectParameters.Bool<GoldenBorderImageEffect>("outer_border", "Outer border", true, (e, v) => e.OuterBorder = v)
    ];

    public int Size { get; set; } = 25;
    public float BevelStrength { get; set; } = 60f;
    public bool InnerBorder { get; set; } = true;
    public bool OuterBorder { get; set; } = true;

    private static readonly SKColor GoldDark = new(160, 120, 40);
    private static readonly SKColor GoldBase = new(212, 175, 55);
    private static readonly SKColor GoldLight = new(255, 223, 100);
    private static readonly SKColor GoldHighlight = new(255, 240, 160);

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));

        int size = Math.Clamp(Size, 5, 80);
        float bevel = Math.Clamp(BevelStrength, 0f, 100f) / 100f;

        int newWidth = source.Width + size * 2;
        int newHeight = source.Height + size * 2;

        SKBitmap result = new(newWidth, newHeight);
        using SKCanvas canvas = new(result);
        canvas.Clear(SKColors.Transparent);

        // Draw the golden frame
        DrawGoldenFrame(canvas, newWidth, newHeight, size, bevel);

        // Draw the image centered
        canvas.DrawBitmap(source, size, size);

        // Inner border line
        if (InnerBorder)
        {
            using SKPaint innerPaint = new()
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1.5f,
                Color = GoldDark
            };
            canvas.DrawRect(size - 1, size - 1, source.Width + 2, source.Height + 2, innerPaint);
        }

        // Outer border line
        if (OuterBorder)
        {
            using SKPaint outerPaint = new()
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1.5f,
                Color = GoldDark
            };
            canvas.DrawRect(0.5f, 0.5f, newWidth - 1, newHeight - 1, outerPaint);
        }

        return result;
    }

    private static void DrawGoldenFrame(SKCanvas canvas, int width, int height, int size, float bevel)
    {
        // Base gold fill
        using (SKPaint basePaint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Color = GoldBase
        })
        {
            // Top strip
            canvas.DrawRect(0, 0, width, size, basePaint);
            // Bottom strip
            canvas.DrawRect(0, height - size, width, size, basePaint);
            // Left strip
            canvas.DrawRect(0, 0, size, height, basePaint);
            // Right strip
            canvas.DrawRect(width - size, 0, size, height, basePaint);
        }

        if (bevel <= 0f) return;

        // Outer bevel: light on top-left edges, dark on bottom-right
        float bevelWidth = size * 0.35f * bevel;

        // Top highlight (light from top-left)
        using (SKPaint highlightPaint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(0, bevelWidth),
                [GoldHighlight.WithAlpha((byte)(200 * bevel)), GoldBase.WithAlpha(0)],
                SKShaderTileMode.Clamp)
        })
        {
            canvas.DrawRect(0, 0, width, bevelWidth, highlightPaint);
        }

        // Left highlight
        using (SKPaint leftHighPaint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(bevelWidth, 0),
                [GoldHighlight.WithAlpha((byte)(180 * bevel)), GoldBase.WithAlpha(0)],
                SKShaderTileMode.Clamp)
        })
        {
            canvas.DrawRect(0, 0, bevelWidth, height, leftHighPaint);
        }

        // Bottom shadow
        using (SKPaint bottomShadow = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, height),
                new SKPoint(0, height - bevelWidth),
                [GoldDark.WithAlpha((byte)(200 * bevel)), GoldBase.WithAlpha(0)],
                SKShaderTileMode.Clamp)
        })
        {
            canvas.DrawRect(0, height - bevelWidth, width, bevelWidth, bottomShadow);
        }

        // Right shadow
        using (SKPaint rightShadow = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(width, 0),
                new SKPoint(width - bevelWidth, 0),
                [GoldDark.WithAlpha((byte)(200 * bevel)), GoldBase.WithAlpha(0)],
                SKShaderTileMode.Clamp)
        })
        {
            canvas.DrawRect(width - bevelWidth, 0, bevelWidth, height, rightShadow);
        }

        // Inner bevel (opposite: dark on top-left, light on bottom-right, near the image)
        float innerBevelWidth = size * 0.25f * bevel;
        float innerOffset = size - innerBevelWidth;

        // Inner top (shadow)
        using (SKPaint innerTopPaint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, size),
                new SKPoint(0, size - innerBevelWidth),
                [GoldDark.WithAlpha((byte)(160 * bevel)), GoldBase.WithAlpha(0)],
                SKShaderTileMode.Clamp)
        })
        {
            canvas.DrawRect(size, size - innerBevelWidth, width - size * 2, innerBevelWidth, innerTopPaint);
        }

        // Inner left (shadow)
        using (SKPaint innerLeftPaint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(size, 0),
                new SKPoint(size - innerBevelWidth, 0),
                [GoldDark.WithAlpha((byte)(140 * bevel)), GoldBase.WithAlpha(0)],
                SKShaderTileMode.Clamp)
        })
        {
            canvas.DrawRect(size - innerBevelWidth, size, innerBevelWidth, height - size * 2, innerLeftPaint);
        }

        // Inner bottom (highlight)
        using (SKPaint innerBottomPaint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, height - size),
                new SKPoint(0, height - size + innerBevelWidth),
                [GoldLight.WithAlpha((byte)(140 * bevel)), GoldBase.WithAlpha(0)],
                SKShaderTileMode.Clamp)
        })
        {
            canvas.DrawRect(size, height - size, width - size * 2, innerBevelWidth, innerBottomPaint);
        }

        // Inner right (highlight)
        using (SKPaint innerRightPaint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(width - size, 0),
                new SKPoint(width - size + innerBevelWidth, 0),
                [GoldLight.WithAlpha((byte)(140 * bevel)), GoldBase.WithAlpha(0)],
                SKShaderTileMode.Clamp)
        })
        {
            canvas.DrawRect(width - size, size, innerBevelWidth, height - size * 2, innerRightPaint);
        }

        // Center groove line
        float grooveY = size * 0.5f;
        using (SKPaint grooveDark = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1,
            Color = GoldDark.WithAlpha((byte)(120 * bevel))
        })
        {
            // Horizontal grooves
            canvas.DrawLine(0, grooveY, width, grooveY, grooveDark);
            canvas.DrawLine(0, height - grooveY, width, height - grooveY, grooveDark);
            // Vertical grooves
            canvas.DrawLine(grooveY, 0, grooveY, height, grooveDark);
            canvas.DrawLine(width - grooveY, 0, width - grooveY, height, grooveDark);
        }

        using (SKPaint grooveLight = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1,
            Color = GoldHighlight.WithAlpha((byte)(80 * bevel))
        })
        {
            canvas.DrawLine(0, grooveY + 1, width, grooveY + 1, grooveLight);
            canvas.DrawLine(0, height - grooveY + 1, width, height - grooveY + 1, grooveLight);
            canvas.DrawLine(grooveY + 1, 0, grooveY + 1, height, grooveLight);
            canvas.DrawLine(width - grooveY + 1, 0, width - grooveY + 1, height, grooveLight);
        }
    }
}
