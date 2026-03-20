using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ShareX.ImageEditor.Core.ImageEffects.Filters;
using ShareX.ImageEditor.Presentation.Controls;
using SkiaSharp;

namespace ShareX.ImageEditor.Presentation.Views.Dialogs;

public partial class RisoPrintDialog : UserControl, IEffectDialog
{
    public event EventHandler<EffectEventArgs>? ApplyRequested;
    public event EventHandler<EffectEventArgs>? PreviewRequested;
    public event EventHandler? CancelRequested;

    public RisoPrintDialog()
    {
        AvaloniaXamlLoader.Load(this);
        SubscribeColorPicker("PrimaryInkColorPicker");
        SubscribeColorPicker("SecondaryInkColorPicker");
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

    private static SKColor ToSkColor(Avalonia.Media.Color color)
    {
        return new SKColor(color.R, color.G, color.B, color.A);
    }

    private RisoPrintImageEffect CreateEffect()
    {
        Avalonia.Media.Color primaryInk = this.FindControl<ColorPickerDropdown>("PrimaryInkColorPicker")?.SelectedColorValue
            ?? Avalonia.Media.Color.Parse("#FFDC4646");
        Avalonia.Media.Color secondaryInk = this.FindControl<ColorPickerDropdown>("SecondaryInkColorPicker")?.SelectedColorValue
            ?? Avalonia.Media.Color.Parse("#FF46C8D2");

        return new RisoPrintImageEffect
        {
            InkStrength = GetValue("InkStrengthSlider", 70d),
            PaperFade = GetValue("PaperFadeSlider", 25d),
            Offset = GetValue("OffsetSlider", 3d),
            DotScale = GetValue("DotScaleSlider", 18d),
            InkNoise = GetValue("InkNoiseSlider", 35d),
            InkColorA = ToSkColor(primaryInk),
            InkColorB = ToSkColor(secondaryInk)
        };
    }

    private void OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (!IsLoaded) return;
        RequestPreview();
    }

    private void RequestPreview()
    {
        PreviewRequested?.Invoke(this, new EffectEventArgs(
            img => CreateEffect().Apply(img),
            "Riso print"));
    }

    private void OnApplyClick(object? sender, RoutedEventArgs e)
    {
        ApplyRequested?.Invoke(this, new EffectEventArgs(
            img => CreateEffect().Apply(img),
            "Applied Riso print"));
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e)
    {
        CancelRequested?.Invoke(this, EventArgs.Empty);
    }
}
