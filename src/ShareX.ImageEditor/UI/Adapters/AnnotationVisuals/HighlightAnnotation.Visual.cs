using Avalonia.Controls;
using Avalonia.Media;

namespace ShareX.ImageEditor.Annotations;

public partial class HighlightAnnotation
{
    /// <summary>
    /// Creates the Avalonia visual for this annotation.
    /// The visual is a transparent rectangle used for hit-testing and selection handles only.
    /// The actual highlight color is rendered in the Skia layer (see EditorCore.Render).
    /// </summary>
    public Control CreateVisual()
    {
        return new Avalonia.Controls.Shapes.Rectangle
        {
            Fill = Brushes.Transparent,
            Stroke = Brushes.Transparent,
            StrokeThickness = 0,
            Tag = this
        };
    }
}
