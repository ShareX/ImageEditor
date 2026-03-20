using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ShareX.ImageEditor.Core.ImageEffects.Filters;
using ShareX.ImageEditor.Presentation.Controls;
using SkiaSharp;

namespace ShareX.ImageEditor.Presentation.Views.Dialogs;

public partial class LuminanceContourLinesDialog : UserControl, IEffectDialog
{
    public event EventHandler<EffectEventArgs>? ApplyRequested;
    public event EventHandler<EffectEventArgs>? PreviewRequested;
    public event EventHandler? CancelRequested;

    public LuminanceContourLinesDialog()
    {
        AvaloniaXamlLoader.Load(this);
        SubscribeColorPicker("LineColorPicker");
        AttachedToVisualTree += (s, e) => RequestPreview();
    }

    private void SubscribeColorPicker(string controlName)
    {
        ColorPickerDropdown? picker = this.FindControl<ColorPickerDropdown>(controlName);
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

    private float GetValue(string controlName, double fallback)
    {
        return (float)(this.FindControl<Slider>(controlName)?.Value ?? fallback);
    }

    private bool GetBool(string controlName, bool fallback = false)
    {
        return this.FindControl<CheckBox>(controlName)?.IsChecked ?? fallback;
    }

    private static SKColor ToSkColor(Avalonia.Media.Color color)
    {
        return new SKColor(color.R, color.G, color.B, color.A);
    }

    private LuminanceContourLinesImageEffect CreateEffect()
    {
        Avalonia.Media.Color lineColor = this.FindControl<ColorPickerDropdown>("LineColorPicker")?.SelectedColorValue
            ?? Avalonia.Media.Colors.Black;

        return new LuminanceContourLinesImageEffect
        {
            Levels = (int)Math.Round(GetValue("LevelsSlider", 12d)),
            LineWidth = GetValue("LineWidthSlider", 6d),
            LineStrength = GetValue("LineStrengthSlider", 65d),
            BackgroundStrength = GetValue("BackgroundStrengthSlider", 20d),
            Threshold = GetValue("ThresholdSlider", 0d),
            Invert = GetBool("InvertCheckBox"),
            LineColor = ToSkColor(lineColor)
        };
    }

    private void OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (!IsLoaded) return;
        RequestPreview();
    }

    private void OnSettingChanged(object? sender, RoutedEventArgs e)
    {
        if (!IsLoaded) return;
        RequestPreview();
    }

    private void RequestPreview()
    {
        PreviewRequested?.Invoke(this, new EffectEventArgs(
            img => CreateEffect().Apply(img),
            "Luminance contour lines"));
    }

    private void OnApplyClick(object? sender, RoutedEventArgs e)
    {
        ApplyRequested?.Invoke(this, new EffectEventArgs(
            img => CreateEffect().Apply(img),
            "Applied Luminance contour lines"));
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        CancelRequested?.Invoke(this, EventArgs.Empty);
    }
}
