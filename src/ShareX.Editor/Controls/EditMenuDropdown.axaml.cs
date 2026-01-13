using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace ShareX.Editor.Controls
{
    public partial class EditMenuDropdown : UserControl
    {
        // Events for menu actions
        public event EventHandler? ResizeImageRequested;
        public event EventHandler? ResizeCanvasRequested;
        public event EventHandler? CropImageRequested;
        public event EventHandler? AutoCropImageRequested;
        public event EventHandler? Rotate90CWRequested;
        public event EventHandler? Rotate90CCWRequested;
        public event EventHandler? Rotate180Requested;
        public event EventHandler? RotateCustomAngleRequested;
        public event EventHandler? FlipHorizontalRequested;
        public event EventHandler? FlipVerticalRequested;

        public EditMenuDropdown()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnDropdownButtonClick(object? sender, RoutedEventArgs e)
        {
            var popup = this.FindControl<Popup>("EditPopup");
            if (popup != null)
            {
                popup.IsOpen = !popup.IsOpen;
            }
        }

        private void ClosePopup()
        {
            var popup = this.FindControl<Popup>("EditPopup");
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }

        private void OnResizeImageClick(object? sender, RoutedEventArgs e)
        {
            ClosePopup();
            ResizeImageRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnResizeCanvasClick(object? sender, RoutedEventArgs e)
        {
            ClosePopup();
            ResizeCanvasRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnCropImageClick(object? sender, RoutedEventArgs e)
        {
            ClosePopup();
            CropImageRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnAutoCropImageClick(object? sender, RoutedEventArgs e)
        {
            ClosePopup();
            AutoCropImageRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnRotate90CWClick(object? sender, RoutedEventArgs e)
        {
            ClosePopup();
            Rotate90CWRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnRotate90CCWClick(object? sender, RoutedEventArgs e)
        {
            ClosePopup();
            Rotate90CCWRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnRotate180Click(object? sender, RoutedEventArgs e)
        {
            ClosePopup();
            Rotate180Requested?.Invoke(this, EventArgs.Empty);
        }

        private void OnRotateCustomAngleClick(object? sender, RoutedEventArgs e)
        {
            ClosePopup();
            RotateCustomAngleRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnFlipHorizontalClick(object? sender, RoutedEventArgs e)
        {
            ClosePopup();
            FlipHorizontalRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnFlipVerticalClick(object? sender, RoutedEventArgs e)
        {
            ClosePopup();
            FlipVerticalRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
