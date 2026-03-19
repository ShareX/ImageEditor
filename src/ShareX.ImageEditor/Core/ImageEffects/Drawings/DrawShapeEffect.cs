using SkiaSharp;

namespace ShareX.ImageEditor.Core.ImageEffects.Drawings;

public sealed class DrawShapeEffect : ImageEffect
{
    public DrawingShapeType Shape { get; set; } = DrawingShapeType.Rectangle;

    public DrawingPlacement Placement { get; set; } = DrawingPlacement.TopLeft;

    public SKPointI Offset { get; set; } = new SKPointI(0, 0);

    public SKSizeI Size { get; set; } = new SKSizeI(100, 100);

    public SKColor Color { get; set; } = new SKColor(255, 255, 255, 255);

    public override string Name => "Shape";

    public override ImageEffectCategory Category => ImageEffectCategory.Drawings;

    public override bool HasParameters => true;

    public override SKBitmap Apply(SKBitmap source)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        int width = ResolveDimension(Size.Width, source.Width);
        int height = ResolveDimension(Size.Height, source.Height);

        if (width <= 0 || height <= 0 || Color.Alpha == 0)
        {
            return source.Copy();
        }

        SKSizeI shapeSize = new SKSizeI(width, height);
        SKPointI shapePosition = DrawingEffectHelpers.GetPosition(
            Placement,
            Offset,
            new SKSizeI(source.Width, source.Height),
            shapeSize);

        SKRect shapeRect = new SKRect(
            shapePosition.X,
            shapePosition.Y,
            shapePosition.X + shapeSize.Width,
            shapePosition.Y + shapeSize.Height);

        SKBitmap result = source.Copy();
        using SKCanvas canvas = new SKCanvas(result);
        using SKPaint paint = new SKPaint
        {
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            Color = Color
        };

        DrawShape(canvas, paint, shapeRect);
        return result;
    }

    private static int ResolveDimension(int value, int fullSize)
    {
        return value == -1 ? fullSize : value;
    }

    private void DrawShape(SKCanvas canvas, SKPaint paint, SKRect rect)
    {
        switch (Shape)
        {
            case DrawingShapeType.RoundedRectangle:
                {
                    float radius = MathF.Max(1f, MathF.Min(rect.Width, rect.Height) * 0.2f);
                    canvas.DrawRoundRect(rect, radius, radius, paint);
                    break;
                }
            case DrawingShapeType.Ellipse:
                canvas.DrawOval(rect, paint);
                break;
            case DrawingShapeType.Triangle:
                using (SKPath trianglePath = CreateTrianglePath(rect))
                {
                    canvas.DrawPath(trianglePath, paint);
                }
                break;
            case DrawingShapeType.Diamond:
                using (SKPath diamondPath = CreateDiamondPath(rect))
                {
                    canvas.DrawPath(diamondPath, paint);
                }
                break;
            default:
                canvas.DrawRect(rect, paint);
                break;
        }
    }

    private static SKPath CreateTrianglePath(SKRect rect)
    {
        SKPath path = new SKPath();
        path.MoveTo(rect.MidX, rect.Top);
        path.LineTo(rect.Right, rect.Bottom);
        path.LineTo(rect.Left, rect.Bottom);
        path.Close();
        return path;
    }

    private static SKPath CreateDiamondPath(SKRect rect)
    {
        SKPath path = new SKPath();
        path.MoveTo(rect.MidX, rect.Top);
        path.LineTo(rect.Right, rect.MidY);
        path.LineTo(rect.MidX, rect.Bottom);
        path.LineTo(rect.Left, rect.MidY);
        path.Close();
        return path;
    }
}
