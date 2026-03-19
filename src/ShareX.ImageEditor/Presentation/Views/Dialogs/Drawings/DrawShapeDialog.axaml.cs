using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ShareX.ImageEditor.Core.ImageEffects.Drawings;
using ShareX.ImageEditor.Presentation.Controls;
using SkiaSharp;

namespace ShareX.ImageEditor.Presentation.Views.Dialogs
{
    public partial class DrawShapeDialog : UserControl, IEffectDialog
    {
        private readonly DrawShapeEffect _defaultEffect = new();

        public event EventHandler<EffectEventArgs>? ApplyRequested;
        public event EventHandler<EffectEventArgs>? PreviewRequested;
        public event EventHandler? CancelRequested;

        public DrawShapeDialog()
        {
            AvaloniaXamlLoader.Load(this);
            ApplyDefaults();
            SubscribeColorPicker("ShapeColorPicker");
            AttachedToVisualTree += (s, e) => RequestPreview();
        }

        private void ApplyDefaults()
        {
            if (this.FindControl<ComboBox>("ShapeComboBox") is ComboBox shapeComboBox)
            {
                shapeComboBox.SelectedIndex = GetShapeIndex(_defaultEffect.Shape);
            }

            if (this.FindControl<ComboBox>("PlacementComboBox") is ComboBox placementComboBox)
            {
                placementComboBox.SelectedIndex = GetPlacementIndex(_defaultEffect.Placement);
            }

            if (this.FindControl<NumericUpDown>("OffsetXInput") is NumericUpDown offsetXInput)
            {
                offsetXInput.Value = _defaultEffect.Offset.X;
            }

            if (this.FindControl<NumericUpDown>("OffsetYInput") is NumericUpDown offsetYInput)
            {
                offsetYInput.Value = _defaultEffect.Offset.Y;
            }

            if (this.FindControl<NumericUpDown>("WidthInput") is NumericUpDown widthInput)
            {
                widthInput.Value = _defaultEffect.Size.Width;
            }

            if (this.FindControl<NumericUpDown>("HeightInput") is NumericUpDown heightInput)
            {
                heightInput.Value = _defaultEffect.Size.Height;
            }

            if (this.FindControl<ColorPickerDropdown>("ShapeColorPicker") is ColorPickerDropdown colorPicker)
            {
                colorPicker.SelectedColorValue = ToAvaloniaColor(_defaultEffect.Color);
            }
        }

        private void SubscribeColorPicker(string name)
        {
            ColorPickerDropdown? picker = this.FindControl<ColorPickerDropdown>(name);
            if (picker != null)
            {
                picker.PropertyChanged += OnColorPickerPropertyChanged;
            }
        }

        private void OnColorPickerPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == ColorPickerDropdown.SelectedColorValueProperty && IsLoaded)
            {
                RequestPreview();
            }
        }

        private int GetInt(string name, int fallback)
        {
            NumericUpDown? control = this.FindControl<NumericUpDown>(name);
            return (int)Math.Round(control?.Value ?? fallback);
        }

        private static int GetShapeIndex(DrawingShapeType shapeType)
        {
            return shapeType switch
            {
                DrawingShapeType.RoundedRectangle => 1,
                DrawingShapeType.Ellipse => 2,
                DrawingShapeType.Triangle => 3,
                DrawingShapeType.Diamond => 4,
                _ => 0
            };
        }

        private static int GetPlacementIndex(DrawingPlacement placement)
        {
            return placement switch
            {
                DrawingPlacement.TopCenter => 1,
                DrawingPlacement.TopRight => 2,
                DrawingPlacement.MiddleLeft => 3,
                DrawingPlacement.MiddleCenter => 4,
                DrawingPlacement.MiddleRight => 5,
                DrawingPlacement.BottomLeft => 6,
                DrawingPlacement.BottomCenter => 7,
                DrawingPlacement.BottomRight => 8,
                _ => 0
            };
        }

        private DrawingShapeType GetShapeType()
        {
            return this.FindControl<ComboBox>("ShapeComboBox")?.SelectedIndex switch
            {
                1 => DrawingShapeType.RoundedRectangle,
                2 => DrawingShapeType.Ellipse,
                3 => DrawingShapeType.Triangle,
                4 => DrawingShapeType.Diamond,
                _ => DrawingShapeType.Rectangle
            };
        }

        private DrawingPlacement GetPlacement()
        {
            return this.FindControl<ComboBox>("PlacementComboBox")?.SelectedIndex switch
            {
                1 => DrawingPlacement.TopCenter,
                2 => DrawingPlacement.TopRight,
                3 => DrawingPlacement.MiddleLeft,
                4 => DrawingPlacement.MiddleCenter,
                5 => DrawingPlacement.MiddleRight,
                6 => DrawingPlacement.BottomLeft,
                7 => DrawingPlacement.BottomCenter,
                8 => DrawingPlacement.BottomRight,
                _ => DrawingPlacement.TopLeft
            };
        }

        private static SKColor ToSkColor(Avalonia.Media.Color color)
        {
            return new SKColor(color.R, color.G, color.B, color.A);
        }

        private static Avalonia.Media.Color ToAvaloniaColor(SKColor color)
        {
            return Avalonia.Media.Color.FromArgb(color.Alpha, color.Red, color.Green, color.Blue);
        }

        private DrawShapeEffect CreateEffect()
        {
            ColorPickerDropdown? colorPicker = this.FindControl<ColorPickerDropdown>("ShapeColorPicker");

            return new DrawShapeEffect
            {
                Shape = GetShapeType(),
                Placement = GetPlacement(),
                Offset = new SKPointI(GetInt("OffsetXInput", _defaultEffect.Offset.X), GetInt("OffsetYInput", _defaultEffect.Offset.Y)),
                Size = new SKSizeI(GetInt("WidthInput", _defaultEffect.Size.Width), GetInt("HeightInput", _defaultEffect.Size.Height)),
                Color = ToSkColor(colorPicker?.SelectedColorValue ?? ToAvaloniaColor(_defaultEffect.Color))
            };
        }

        private void RequestPreview()
        {
            PreviewRequested?.Invoke(this, new EffectEventArgs(
                img => CreateEffect().Apply(img),
                "Shape"));
        }

        private void OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) RequestPreview();
        }

        private void OnNumericChanged(object? sender, NumericUpDownValueChangedEventArgs e)
        {
            if (IsLoaded) RequestPreview();
        }

        private void OnApplyClick(object? sender, RoutedEventArgs e)
        {
            ApplyRequested?.Invoke(this, new EffectEventArgs(
                img => CreateEffect().Apply(img),
                "Applied shape"));
        }

        private void OnCancelClick(object? sender, RoutedEventArgs e)
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
