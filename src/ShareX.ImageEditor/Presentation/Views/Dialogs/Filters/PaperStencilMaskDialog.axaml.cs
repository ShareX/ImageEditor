using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ShareX.ImageEditor.Core.ImageEffects.Filters;
using ShareX.ImageEditor.Presentation.Controls;
using SkiaSharp;

namespace ShareX.ImageEditor.Presentation.Views.Dialogs;

public partial class PaperStencilMaskDialog : UserControl, IEffectDialog
{
    public event EventHandler<EffectEventArgs>? ApplyRequested;
    public event EventHandler<EffectEventArgs>? PreviewRequested;
    public event EventHandler? CancelRequested;

    public PaperStencilMaskDialog()
    {
        AvaloniaXamlLoader.Load(this);
        SubscribeColorPicker("StencilColorPicker");
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

    private PaperStencilMaskImageEffect CreateEffect()
    {
        Avalonia.Media.Color stencilColor = this.FindControl<ColorPickerDropdown>("StencilColorPicker")?.SelectedColorValue
            ?? Avalonia.Media.Color.Parse("#DC000000");

        return new PaperStencilMaskImageEffect
        {
            Threshold = GetValue("ThresholdSlider", 140d),
            FeatherRadius = GetValue("FeatherRadiusSlider", 8d),
            EdgeStrength = GetValue("EdgeStrengthSlider", 70d),
            BackgroundDim = GetValue("BackgroundDimSlider", 35d),
            InvertMask = GetBool("InvertMaskCheckBox"),
            StencilColor = ToSkColor(stencilColor)
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
            "Paper stencil mask"));
    }

    private void OnApplyClick(object? sender, RoutedEventArgs e)
    {
        ApplyRequested?.Invoke(this, new EffectEventArgs(
            img => CreateEffect().Apply(img),
            "Applied Paper stencil mask"));
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        CancelRequested?.Invoke(this, EventArgs.Empty);
    }
}
