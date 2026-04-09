using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ShareX.ImageEditor.Core.Annotations;

namespace ShareX.ImageEditor.Presentation.Controls
{
    /// <summary>
    /// Selection shell for spotlight annotations.
    /// The actual darkening overlay is rendered once by SpotlightOverlayControl.
    /// </summary>
    public class SpotlightControl : Control
    {

        public static readonly StyledProperty<SpotlightAnnotation?> AnnotationProperty =
            AvaloniaProperty.Register<SpotlightControl, SpotlightAnnotation?>(nameof(Annotation));

        public SpotlightAnnotation? Annotation
        {
            get => GetValue(AnnotationProperty);
            set => SetValue(AnnotationProperty, value);
        }

        static SpotlightControl()
        {
            AffectsRender<SpotlightControl>(AnnotationProperty);
        }

        public SpotlightControl()
        {
            // Make this control take up the full canvas space
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
        }
    }
}
