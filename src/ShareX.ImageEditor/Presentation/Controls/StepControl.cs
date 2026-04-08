using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ShareX.ImageEditor.Core.Annotations;
using SkiaSharp;

namespace ShareX.ImageEditor.Presentation.Controls;

public class StepControl : Control
{
    public static readonly StyledProperty<NumberAnnotation?> AnnotationProperty =
        AvaloniaProperty.Register<StepControl, NumberAnnotation?>(nameof(Annotation));

    public NumberAnnotation? Annotation
    {
        get => GetValue(AnnotationProperty);
        set => SetValue(AnnotationProperty, value);
    }

    static StepControl()
    {
        AffectsRender<StepControl>(AnnotationProperty);
        AffectsMeasure<StepControl>(AnnotationProperty);
    }

    public StepControl()
    {
        ClipToBounds = false;
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (Annotation == null || Bounds.Width <= 0 || Bounds.Height <= 0)
        {
            return;
        }

        var annotationBounds = Annotation.GetBounds();
        var width = Math.Max(annotationBounds.Width, 2);
        var height = Math.Max(annotationBounds.Height, 2);

        var bodyGeometry = new EllipseGeometry(new Rect(0, 0, width, height));
        var tailGeometry = CreateTailGeometry(Annotation);
        Geometry geometry = tailGeometry != null
            ? new CombinedGeometry(GeometryCombineMode.Union, bodyGeometry, tailGeometry)
            : bodyGeometry;

        var strokeBrush = new SolidColorBrush(Color.Parse(Annotation.StrokeColor));
        var fillBrush = new SolidColorBrush(Color.Parse(Annotation.FillColor));
        var strokePen = new Pen(strokeBrush, Annotation.StrokeWidth);

        context.DrawGeometry(fillBrush, null, geometry);
        context.DrawGeometry(null, strokePen, geometry);

        var formattedText = new FormattedText(
            Annotation.Number.ToString(),
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            new Typeface(FontFamily.Default, FontStyle.Normal, FontWeight.Bold),
            Annotation.FontSize * 0.6,
            new SolidColorBrush(Color.Parse(Annotation.TextColor)));

        var textX = (width - formattedText.Width) / 2;
        var textY = (height - formattedText.Height) / 2;
        context.DrawText(formattedText, new Point(textX, textY));
    }

    private static Geometry? CreateTailGeometry(NumberAnnotation annotation)
    {
        return annotation.TailStyle switch
        {
            StepTailStyle.Arrow => CreateArrowTailGeometry(annotation),
            _ => CreateTriangleTailGeometry(annotation)
        };
    }

    private static Geometry? CreateTriangleTailGeometry(NumberAnnotation annotation)
    {
        if (!annotation.TryGetTailPolygon(out var tailBaseStart, out var tailTip, out var tailBaseEnd))
        {
            return null;
        }

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(ToRenderPoint(annotation, tailBaseStart), true);
            ctx.LineTo(ToRenderPoint(annotation, tailTip));
            ctx.LineTo(ToRenderPoint(annotation, tailBaseEnd));
            ctx.EndFigure(true);
        }

        return geometry;
    }

    private static Geometry? CreateArrowTailGeometry(NumberAnnotation annotation)
    {
        var tailPoint = annotation.GetDefaultTailHandlePoint();

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            var pts = ArrowAnnotation.ComputeArrowPoints(
                annotation.StartPoint.X, annotation.StartPoint.Y,
                tailPoint.X, tailPoint.Y,
                annotation.StrokeWidth * ArrowAnnotation.ArrowHeadWidthMultiplier);

            if (pts is { } p)
            {
                ctx.BeginFigure(ToRenderPoint(annotation, p.ShaftEndLeft), true);
                ctx.LineTo(ToRenderPoint(annotation, p.WingLeft));
                ctx.LineTo(ToRenderPoint(annotation, tailPoint));
                ctx.LineTo(ToRenderPoint(annotation, p.WingRight));
                ctx.LineTo(ToRenderPoint(annotation, p.ShaftEndRight));
                ctx.EndFigure(true);
            }
        }

        return geometry;
    }

    private static Point ToRenderPoint(NumberAnnotation annotation, SKPoint point)
    {
        var bounds = annotation.GetBounds();
        return new Point(point.X - bounds.Left, point.Y - bounds.Top);
    }
}
