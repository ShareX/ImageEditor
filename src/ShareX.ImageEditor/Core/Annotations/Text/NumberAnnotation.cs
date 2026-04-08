#region License Information (GPL v3)

/*
    ShareX.ImageEditor - The UI-agnostic Editor library for ShareX
    Copyright (c) 2007-2026 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using SkiaSharp;
using System.Text.Json.Serialization;

namespace ShareX.ImageEditor.Core.Annotations;

/// <summary>
/// Number annotation - auto-incrementing numbered circle markers
/// </summary>
public partial class NumberAnnotation : Annotation
{
    private const float GeometryEpsilon = 0.001f;
    private const float TailWidthMultiplier = 1.5f;

    public override AnnotationCategory Category => AnnotationCategory.Text;
    /// <summary>
    /// Number to display (typically auto-incremented)
    /// </summary>
    public int Number { get; set; } = 1;

    /// <summary>
    /// Font size for the number
    /// </summary>
    public float FontSize { get; set; } = 24;

    /// <summary>
    /// Text body color
    /// </summary>
    public string TextColor { get; set; } = "#FFFFFFFF";

    /// <summary>
    /// Circle radius - auto-calculated based on FontSize if not explicitly set
    /// </summary>
    public float Radius
    {
        get => CalculateRadius();
        set { } // Allow setting but use calculated value
    }

    /// <summary>
    /// Tail point (absolute position). The tail is only rendered after this point is explicitly set.
    /// </summary>
    public SKPoint TailPoint { get; set; }

    /// <summary>
    /// Tracks whether the tail point was explicitly initialized.
    /// </summary>
    public bool TailPointInitialized { get; set; }

    /// <summary>
    /// Calculate radius based on font size to ensure text fits
    /// </summary>
    private float CalculateRadius()
    {
        // Radius should be about 70% of FontSize to properly contain the number
        // with some padding around it
        return Math.Max(12, FontSize * 0.7f);
    }

    public NumberAnnotation()
    {
        ToolType = EditorTool.Step;
    }

    [JsonIgnore]
    public bool HasTailPoint => TailPointInitialized;

    public SKPoint GetDefaultTailHandlePoint()
    {
        var bounds = GetBounds();
        // For arrow tails, the tail point needs to be outside the circle so the
        // arrowhead geometry (which places the shaft base headHeight before the tip)
        // doesn't end up inside the circle body. Use 2x radius offset so the default
        // tail is well outside the circle with visible shaft length.
        return new SKPoint(bounds.Right + Radius, bounds.Bottom + Radius);
    }

    public SKPoint GetTailHandlePoint() => HasTailPoint ? TailPoint : GetDefaultTailHandlePoint();

    public void SetTailPoint(SKPoint tailPoint)
    {
        TailPoint = tailPoint;
        TailPointInitialized = true;
    }

    public bool IsTailVisible()
    {
        return TryGetTailPolygon(out _, out _, out _);
    }

    public SKRect GetInteractionBounds(float tolerance = 0)
    {
        var interactionBounds = GetBounds();

        if (TryGetTailPolygon(out var tailBaseStart, out var tailTip, out var tailBaseEnd))
        {
            var tailBounds = new SKRect(
                MathF.Min(tailBaseStart.X, MathF.Min(tailTip.X, tailBaseEnd.X)),
                MathF.Min(tailBaseStart.Y, MathF.Min(tailTip.Y, tailBaseEnd.Y)),
                MathF.Max(tailBaseStart.X, MathF.Max(tailTip.X, tailBaseEnd.X)),
                MathF.Max(tailBaseStart.Y, MathF.Max(tailTip.Y, tailBaseEnd.Y)));
            interactionBounds = SKRect.Union(interactionBounds, tailBounds);
        }

        if (tolerance > 0)
        {
            interactionBounds = SKRect.Inflate(interactionBounds, tolerance, tolerance);
        }

        return interactionBounds;
    }

    public bool TryGetTailPolygon(out SKPoint tailBaseStart, out SKPoint tailTip, out SKPoint tailBaseEnd)
    {
        tailBaseStart = default;
        tailTip = default;
        tailBaseEnd = default;

        if (!HasTailPoint)
        {
            return false;
        }

        var center = StartPoint;
        float radius = Radius;
        if (radius <= GeometryEpsilon)
        {
            return false;
        }

        tailTip = TailPoint;

        float directionX = tailTip.X - center.X;
        float directionY = tailTip.Y - center.Y;
        float directionLength = MathF.Sqrt(directionX * directionX + directionY * directionY);
        if (directionLength <= radius + GeometryEpsilon)
        {
            return false;
        }

        float normalizedDirectionX = directionX / directionLength;
        float normalizedDirectionY = directionY / directionLength;

        float tailWidth = radius * TailWidthMultiplier;
        float halfTailWidth = tailWidth / 2f;

        var perpendicular = new SKPoint(-normalizedDirectionY, normalizedDirectionX);
        var baseStart = new SKPoint(
            center.X + perpendicular.X * halfTailWidth,
            center.Y + perpendicular.Y * halfTailWidth);
        var baseEnd = new SKPoint(
            center.X - perpendicular.X * halfTailWidth,
            center.Y - perpendicular.Y * halfTailWidth);

        return TryGetCircleSegmentExitPoint(center, radius, baseStart, tailTip, out tailBaseStart) &&
               TryGetCircleSegmentExitPoint(center, radius, baseEnd, tailTip, out tailBaseEnd);
    }


    public override bool HitTest(SKPoint point, float tolerance = 5)
    {
        var dx = point.X - StartPoint.X;
        var dy = point.Y - StartPoint.Y;
        var distance = (float)Math.Sqrt(dx * dx + dy * dy);
        if (distance <= (Radius + tolerance))
        {
            return true;
        }

        if (!TryGetTailPolygon(out var tailBaseStart, out var tailTip, out var tailBaseEnd))
        {
            return false;
        }

        if (PointInTriangle(point, tailBaseStart, tailTip, tailBaseEnd))
        {
            return true;
        }

        return DistanceToSegment(point, tailBaseStart, tailTip) <= tolerance ||
               DistanceToSegment(point, tailTip, tailBaseEnd) <= tolerance ||
               DistanceToSegment(point, tailBaseEnd, tailBaseStart) <= tolerance;
    }

    public override SKRect GetBounds()
    {
        return new SKRect(
            StartPoint.X - Radius,
            StartPoint.Y - Radius,
            StartPoint.X + Radius,
            StartPoint.Y + Radius);
    }

    private static bool TryGetCircleSegmentExitPoint(SKPoint center, float radius, SKPoint start, SKPoint end, out SKPoint intersection)
    {
        float dx = end.X - start.X;
        float dy = end.Y - start.Y;

        float offsetX = start.X - center.X;
        float offsetY = start.Y - center.Y;

        float a = (dx * dx) + (dy * dy);
        float b = 2f * ((offsetX * dx) + (offsetY * dy));
        float c = (offsetX * offsetX) + (offsetY * offsetY) - (radius * radius);

        float discriminant = (b * b) - (4f * a * c);
        if (a <= GeometryEpsilon || discriminant < 0)
        {
            intersection = default;
            return false;
        }

        float sqrtDiscriminant = MathF.Sqrt(discriminant);
        float t1 = (-b - sqrtDiscriminant) / (2f * a);
        float t2 = (-b + sqrtDiscriminant) / (2f * a);
        float t = MathF.Max(t1, t2);

        if (t < -GeometryEpsilon || t > 1f + GeometryEpsilon)
        {
            intersection = default;
            return false;
        }

        t = Math.Clamp(t, 0f, 1f);
        intersection = new SKPoint(start.X + (dx * t), start.Y + (dy * t));
        return true;
    }

    private static bool PointInTriangle(SKPoint point, SKPoint a, SKPoint b, SKPoint c)
    {
        float d1 = Sign(point, a, b);
        float d2 = Sign(point, b, c);
        float d3 = Sign(point, c, a);

        bool hasNegative = d1 < 0 || d2 < 0 || d3 < 0;
        bool hasPositive = d1 > 0 || d2 > 0 || d3 > 0;

        return !(hasNegative && hasPositive);
    }

    private static float Sign(SKPoint p1, SKPoint p2, SKPoint p3)
    {
        return (p1.X - p3.X) * (p2.Y - p3.Y) -
               (p2.X - p3.X) * (p1.Y - p3.Y);
    }

    private static float DistanceToSegment(SKPoint point, SKPoint start, SKPoint end)
    {
        float dx = end.X - start.X;
        float dy = end.Y - start.Y;
        float segmentLengthSquared = dx * dx + dy * dy;

        if (segmentLengthSquared <= GeometryEpsilon)
        {
            return MathF.Sqrt((point.X - start.X) * (point.X - start.X) + (point.Y - start.Y) * (point.Y - start.Y));
        }

        float t = ((point.X - start.X) * dx + (point.Y - start.Y) * dy) / segmentLengthSquared;
        t = Math.Clamp(t, 0f, 1f);

        float projectionX = start.X + t * dx;
        float projectionY = start.Y + t * dy;
        float deltaX = point.X - projectionX;
        float deltaY = point.Y - projectionY;

        return MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
    }
}
